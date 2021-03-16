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
            Instantiate(obstaclePrefab[Random.Range(0, 2)], transform);
            yield return new WaitForSeconds(spawnInterval[(Random.Range(1,3))]);
        }
    }

}
