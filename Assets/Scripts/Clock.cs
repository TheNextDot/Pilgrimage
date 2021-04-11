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
    [SerializeField] CameraMovement cameraMovement;
    [SerializeField] Tutorial tutorial;
    [SerializeField] bool startTutorial = true;

    void Start()
    {
        if (PlayerPrefs.GetInt("startTutorial", 1)==1)
        {
            StartCoroutine(TutorialTick());
            PlayerPrefs.SetInt("startTutorial", 0);
        }
        else
        {
            StartCoroutine(Tick());
        }
    }

    private IEnumerator TutorialTick()
    {
        yield return new WaitForSeconds(1.0f);
        obstacleSpawner.SpawnObstacles(Ability.Bash);
        obstacleTracker.MoveObstacles();
        yield return new WaitForSeconds(1.0f);
        obstacleSpawner.SpawnObstacles(Ability.Jump);
        obstacleTracker.MoveObstacles();
        yield return new WaitForSeconds(1.0f);
        obstacleSpawner.SpawnObstacles(Ability.Duck);
        obstacleTracker.MoveObstacles();
        yield return new WaitForSeconds(1.0f);
        Freeze();
        yield return tutorial.showTutorial(0);
        // TODO: Trigger + deactivate ability
        playerControl.activeAbility = Ability.Bash;
        ProcessCollision();
        Unfreeze();
        obstacleSpawner.SpawnObstacles();
        obstacleTracker.MoveObstacles();
        yield return new WaitForSeconds(1.0f);
        LowerCooldowns();
        Freeze();
        yield return tutorial.showTutorial(1);
        playerControl.activeAbility = Ability.Jump;
        ProcessCollision();
        Unfreeze();
        obstacleSpawner.SpawnObstacles();
        obstacleTracker.MoveObstacles();
        yield return new WaitForSeconds(1.0f);
        LowerCooldowns();
        Freeze();
        yield return tutorial.showTutorial(2);
        playerControl.activeAbility = Ability.Duck;
        ProcessCollision();
        obstacleSpawner.SpawnObstacles();
        Unfreeze();
        obstacleTracker.MoveObstacles();
        StartCoroutine(Tick());
    }

    private void Freeze()
    {
        Time.timeScale = 0f;
    }

    private void Unfreeze()
    {
        Time.timeScale = 1f;
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
            }
            else
            {
                LowerCooldowns();
                obstacleSpawner.SpawnObstacles();
                obstacleTracker.MoveObstacles();
                scoreCounter.AddScore();
                cameraMovement.Tick();
            }
        }
    }

    private void LowerCooldowns()
    {
        foreach (UnityEngine.UI.Image cooldown in cooldowns)
        {
            bool wasNotFull = cooldown.fillAmount > 0;
            cooldown.fillAmount = Math.Max(0, cooldown.fillAmount - 0.25f);
            if (wasNotFull & cooldown.fillAmount == 0)
            {
                cooldown.transform.parent.gameObject.GetComponentInChildren<ParticleSystem>().Play();
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
}
