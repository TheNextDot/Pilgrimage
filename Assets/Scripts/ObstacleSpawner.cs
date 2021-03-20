using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    [SerializeField] GameObject[] obstaclePrefab;
    [SerializeField] float[] spawnInterval;


     void Start()
    {
        StartCoroutine(SpawnEnemies());
    }

    IEnumerator SpawnEnemies()
    {
        while (true)
        {
            int obstacle = Random.Range(0, 4);
            if (obstacle != 3)
                Instantiate(obstaclePrefab[obstacle], transform);
            yield return new WaitForSeconds(1);
        }
    }

}
