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
        Destroy(other.gameObject);
        if (scoreCounter != null && other.gameObject.tag != "Pillar") { scoreCounter.AddScore(); }
    }
}
