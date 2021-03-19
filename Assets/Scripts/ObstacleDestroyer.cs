using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleDestroyer : MonoBehaviour
{
    ScoreCounter scoreCounter;
    void Start()
    {
        scoreCounter = FindObjectOfType<ScoreCounter>();
    }


    private void OnTriggerEnter(Collider other)
    {
        scoreCounter.AddScore();
        Destroy(other.gameObject);
    }
}
