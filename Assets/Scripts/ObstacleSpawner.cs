#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    [SerializeField] Obstacle[] obstaclePrefab;
    public bool Spawn = true;
    public int maxDepth = 1;
    [SerializeField] PlayerControl playerControl;
    [SerializeField] ObstacleTracker obstacleTracker;
    public CameraShake cam;  // Passed to obstacles

    private readonly int COOLDOWN_AFTER_USE = 4;  // TODO: define globally so you don't need to update this and the cooldown images
    private readonly float LANE_WIDTH = 1.0f;

    private List<AbilityTree> historicPossiblePaths = new List<AbilityTree>();
    private List<Ability[]> historicPaths = new List<Ability[]>();

    private class AbilityTree
    {
        public Ability ability;
        public Dictionary<Ability, int> cooldowns;
        public int depth;
        public List<AbilityTree> children = new List<AbilityTree>();
        public AbilityTree? parent;

        public AbilityTree(Ability ability = Ability.NoAbility, int depth = 0, AbilityTree? parent = null, Dictionary<Ability, int> cooldowns = null)
        {
            this.ability = ability;
            this.depth = depth;
            this.parent = parent;
            if (cooldowns == null) { cooldowns = new Dictionary<Ability, int>(); }
            this.cooldowns = cooldowns;
        }
    }

    public void Deepen()
    {
        maxDepth++;  // TODO: make setter
        obstacleTracker.Deepen();
    }

    public void SpawnObstacles()
    {
        AbilityTree possiblePaths = CalculatePossiblePaths();
        Ability[] pathToGenerate = PathToRandomLeaf(possiblePaths);
        Ability obstacleToGenerate = pathToGenerate.Last();
        Ability[] obstaclesToGenerate = GenerateObstacles(obstacleToGenerate);
        AddObstacles(obstaclesToGenerate);
        SpawnObstacles(obstaclesToGenerate);
        CheckPathIsWrong(pathToGenerate);
        Debug.Log(string.Join(", ", pathToGenerate));
        historicPossiblePaths.Add(possiblePaths);
        historicPaths.Add(pathToGenerate);
    }

    private void CheckPathIsWrong(Ability[] pathToGenerate)
    {
        foreach (Ability ability in Enum.GetValues(typeof(Ability)))
        {
            if (ability != Ability.NoAbility & pathToGenerate.Count(a=>a==ability)>=2)
            {
                Debug.Log("Something is up");
            }
        }
    }

    private void SpawnObstacles(Ability[] obstaclesToGenerate)
    {
        List<Obstacle?> obstacles = new List<Obstacle>();
        int lane = -1;  // The ObstacleSpawner is located in the center, so we spawn at relative lanes -1, 0 and 1
        foreach (Ability obstacleType in obstaclesToGenerate)
        {
            if (obstacleType != Ability.NoAbility)
            {
                Obstacle prefab = obstaclePrefab[(int)obstacleType - 1];
                Vector3 position = new Vector3(
                    transform.position.x + prefab.transform.position.x, 
                    transform.position.y + prefab.transform.position.y, 
                    transform.position.z + prefab.transform.position.z + lane * LANE_WIDTH);
                Obstacle obstacle = Instantiate(prefab, position, prefab.transform.rotation);  // Obstacles start at 1 (0 is for NoAbility)
                obstacle.cam = cam;
                obstacle.column = maxDepth;
                obstacles.Add(obstacle);
            } else
            {
                obstacles.Add(null);
            }
            lane++;
        }
        obstacleTracker.SpawnObstacles(obstacles.ToArray());
    }

    private void AddObstacles(Ability[] obstaclesToGenerate)
    {
        Ability[][] existingObstacles = obstacleTracker.GetObstacleTypes();
        for(int i = 0; i< existingObstacles.Count()-1; i++)
        {
            existingObstacles[i] = existingObstacles[i + 1];
        }
        existingObstacles[existingObstacles.Count()-1] = obstaclesToGenerate;
    }

    private Ability[] GenerateObstacles(Ability guaranteedObstacle)
    {
        List<Ability> abilities = new List<Ability>();
        abilities.Add(guaranteedObstacle);
        abilities.Add(RandomObstacle());
        abilities.Add(RandomObstacle());
        return abilities.OrderBy(a => UnityEngine.Random.value).ToArray();  // Shuffle to place guaranteedObstacle in random lane
    }

    private Ability RandomObstacle()
    {
        int random = UnityEngine.Random.Range(1, 4);
        return (Ability)Enum.GetValues(typeof(Ability)).GetValue(random);
    }

    private Ability[] PathToRandomLeaf(AbilityTree possiblePaths)
    {
        List<AbilityTree> leafs = GetLeafs(possiblePaths);
        leafs = leafs.Where(l => l.depth == maxDepth).ToList();
        if (leafs.Count() == 0)  // No paths: player will die
        {
            return new Ability[] { Ability.NoAbility };
        }
        AbilityTree leaf = leafs[UnityEngine.Random.Range(0, leafs.Count())];

        List<AbilityTree> path = new List<AbilityTree>();
        path.Add(leaf);
        AbilityTree? parent = leaf.parent;

        while (parent != null)
        {
            path.Add(parent);
            parent = parent.parent;
        }
        path.Reverse();
        return path.Select(at => at.ability).ToArray();
    }

    private List<AbilityTree> GetLeafs(AbilityTree tree)
    {
        if (tree.children.Count() == 0) { return new List<AbilityTree>() { tree }; }
        List<AbilityTree> leafs = new List<AbilityTree>();
        foreach (AbilityTree child in tree.children)
        {
            leafs = leafs.Union(GetLeafs(child)).ToList();
        }
        return leafs;
    }

    private AbilityTree CalculatePossiblePaths()
    {
        Dictionary<Ability, int> cooldowns = GetCooldowns();
        return CalculatePossiblePathsAfterAbility(Ability.NoAbility, cooldowns, 0, null);
    }

    private AbilityTree CalculatePossiblePathsAfterAbility(Ability ability, Dictionary<Ability, int> cooldowns, int depth, AbilityTree? parent)
    {
        AbilityTree possiblePaths = new AbilityTree(ability, depth, parent, cooldowns);
        if (depth >= maxDepth) { return possiblePaths; }
        Ability[] availableAbilities = GetAvailableAbilities(cooldowns);
        availableAbilities = FilterOnExistingObstacles(availableAbilities, depth);
        foreach (Ability availableAbility in availableAbilities)
        {
            Dictionary<Ability, int> newCooldowns = cooldowns.Select(kvp => new KeyValuePair<Ability, int>(kvp.Key, kvp.Value - 1)).ToDictionary(x => x.Key, x => x.Value);
            newCooldowns[availableAbility] = availableAbility == Ability.NoAbility? 0 : COOLDOWN_AFTER_USE;
            possiblePaths.children.Add(CalculatePossiblePathsAfterAbility(availableAbility, newCooldowns, depth+1, possiblePaths));
        };
        return possiblePaths;
    }

    private Ability[] FilterOnExistingObstacles(Ability[] availableAbilities, int depth)
    {
        Ability[][] existingObstacles = obstacleTracker.GetObstacleTypes();
        // existingObstacles[0] are currently being tackled by player so we want to find the obstacles from column 1 to end
        if (depth + 1 < existingObstacles.Count())
        {
            return existingObstacles[depth + 1].Intersect(availableAbilities).ToArray();
        }
        else  // No obstacles exist on the level we want to spawn, so all available actions are allowed
        {
            return availableAbilities;
        }
    }

    private Ability[] GetAvailableAbilities(Dictionary<Ability, int> cooldowns)
    {
        List<Ability> abilities = cooldowns.Where(kvp => kvp.Value <= 0).Select(kvp => kvp.Key).ToList();
        abilities.Add(Ability.NoAbility);  // Not doing anything has no cooldown
        return abilities.ToArray();
    }

    private Dictionary<Ability, int> GetCooldowns()
    {
        Dictionary<Ability, int> cooldowns = new Dictionary<Ability, int>();
        foreach (Ability ability in Enum.GetValues(typeof(Ability)))
        {
            if (ability != Ability.NoAbility)
            {
                cooldowns.Add(ability, (int)playerControl.abilityImages[ability].fillAmount * COOLDOWN_AFTER_USE);
            }
        }
        return cooldowns;
    }
}
