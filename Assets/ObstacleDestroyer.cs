using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleDestroyer : MonoBehaviour
{


    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("collision detected");
        Destroy(other.gameObject);
    }
}
