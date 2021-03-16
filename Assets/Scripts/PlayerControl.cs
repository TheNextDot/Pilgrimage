using System;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    readonly float X = -1.5f;
    readonly float Y = 0.75f;
    readonly float BASE_Z = 6.5f;

    public Rigidbody rb;
    
    int movement;

    enum Lane
    {
        UPPER = 0,
        MIDDLE = 1,
        LOWER = 2
    }
    Lane lane = Lane.MIDDLE;



    // Update is called once per frame
    void Update()
    {
        movement = Input.GetKeyDown(KeyCode.W) ? -1 : Input.GetKeyDown(KeyCode.S) ? 1 : 0;
        Move();
    }

    // Used to execute the commands given in the Update function
    void FixedUpdate()
    {

    }

    private void Move()
    {
        if(movement < 0 & lane != Lane.UPPER)
        {
            lane--;
        } else if(movement > 0 & lane != Lane.LOWER)
        {
            lane++;
        }
        rb.MovePosition(new Vector3(X, Y, BASE_Z - ((float)lane)));
    }

}
