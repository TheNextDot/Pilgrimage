using UnityEngine;

public class PillarMover : MonoBehaviour
{
    float speed = 5f;

    void Update()
    {
        transform.position = new Vector3(transform.position.x - (speed * Time.deltaTime), transform.position.y, transform.position.z);
    }


}