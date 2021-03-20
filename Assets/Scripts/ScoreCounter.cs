using UnityEngine;
using UnityEngine.UI;

public class ScoreCounter : MonoBehaviour
{
    private int currentScore;
    public Text scoreText;

    // Use this for initialization
    void Start()
    {
        currentScore = 0;
    }

    void Update()
    {
        scoreText.text = "Score: " + currentScore;
    }

    public void AddScore()
    {
        currentScore++;
    }


}
