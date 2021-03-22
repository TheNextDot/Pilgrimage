using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundSpawner : MonoBehaviour
{
    [SerializeField] GameObject objectPrefab;
    public bool Spawn = true; 


    void Start()
    {
        StartCoroutine(SpawnObject());
    }

    IEnumerator SpawnObject()
    {
        while (Spawn == true)
        {
            Instantiate(objectPrefab, transform);
            yield return new WaitForSeconds(1.2f);
        }
    }

}