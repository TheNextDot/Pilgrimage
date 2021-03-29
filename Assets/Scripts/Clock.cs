using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Clock : MonoBehaviour
{
    [SerializeField] ObstacleTracker obstacleTracker;
    [SerializeField] ObstacleSpawner obstacleSpawner;
    [SerializeField] PlayerControl playerControl;
    [SerializeField] UnityEngine.UI.Image[] cooldowns;

    void Start()
    {
        StartCoroutine(Tick());
    }

    private IEnumerator Tick()
    {
        while (true)
        {
            yield return new WaitForSeconds(1.0f);
            bool passed = ProcessCollision();
            AnimatePlayer(passed);
            if (!passed)
            {
                Invoke("LoadScene", 0.9f);
            } else {
                LowerCooldowns();
                SpawnObstacles();
                MoveObstacles();
                // TODO: AddScore();
            }
        }
    }

    private void MoveObstacles()
    {
        obstacleTracker.MoveObstacles();
    }

    private void SpawnObstacles()
    {
        obstacleSpawner.SpawnObstacles();
    }

    private void LowerCooldowns()
    {
        foreach(UnityEngine.UI.Image cooldown in cooldowns)
        {
            cooldown.fillAmount = Math.Max(0, cooldown.fillAmount - 0.25f);
        }
    }

    private void AnimatePlayer(bool passed)
    {
        playerControl.Animate(passed);
    }

    private bool ProcessCollision()
    {
        return obstacleTracker.Process(playerControl);
    }

    private void LoadScene()
    {
        SceneManager.LoadScene(0);
    }
}
