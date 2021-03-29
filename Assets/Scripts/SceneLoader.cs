using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void LoadGame()
    {
        Invoke("LoadScene", 1f);
    }

    public void LoadScene()
    {
        SceneManager.LoadScene(1);
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return)) 
        {
            GetComponent<AudioSource>().Play();
            LoadGame();

        }
    }

    public void QuitGame()
    {
        QuitGame();
    }

}
