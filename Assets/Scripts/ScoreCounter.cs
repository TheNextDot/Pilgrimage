using UnityEngine;
using UnityEngine.UI;

public class ScoreCounter : MonoBehaviour
{
    private int currentScore;
    private int highScore;
    string highScoreKey = "HighScore";
    public Text scoreText;

    // Use this for initialization
    void Start()
    {
        currentScore = 0;
        highScore = PlayerPrefs.GetInt(highScoreKey, 0);
    }

    void Update()
    {
        scoreText.text = "Score: " + currentScore;
    }

    public void AddScore()
    {
        currentScore++;
        if (currentScore > highScore)
        {
            PlayerPrefs.SetInt(highScoreKey, currentScore);
            PlayerPrefs.Save();
        }
    }


}
