using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIntroducer : MonoBehaviour
{
    [SerializeField] GameObject playerCharacter;
    [SerializeField] float speed = 1f;
    Vector3 currentPos;
    Vector3 upYPos;
    Vector3 downYPos;
    bool moveLift = true;
    bool liftDown = false;


    // Start is called before the first frame update
    void Start()
    {
        playerCharacter.SetActive(false);
        downYPos = new Vector3(transform.position.x, currentPos.y - 3, transform.position.z);
        upYPos = new Vector3(transform.position.x, currentPos.y+3, transform.position.z);
        
    }

    // Update is called once per frame
    void Update()
    {
        if (moveLift == true)
        {
            if (liftDown == false)
            {
                LiftDown();
            }

            if (liftDown == true)
            {
                LiftUp();
            }
        }

        
         
    }

    private void LiftDown()
    {
        currentPos = transform.position;
        float step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, downYPos, step);
        if (transform.position.y <= downYPos.y)
        {
            liftDown = true;
        }
    }

    private void LiftUp()
    {
        playerCharacter.SetActive(true);
        currentPos = transform.position;
        float step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, upYPos, step);
        if (transform.position.y >= -0.76)
        {
            moveLift = false;
        }
    }
}
