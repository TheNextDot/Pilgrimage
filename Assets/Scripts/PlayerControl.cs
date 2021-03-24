using System.Collections.Generic;
using UnityEngine;

public enum Ability
{
    Jump = 0,
    Duck = 1,
    Bash = 2
}
public class PlayerControl : MonoBehaviour
{
    readonly float X = -1.5f;
    readonly float BASE_Z = 6.5f;
    readonly float abilityTime = .7f;

    public UnityEngine.UI.Image jumpImage;
    public UnityEngine.UI.Image duckImage;
    public UnityEngine.UI.Image bashImage;

    int movement;




    enum Lane
    {
        UPPER = 0,
        MIDDLE = 1,
        LOWER = 2
    }
    Lane lane = Lane.MIDDLE;

    public HashSet<Ability> activeAbilities = new HashSet<Ability>();




    void Update()
    {
        Move();
        Act();
    }

    private void Move()
    {
        movement = Input.GetKeyDown(KeyCode.UpArrow) ? -1 : Input.GetKeyDown(KeyCode.DownArrow) ? 1 : 0;
        if (movement < 0 & lane != Lane.UPPER)
        {
            lane--;
        } else if(movement > 0 & lane != Lane.LOWER)
        {
            lane++;
        }
        transform.position = new Vector3(X, transform.position.y, BASE_Z - ((float)lane));
    }

    private void Act()
    {
        Ability? newAbility = Input.GetKeyDown(KeyCode.W) ? Ability.Jump : Input.GetKeyDown(KeyCode.S) ? Ability.Duck : Input.GetKeyDown(KeyCode.D) ? Ability.Bash : default(Ability?);
        if (newAbility == null)
        {
            return;
        }
        this.GetComponent<Animator>().SetBool("isJumping", false);
        this.GetComponent<Animator>().SetBool("isDucking", false);
        this.GetComponent<Animator>().SetBool("isBashing", false);

        if (newAbility == Ability.Jump)
        {
            if (jumpImage.fillAmount == 0)
            {
                this.GetComponent<Animator>().SetBool("isJumping", true);
                jumpImage.fillAmount = 1;
                activeAbilities.Add((Ability)newAbility);
                Invoke("stopJumping", abilityTime);

            }
        } else if (newAbility == Ability.Duck)
        {
            if (duckImage.fillAmount == 0)
            {
                this.GetComponent<Animator>().SetBool("isDucking", true);
                duckImage.fillAmount = 1;
                activeAbilities.Add((Ability)newAbility);
                Invoke("stopDucking", abilityTime);

            }
        } else if (newAbility == Ability.Bash)
        {
            if (bashImage.fillAmount == 0)
            {
                this.GetComponent<Animator>().SetBool("isBashing", true);
                bashImage.fillAmount = 1;
                activeAbilities.Add((Ability)newAbility);
                Invoke("stopBashing", abilityTime);
            }
        }
    }

    private void stopJumping()
    {
        activeAbilities.Remove(Ability.Jump);
        this.GetComponent<Animator>().SetBool("isJumping", false);
    }
    private void stopDucking() {
        activeAbilities.Remove(Ability.Duck);
        this.GetComponent<Animator>().SetBool("isDucking", false);

    }
    private void stopBashing()
    {
        activeAbilities.Remove(Ability.Bash);
        this.GetComponent<Animator>().SetBool("isBashing", false);
    }
}
