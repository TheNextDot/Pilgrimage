using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerDestroyer : MonoBehaviour
{

    public Ability obstacleType;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            PlayerControl playerControl = other.gameObject.GetComponent<PlayerControl>();
            if (!playerControl.activeAbilities.Contains(obstacleType))
            {
                StartDeathSequence();
            }
            else
            {
                if (this.GetComponent<ParticleSystem>())
                {
                    this.GetComponent<ParticleSystem>().Play();
                    this.GetComponent<MeshRenderer>().enabled = false;
                }
  
            }
        }
    }

    private void StartDeathSequence()
    {
        //Stop all moving objects
        ObstacleMover[] obstacleMovers = FindObjectsOfType<ObstacleMover>();
        foreach (ObstacleMover obstacleMover in obstacleMovers)
            obstacleMover.canMove = false;

        //Stop obstacle instantiators
        ObstacleSpawner[] obstacleSpawners = FindObjectsOfType<ObstacleSpawner>();
        foreach (ObstacleSpawner obstacleSpawner in obstacleSpawners)
            obstacleSpawner.Spawn = false;

        //Stop Background instantiators
        BackgroundSpawner[] backgroundSpawners = FindObjectsOfType<BackgroundSpawner>();
        foreach (BackgroundSpawner backgroundSpawner in backgroundSpawners)
            backgroundSpawner.Spawn = false;

        //Determine object that killed player and process matching Death Animation -> todo
        switch (gameObject.name)
        {
            case "obstacle A":
                //duck animation with blood
            case "obstacle B:":
                //fall over 
            case "obstacle C":
                //blood explosion
                break;
        }


        FindObjectOfType<Animator>().SetBool("isRunning", true);
        

        //Reload game after 5 seconds
        Invoke("LoadScene", 5f);
    }

    private void LoadScene()
    {
        SceneManager.LoadScene(0);
    }
}
