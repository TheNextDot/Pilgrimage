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
    readonly float Y = 0.6f;
    readonly float BASE_Z = 6.5f;
    readonly float abilityDuration = 1.0f;

    int movement;

    enum Lane
    {
        UPPER = 0,
        MIDDLE = 1,
        LOWER = 2
    }
    Lane lane = Lane.MIDDLE;

    public Ability? ability = null;

    void Update()
    {
        movement = Input.GetKeyDown(KeyCode.UpArrow) ? -1 : Input.GetKeyDown(KeyCode.DownArrow) ? 1 : 0;
        Move();
        Ability? newAbility = Input.GetKeyDown(KeyCode.W) ? Ability.Jump : Input.GetKeyDown(KeyCode.S) ? Ability.Duck : Input.GetKeyDown(KeyCode.D) ? Ability.Bash : default(Ability?);
        if (newAbility != null) { ability = newAbility; }
        Act();
    }

    private void Move()
    {
        if(movement < 0 & lane != Lane.UPPER)
        {
            lane--;
        } else if(movement > 0 & lane != Lane.LOWER)
        {
            lane++;
        }
        transform.position = new Vector3(X, Y, BASE_Z - ((float)lane));
    }

    private void Act()
    {
        if(ability == Ability.Jump)
        {
            this.GetComponent<Animator>().SetBool("isJumping", true);
        } else if (ability == Ability.Duck)
        {
            this.GetComponent<Animator>().SetBool("isDucking", true);
        } else if (ability == Ability.Bash)
        {
            this.GetComponent<Animator>().SetBool("isBashing", true);
        }

        if(ability != null)
        {
            Invoke("ResetAbility", abilityDuration);
        }
    }

    private void ResetAbility()
    {
        ability = null;
        this.GetComponent<Animator>().SetBool("isJumping", false);
        this.GetComponent<Animator>().SetBool("isDucking", false);
        this.GetComponent<Animator>().SetBool("isBashing", false);
    }
}
