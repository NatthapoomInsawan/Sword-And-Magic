using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Character : MonoBehaviour
{
    public enum State 
    {
        idle,
        attack,
        dead
    }
    [Header("Status")]
    [SerializeField] private int currentHP;
    [SerializeField] private int maxHP;
    [SerializeField] private int AC;
    [SerializeField] private int DC;
    [SerializeField] private int normalAttack;
    [Header("Damage")]
    [SerializeField] private int maxDamage;
    [SerializeField] private int minDamage;
    [SerializeField] private int save;
    [SerializeField] private State state = State.idle;
    [SerializeField] private int cost;
    public bool IsMark { get; set; }

    [Header("Animator References")]
    [SerializeField] private Animator animator;

    [Header("HPSlider References")]
    [SerializeField] private Slider HPslider;

    readonly private Skill[] skills = new Skill[4];

    // Start is called before the first frame update
    void Start()
    {
        UpdateHPSlider();

        //initiate skills
        Skill secondWind = new ("Second Wind", -11, -2, Skill.TargetType.self, 0, true);
        Skill hunterMark = new ("Hunter's Mark", 0, 0, Skill.TargetType.enemy, 0, true);
        Skill magicMissle = new ("Magic Missile", 2, 6, Skill.TargetType.enemy, 1, false);
        Skill healingWord = new ("Healing Word", -8, -4, Skill.TargetType.ally, 1, false);
        switch (this.gameObject.name)
        {
            case "Fighter":
                skills[0] = secondWind;
                break;
            case "Ranger":
                skills[0] = hunterMark;
                break;
            case "Wizard":
                skills[0] = magicMissle;
                break;
            case "Cleric":
                skills[0] = healingWord;
                break;
        }

    }

    // Update is called once per frame
    void Update()
    {
        UpdateAnimation();
        UpdateHPSlider();
    }

    public void TakeDamage(int recievedDamage)
    {
        if ((currentHP-recievedDamage) < 0) //Check if HP is reduced morethan zero
        {
            currentHP = 0;
        }
        else if ((currentHP-recievedDamage) > maxHP) //Check if HP is increase morethan maxHP
        {
            currentHP = maxHP;
        }
        else
        {
            currentHP -= recievedDamage; //take damage
        }

        if ((state == State.dead) && currentHP > 0) //update state if healed
        {
            state = State.idle;
        }
        else if (currentHP == 0) //update state if die
        {
            state = State.dead;
        }
    }

    public int GetAttack()
    {
        return normalAttack;
    }

    public int GetDamage()
    {
        return Random.Range(minDamage,maxDamage);
    }

    public int GetAC()
    {
        return AC;
    }

    public int GetDC()
    {
        return DC;
    }

    public State GetState()
    {
        return state;
    }

    public void SetState(State newstate)
    {
        state = newstate;
    }

    public int GetSave()
    {
        return save;
    }

    public int GetCost()
    {
        return cost;
    }

    public void ReduceCost(int reduce)
    {
        if (cost - reduce < 0)
            cost = 0;
        else
            cost -= reduce;
    }

    public Skill[] GetSkills()
    {
        return skills;
    }

    private void UpdateAnimation()
    {
        if (animator == null)
        {
            return;
        }

        switch(state)
        {
            case State.idle:
                animator.SetBool("isIdle", true);
                animator.SetBool("isAttack", false);
                animator.SetBool("isDead", false);
                break;
            case State.attack:
                animator.SetBool("isIdle", false);
                animator.SetBool("isAttack", true);
                animator.SetBool("isDead", false);
                break;
            case State.dead:
                animator.SetBool("isIdle", false);
                animator.SetBool("isAttack", false);
                animator.SetBool("isDead", true);
                break;
        }
    }

    private void UpdateHPSlider()
    {
        if (HPslider == null)
        {
            return;
        }

        if (currentHP == 0)
        {
            HPslider.fillRect.gameObject.SetActive(false);
        }
        else
        {
            HPslider.fillRect.gameObject.SetActive(true);
        }

        HPslider.value = currentHP;
        HPslider.minValue = 0;
        HPslider.maxValue = maxHP;

    }
}
