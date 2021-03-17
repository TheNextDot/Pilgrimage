using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreAdder : MonoBehaviour
{
    [SerializeField] 
    float playerXPos;

    private void Start()
    {
        playerXPos =  GameObject.FindGameObjectWithTag("Player").transform.position.x;
    }

    void Update()
    {
        if (transform.position.x < playerXPos)
        {
            GetComponent<ScoreCounter>().AddScore();
        }
    }
}
