using System.Collections.Generic;
using UnityEngine;

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
    readonly float BASE_Z = 6.5f;
    readonly float abilityTime = 0.8f;

    public UnityEngine.UI.Image jumpAbilityImage;
    public UnityEngine.UI.Image duckAbilityImage;
    public UnityEngine.UI.Image bashAbilityImage;
    public Dictionary<Ability, UnityEngine.UI.Image> abilityImages;

    [SerializeField] AudioClip bash;
    [SerializeField] AudioClip jump;
    [SerializeField] AudioClip duck;
    int movement;

    enum Lane
    {
        UPPER = 0,
        MIDDLE = 1,
        LOWER = 2
    }
    Lane lane = Lane.MIDDLE;

    public HashSet<Ability> activeAbilities = new HashSet<Ability>();
    void Awake()
    {
        abilityImages = new Dictionary<Ability, UnityEngine.UI.Image>();
        abilityImages.Add(Ability.Jump, jumpAbilityImage);
        abilityImages.Add(Ability.Duck, duckAbilityImage);
        abilityImages.Add(Ability.Bash, bashAbilityImage);
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
            if (abilityImages[Ability.Jump].fillAmount == 0)
            {
                this.GetComponent<Animator>().SetBool("isJumping", true);
                abilityImages[Ability.Jump].fillAmount = 1;
                activeAbilities.Add((Ability)newAbility);
                Invoke("stopJumping", abilityTime);
                GetComponent<AudioSource>().PlayOneShot(jump);


            }
        } else if (newAbility == Ability.Duck)
        {
            if (abilityImages[Ability.Duck].fillAmount == 0)
            {
                this.GetComponent<Animator>().SetBool("isDucking", true);
                abilityImages[Ability.Duck].fillAmount = 1;
                activeAbilities.Add((Ability)newAbility);
                Invoke("stopDucking", abilityTime);
                GetComponent<AudioSource>().PlayOneShot(duck);

            }
        } else if (newAbility == Ability.Bash)
        {
            if (abilityImages[Ability.Bash].fillAmount == 0)
            {
                this.GetComponent<Animator>().SetBool("isBashing", true);
                abilityImages[Ability.Bash].fillAmount = 1;
                activeAbilities.Add((Ability)newAbility);
                Invoke("stopBashing", abilityTime);
                GetComponent<AudioSource>().PlayOneShot(bash);
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
