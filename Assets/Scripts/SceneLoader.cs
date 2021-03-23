using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void LoadGame()
    {
        SceneManager.LoadScene(1);
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return)) { LoadGame(); }
    }

    public void QuitGame()
    {
        QuitGame();
    }

}
