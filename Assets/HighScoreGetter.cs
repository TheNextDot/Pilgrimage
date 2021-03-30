using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighScoreGetter : MonoBehaviour
{
    public Text highScoreText;
    private int highScore;
    string highScoreKey = "HighScore";

    // Start is called before the first frame update
    void Start()
    {
        highScore = PlayerPrefs.GetInt(highScoreKey, 0);
        highScoreText.text = "High Score: " + highScore;
    }

}
