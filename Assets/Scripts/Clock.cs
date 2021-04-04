using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Clock : MonoBehaviour
{
    [SerializeField] ObstacleTracker obstacleTracker;
    [SerializeField] ObstacleSpawner obstacleSpawner;
    [SerializeField] PlayerControl playerControl;
    [SerializeField] ScoreCounter scoreCounter;
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
            if (!passed)
            {
                Invoke("LoadScene", 0.9f);
            } else {
                LowerCooldowns();
                SpawnObstacles();
                MoveObstacles();
                AddScore();
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
            bool wasNotFull = cooldown.fillAmount < 1;
            cooldown.fillAmount = Math.Min(1, cooldown.fillAmount + 0.25f);
            if (wasNotFull & cooldown.fillAmount == 1)
            {
                cooldown.GetComponentInChildren<ParticleSystem>().Play();
            }
        }
    }

    private bool ProcessCollision()
    {
        return obstacleTracker.Process(playerControl);
    }

    private void LoadScene()
    {
        SceneManager.LoadScene(0);
    }

    private void AddScore()
    {
        scoreCounter.AddScore();
    }
}
