using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    readonly List<Tuple<Vector3, Quaternion>> transforms = new List<Tuple<Vector3, Quaternion>>()
    {
        { new Tuple<Vector3, Quaternion>(new Vector3(0, 2, -1), Quaternion.Euler(15, 0, 0)) },
        { new Tuple<Vector3, Quaternion>(new Vector3(2.5f, 2.3f, -2.1f), Quaternion.Euler(15, 0, 0)) },
        { new Tuple<Vector3, Quaternion>(new Vector3(5, 2.6f, -3.3f), Quaternion.Euler(15, 0, 0)) },
        { new Tuple<Vector3, Quaternion>(new Vector3(8f, 3, -4.5f), Quaternion.Euler(15, 0, 0)) },
    };
    public float moveDelay = 1.0f;
    public float moveDuration = 3.0f;
    public int position = 0;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Move(position));
        StartCoroutine(MoveToNextPosition());
    }

    private IEnumerator MoveToNextPosition()
    {
        while (position < transforms.Count-1)
        {
            yield return new WaitForSeconds(moveDelay);
            StartCoroutine(Move(this.position+1));
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
        for (float t = 0f; t < moveDuration; t += Time.deltaTime)
        {
            gameObject.transform.position = Vector3.Lerp(oldPosition, transforms[position].Item1, t/moveDuration);
            gameObject.transform.rotation = Quaternion.Slerp(oldRotation, transforms[position].Item2, t/moveDuration);
            yield return 0;
        }
        gameObject.transform.position = transforms[position].Item1;
        gameObject.transform.rotation = transforms[position].Item2;
        gameObject.GetComponent<CameraShake>().OnEnable();
    }

}
