using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    [SerializeField] GameObject obstaclePrefab;

    void Start()
    {
        StartCoroutine(SpawnEnemies());   
    }

    IEnumerator SpawnEnemies()
    {
        while (true)
        {
            Instantiate(obstaclePrefab, transform);

            yield return new WaitForSeconds(3f);

        }


    }

}
