#nullable enable
using System.Linq;
using UnityEngine;

public class ObstacleTracker : MonoBehaviour
{
    public int nObstacleCols = 3;
    public int nObstacleRows = 3;
    public Obstacle?[][] obstacles { get; private set; }
    private Obstacle?[] passedObstacles;

    private void Awake()
    {
        obstacles = new Obstacle?[nObstacleCols][];
        foreach(int i in Enumerable.Range(0, nObstacleCols))
        {
            obstacles[i] = new Obstacle?[nObstacleRows];  // Defaults to null
        }
        passedObstacles = new Obstacle?[nObstacleRows];
    }

    public Ability[][] GetObstacleTypes()
    {
        return obstacles.Select(l => l.Select(o => o==null?Ability.NoAbility:o.obstacleType).ToArray()).ToArray();
    }

    public void Deepen()
    {
        nObstacleCols++;
        Obstacle?[][] oldObstacles = obstacles;
        obstacles = new Obstacle?[nObstacleCols][];
        foreach (int i in Enumerable.Range(0, nObstacleCols - 1))
        {
            obstacles[i] = oldObstacles[i];
        }
        obstacles[nObstacleCols - 1] = new Obstacle?[nObstacleRows];
    }

    public void SpawnObstacles(Obstacle?[] newColumn)
    {
        DestroyPassedObstacles();
        passedObstacles = obstacles[0];
        foreach (int i in Enumerable.Range(0, nObstacleCols-1))
        {
            obstacles[i] = obstacles[i + 1];
        }
        obstacles[nObstacleCols-1] = newColumn;
    }

    private void DestroyPassedObstacles()
    {
        foreach(Obstacle? obstacle in passedObstacles)
        {
            if (obstacle != null)
            {
                Destroy(obstacle.gameObject);
            }
        }
    }

    internal void MoveObstacles()
    {
        int column = 0;
        foreach (Obstacle?[] obstacleColumn in obstacles)
        {
            foreach (Obstacle? obstacle in obstacleColumn)
            {
                if (obstacle != null)
                    obstacle.MoveToPosition(column);
            }
            column++;
        }
        foreach (Obstacle? obstacle in passedObstacles)
        {
            if (obstacle != null)
                obstacle.MoveToPosition(-1);
        }
    }

    internal bool Process(PlayerControl player)
    {
        PlayerControl.Lane lane = player.lane;
        Obstacle obstacle = obstacles[0][(int)lane];
        Ability obstacleType = obstacle==null?Ability.NoAbility:obstacle.obstacleType;
        bool passed = obstacleType == Ability.NoAbility | player.activeAbility == obstacleType;
        if (passed & obstacle != null)
        {
            obstacle.Animate();
        }
        return passed;
    }
}
