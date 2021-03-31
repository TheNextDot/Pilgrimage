using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] AudioClip startGame;

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
            AudioSource.PlayClipAtPoint(startGame, transform.position);
            LoadGame();
        }
    }

    public void QuitGame()
    {
        QuitGame();
    }

}
