using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    readonly List<Tuple<Vector3, Quaternion>> transforms = new List<Tuple<Vector3, Quaternion>>()
    {
        { new Tuple<Vector3, Quaternion>(new Vector3(1, 3.7f, -0.6f), Quaternion.Euler(15, 0, 0)) },
        { new Tuple<Vector3, Quaternion>(new Vector3(2.75f, 4f, -2.1f), Quaternion.Euler(15, 0, 0)) },
        { new Tuple<Vector3, Quaternion>(new Vector3(4.5f, 4.3f, -3.3f), Quaternion.Euler(15, 0, 0)) },
        { new Tuple<Vector3, Quaternion>(new Vector3(6.25f, 4.6f, -4.5f), Quaternion.Euler(15, 0, 0)) },
    };
    readonly List<float> abilitiesPosition = new List<float>{ 960, 840, 720, 600};
    readonly List<float> spawnerPosition = new List<float> { 9, 12, 15, 18};
    public int moveDelay = 10;
    private int currentDelay;
    public float moveDuration = 3.0f;
    public int position = 0;
    [SerializeField] GameObject abilitiesCanvas;
    [SerializeField] ObstacleSpawner obstacleSpawner;

    // Start is called before the first frame update
    void Start()
    {
        currentDelay = moveDelay;
    }

    public void Tick()
    {
        currentDelay--;
        if (currentDelay == 0 & position<transforms.Count-1)
        {
            StartCoroutine(Move(this.position + 1));
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator Move(int position)
    {
        this.position = position;
        Vector3 oldPosition = gameObject.transform.position;
        Quaternion oldRotation = gameObject.transform.rotation;
        Vector3 oldSpawnerPosition = obstacleSpawner.transform.position;
        for (float t = 0f; t < moveDuration; t += Time.deltaTime)
        {
            gameObject.transform.position = Vector3.Lerp(oldPosition, transforms[position].Item1, t/moveDuration);
            gameObject.transform.rotation = Quaternion.Slerp(oldRotation, transforms[position].Item2, t/moveDuration);
            yield return 0;
        }
        gameObject.transform.position = transforms[position].Item1;
        gameObject.transform.rotation = transforms[position].Item2;
        gameObject.GetComponent<CameraShake>().OnEnable();

        obstacleSpawner.transform.position = new Vector3(spawnerPosition[position], oldSpawnerPosition.y, oldSpawnerPosition.z);
        obstacleSpawner.Deepen();
    }

}
