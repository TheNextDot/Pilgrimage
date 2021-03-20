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
            if (playerControl.ability != obstacleType)
            {
                Destroy(other.gameObject);
                SceneManager.LoadScene(0);
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
}
