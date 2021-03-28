using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerDestroyer : MonoBehaviour
{

    public Ability obstacleType;
    public CameraShake cam;

    [SerializeField] GameObject deathFX;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            PlayerControl playerControl = other.gameObject.GetComponent<PlayerControl>();
            if (!playerControl.activeAbilities.Contains(obstacleType))
            {
                Invoke("LoadScene", 1f);
                deathFX = Instantiate(deathFX, gameObject.transform) as GameObject;
                deathFX.GetComponent<ParticleSystem>().Play();
                Destroy(other.gameObject);
            }
            else
            {
                if (this.GetComponent<ParticleSystem>())
                {
                    this.GetComponent<ParticleSystem>().Play();
                    this.GetComponent<MeshRenderer>().enabled = false;
                    cam.shakeDuration = 0.5f;
                }

            }
        }
    }


    private void LoadScene()
    {
        SceneManager.LoadScene(0);
    }
}
