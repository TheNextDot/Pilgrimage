using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleMover : MonoBehaviour
{
    public bool canMove = true;
    float speed = 0.1f;

    void Update()
    {
        if (canMove == true)
            transform.position = new Vector3(transform.position.x - speed, transform.position.y, transform.position.z);
    }


}
