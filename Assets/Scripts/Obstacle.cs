using System.Collections;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public Ability obstacleType;
    public CameraShake cam;

    public int column;
    float positionBase = -1.0f;  // Position when player interacts with obstacle
    public float positionDelta = 3.8f;

    public void MoveToPosition(int column)
    {
        this.column = column;
        StartCoroutine(Move());
    }

    IEnumerator Move()
    {
        float moveDuration = 1.0f;
        for (float t = 0f; t < moveDuration; t += Time.deltaTime)
        {
            gameObject.transform.position = Vector3.Lerp(
                new Vector3(positionBase + (column + 1)* positionDelta, transform.position.y, transform.position.z),
                new Vector3(positionBase + column * positionDelta, transform.position.y, transform.position.z),
                t / moveDuration);
            yield return 0;
        }
    }

    internal void Animate()
    {
        if (this.GetComponent<ParticleSystem>())
        {
            this.GetComponent<ParticleSystem>().Play();
            this.GetComponent<MeshRenderer>().enabled = false;
            cam.shakeDuration = 0.5f;
        }
    }
}
