using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum Ability
{
    Jump = 1,
    Duck = 2,
    Bash = 3,
    NoAbility = 0
}
public class PlayerControl : MonoBehaviour
{
    readonly float X = -1.5f;
    readonly float BASE_Z = 4.5f;

    public UnityEngine.UI.Image jumpAbilityImage;
    public UnityEngine.UI.Image duckAbilityImage;
    public UnityEngine.UI.Image bashAbilityImage;
    public Dictionary<Ability, UnityEngine.UI.Image> abilityImages;
    [SerializeField] GameObject deathFX;
    Dictionary<Ability, string> animationBoolMap = new Dictionary<Ability, string>() { { Ability.Bash, "isBashing" }, { Ability.Jump, "isJumping" }, { Ability.Duck, "isDucking" } };

    [SerializeField] AudioClip bash;
    [SerializeField] AudioClip jump;
    [SerializeField] AudioClip duck;
    [SerializeField] AudioClip deathSFX;

    Dictionary<Ability, AudioClip> abilityFX;

    int movement;

    public enum Lane  // TODO: move somewhere common
    {
        LOWER = 0,
        MIDDLE = 1,
        UPPER = 2
    }
    public Lane lane = Lane.MIDDLE;

    public Ability? activeAbility = null;
    void Awake()
    {
        abilityImages = new Dictionary<Ability, UnityEngine.UI.Image>();
        abilityImages.Add(Ability.Jump, jumpAbilityImage);
        abilityImages.Add(Ability.Duck, duckAbilityImage);
        abilityImages.Add(Ability.Bash, bashAbilityImage);

        abilityFX = new Dictionary<Ability, AudioClip>();
        abilityFX.Add(Ability.Jump, jump);
        abilityFX.Add(Ability.Bash, bash);
        abilityFX.Add(Ability.Duck, duck);

    }

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
            lane++;
        } else if(movement > 0 & lane != Lane.LOWER)
        {
            lane--;
        }
        transform.position = new Vector3(X, transform.position.y, BASE_Z + ((float)lane));
    }


    internal void Animate(bool passed, bool isObstacle)
    {
        if (!passed)
        {
            AudioSource.PlayClipAtPoint(deathSFX, transform.position);
            deathFX = Instantiate(deathFX, transform.position, transform.rotation);
            deathFX.GetComponent<ParticleSystem>().Play();
            Destroy(gameObject);
        }
        else if (isObstacle)
        {
            if (activeAbility != null)
            {
                if (activeAbility == Ability.Duck)
                {
                    GetComponentInChildren<ParticleSystem>().Play();
                }
                GetComponent<AudioSource>().PlayOneShot(abilityFX[(Ability)activeAbility]);
                this.GetComponent<Animator>().SetBool(animationBoolMap[(Ability)activeAbility], true);
                StartCoroutine(DelayedStopAnimation());
                activeAbility = null;
            }
        }
    }

    internal void TriggerAbility()
    {
        if (activeAbility != null)
        {
            abilityImages[(Ability)activeAbility].fillAmount = 1;
        }
    }

    private void Act()
    {
        Ability? newAbility = Input.GetKeyDown(KeyCode.W) ? Ability.Jump : Input.GetKeyDown(KeyCode.S) ? Ability.Duck : Input.GetKeyDown(KeyCode.D) ? Ability.Bash : default(Ability?);
        if (newAbility == null)
        {
            return;
        }
        if (abilityImages[(Ability)newAbility].fillAmount == 0)
        {
            activeAbility = newAbility;
        }
    }

    IEnumerator DelayedStopAnimation()
    {
        yield return new WaitForSeconds(0.5f);
        this.GetComponent<Animator>().SetBool("isBashing", false);
        this.GetComponent<Animator>().SetBool("isDucking", false);
        this.GetComponent<Animator>().SetBool("isJumping", false);
        yield return new WaitForSeconds(0.3f);
        transform.position = new Vector3(transform.position.x, 0.57f, transform.position.z);  // Jumping changes y, reset.
    }
}
