using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialButton : MonoBehaviour
{
    private void Start()
    {
        bool active = PlayerPrefs.GetInt("startTutorial", 1) == 1;
        if (active) { GetComponentInChildren<Text>().text = "with\ntutorial"; }
        else { GetComponentInChildren<Text>().text = "without\ntutorial"; }
    }

    public void OnClick()
    {
        bool active = PlayerPrefs.GetInt("startTutorial", 0) == 1;
        if (active)
        {
            PlayerPrefs.SetInt("startTutorial", 0);
            GetComponentInChildren<Text>().text = "without\ntutorial";
        } else
        {
            PlayerPrefs.SetInt("startTutorial", 1);
            GetComponentInChildren<Text>().text = "with\ntutorial";
        }
    }
}
