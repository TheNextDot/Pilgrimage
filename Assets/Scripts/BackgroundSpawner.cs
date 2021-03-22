using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundSpawner : MonoBehaviour
{
    [SerializeField] GameObject objectPrefab;


    void Start()
    {
        StartCoroutine(SpawnObject());
    }

    IEnumerator SpawnObject()
    {
        while (true)
        {
            Instantiate(objectPrefab, transform);
            yield return new WaitForSeconds(1.2f);
        }
    }

}