#nullable enable
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    [SerializeField] PlayerDestroyer[] obstaclePrefab;
    public bool Spawn = true;
    public float spawnDelay = 1.0f;
    public int maxDepth = 4;
    [SerializeField] PlayerControl playerControl;
    public CameraShake cam;  // Passed to obstacles

    private readonly float COOLDOWN_AFTER_USE = 4.0f;  // TODO: define globally so you don't need to update this and the cooldown images
    private readonly float LANE_WIDTH = 1.0f;

    private Ability[][] existingObstacles;

    private class AbilityTree
    {
        public Ability ability;
        public int depth;
        public List<AbilityTree> children = new List<AbilityTree>();
        public AbilityTree? parent;

        public AbilityTree(Ability ability = Ability.NoAbility, int depth = 0, AbilityTree? parent=null)
        {
            this.ability = ability;
            this.depth = depth;
            this.parent = parent;
        }
    }

     void Start()
    {
        InitializeObstacleArray();
        StartCoroutine(SpawnObstacles());
    }

    private void InitializeObstacleArray()
    {
        existingObstacles = new Ability[maxDepth][];
        for (int i=0; i<maxDepth; i++)
        {
            existingObstacles[i] = new Ability[3];
        }
    }

    IEnumerator SpawnObstacles()
    {
        while (true)
        {
            AbilityTree possiblePaths = CalculatePossiblePaths();
            Ability[] pathToGenerate = PathToRandomLeaf(possiblePaths);
            Ability obstacleToGenerate = pathToGenerate.Last();
            Ability[] obstaclesToGenerate = GenerateObstacles(obstacleToGenerate);
            AddObstacles(obstaclesToGenerate);
            SpawnObstacles(obstaclesToGenerate);
            Debug.Log("Path: " + string.Join(", ", pathToGenerate.ToString()));
            yield return new WaitForSeconds(1.0f);
        }
    }

    private void SpawnObstacles(Ability[] obstaclesToGenerate)
    {
        int lane = -1;  // The ObstacleSpawner is located in the center, so we spawn at relative lanes -1, 0 and 1
        foreach (Ability obstacleType in obstaclesToGenerate)
        {
            if (obstacleType != Ability.NoAbility)
            {
                PlayerDestroyer prefab = obstaclePrefab[(int)obstacleType - 1];
                Vector3 position = new Vector3(
                    transform.position.x + prefab.transform.position.x, 
                    transform.position.y + prefab.transform.position.y, 
                    transform.position.z + prefab.transform.position.z + lane * LANE_WIDTH);
                PlayerDestroyer newObject = Instantiate(prefab, position, prefab.transform.rotation);  // Obstacles start at 1 (0 is for NoAbility)
                newObject.cam = cam;
            }
            lane++;
        }
    }

    private void AddObstacles(Ability[] obstaclesToGenerate)
    {
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
        if (parent == null)  // No paths: player will die
        {
            return new Ability[] { Ability.NoAbility };
        }

        while (parent.parent != null)
        {
            path.Add(parent);
            parent = parent.parent;
        }
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
        Dictionary<Ability, float> cooldowns = GetCooldowns();
        return CalculatePossiblePathsAfterAbility(Ability.NoAbility, cooldowns, 0, null);
    }

    private AbilityTree CalculatePossiblePathsAfterAbility(Ability ability, Dictionary<Ability, float> cooldowns, int depth, AbilityTree? parent)
    {
        AbilityTree possiblePaths = new AbilityTree(ability, depth, parent);
        if (depth >= maxDepth) { return possiblePaths; }
        Ability[] availableAbilities = GetAvailableAbilities(cooldowns);
        availableAbilities = FilterOnExistingObstacles(availableAbilities, depth);
        foreach (Ability availableAbility in availableAbilities)
        {
            Dictionary<Ability, float> newCooldowns = cooldowns.Select(kvp => new KeyValuePair<Ability, float>(kvp.Key, kvp.Value - 1.0f)).ToDictionary(x => x.Key, x => x.Value); ;
            newCooldowns[availableAbility] = availableAbility == Ability.NoAbility? 0 : COOLDOWN_AFTER_USE;
            possiblePaths.children.Add(CalculatePossiblePathsAfterAbility(availableAbility, cooldowns, depth+1, possiblePaths));
        };
        return possiblePaths;
    }

    private Ability[] FilterOnExistingObstacles(Ability[] availableAbilities, int depth)
    {
        return this.existingObstacles[depth].Intersect(availableAbilities).ToArray();
    }

    private Ability[] GetAvailableAbilities(Dictionary<Ability, float> cooldowns)
    {
        List<Ability> abilities = cooldowns.Where(kvp => kvp.Value <= 0).Select(kvp => kvp.Key).ToList<Ability>();
        abilities.Add(Ability.NoAbility);  // No obstacle is always possible
        return abilities.ToArray();
    }

    private Dictionary<Ability, float> GetCooldowns()
    {
        Dictionary<Ability, float> cooldowns = new Dictionary<Ability, float>();
        foreach (Ability ability in Enum.GetValues(typeof(Ability)))
        {
            if (ability != Ability.NoAbility)
            {
                cooldowns.Add(ability, playerControl.abilityImages[ability].fillAmount);
            }
        }
        return cooldowns;
    }
}
