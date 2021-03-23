using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleMover : MonoBehaviour
{
    public bool canMove = true;
    float speed = 5f;

    void Update()
    {
        if (canMove == true)
            transform.position = new Vector3(transform.position.x - (speed * Time.deltaTime), transform.position.y, transform.position.z);
    }


}
