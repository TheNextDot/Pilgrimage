using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleMover : MonoBehaviour
{
    readonly float speed = 0.1f;
    // Start is called before the first frame update
    void Start()
    {

    }

    void FixedUpdate()
    {
        transform.position = new Vector3(transform.position.x - speed, transform.position.y, transform.position.z);
    }
}
