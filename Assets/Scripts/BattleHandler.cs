using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleHandler : MonoBehaviour
{
    public enum BattleState
    {
        ingame,
        won,
        lose
    }

    [Header("Player")]
    public GameObject player1;
    public GameObject player2;
    public GameObject player3;
    public GameObject player4;
    [Header("Enemy")]
    public GameObject enemy1;
    public GameObject enemy2;
    public GameObject enemy3;
    public GameObject enemy4;
    [Header("Damage Text")]
    public GameObject damageText;

    [Header("UI Handler")]
    public GameObject UIhandler;

    readonly private List<GameObject> characterList = new();
    private GameObject selectedCharacter;
    private GameObject currentTarget;

    private BattleState battleState;

    readonly private GameObject[] playerLane = new GameObject[4];
    readonly private GameObject[] enemyLane = new GameObject[4];

    readonly private Item[] inventory = new Item[4];

    public Skill SelectedSkill { get; set; }
    public Item SelectedItem { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        //declare battle state
        battleState = BattleState.ingame;

        selectedCharacter = player1;

        //addGameObject to List
        characterList.Add(player1);
        characterList.Add(enemy1);
        characterList.Add(player2);
        characterList.Add(enemy2);
        characterList.Add(player3);
        characterList.Add(enemy3);
        characterList.Add(player4);
        characterList.Add(enemy4);

        //add player to playerLane
        playerLane[0] = player1;
        playerLane[1] = player2;
        playerLane[2] = player3;
        playerLane[3] = player4;

        //add enemy to enemyLane
        enemyLane[0] = enemy1;
        enemyLane[1] = enemy2;
        enemyLane[2] = enemy3;
        enemyLane[3] = enemy4;

        //add item to items
        inventory[0] = new Item("Potion of Healing", -10, -2, Item.TargetType.ally, 3);

    }

    // Update is called once per frame
    void Update()
    {

        UpdateSelection();

        //GameOver Check
        switch (battleState)
        {
            case BattleState.won:
                UIhandler.transform.GetChild(8).GetComponentInChildren<TMPro.TextMeshProUGUI>().text = "VICTORY";
                UIhandler.transform.GetChild(8).GetComponentInChildren<TMPro.TextMeshProUGUI>().color = new Color(255, 215, 0);
                UIhandler.transform.GetChild(8).gameObject.SetActive(true);
                break;
            case BattleState.lose:
                UIhandler.transform.GetChild(8).GetComponentInChildren<TMPro.TextMeshProUGUI>().text = "DEFEATED";
                UIhandler.transform.GetChild(8).GetComponentInChildren<TMPro.TextMeshProUGUI>().color = new Color(169, 169, 169);
                UIhandler.transform.GetChild(8).gameObject.SetActive(true);
                break;
            case BattleState.ingame:
                if (enemy1.GetComponent<Character>().GetState() == Character.State.dead &&
                    enemy2.GetComponent<Character>().GetState() == Character.State.dead &&
                    enemy3.GetComponent<Character>().GetState() == Character.State.dead &&
                    enemy4.GetComponent<Character>().GetState() == Character.State.dead)
                    battleState = BattleState.won;
                else if (player1.GetComponent<Character>().GetState() == Character.State.dead &&
                    player2.GetComponent<Character>().GetState() == Character.State.dead &&
                    player3.GetComponent<Character>().GetState() == Character.State.dead &&
                    player4.GetComponent<Character>().GetState() == Character.State.dead)
                    battleState = BattleState.lose;
                break;
        }
    }

    public BattleState GetBattleState()
    {
        return battleState;
    }


    public GameObject GetSelectedCharacter()
    {
        return selectedCharacter;
    }

    public void SetCurrentTarget(GameObject target)
    {
        currentTarget = target;
    }

    public GameObject[] GetPlayerLane()
    {
        return playerLane;
    }

    public GameObject[] GetEnemyLane()
    {
        return enemyLane;
    }

    public Item[] GetInventory()
    {
        return inventory;
    }

    private void UpdateSelection()
    {

        foreach (GameObject character in characterList)
        {
            if (character == null)
            {
                return;
            }

            if (character == selectedCharacter)
            {
                selectedCharacter.transform.GetChild(0).gameObject.SetActive(true);
            }
            else
            {
                character.transform.GetChild(0).gameObject.SetActive(false);
            }
        }
    }

    public void NextTurn()
    {
        if (battleState != BattleState.ingame)
            return;

        if (characterList.IndexOf(selectedCharacter) < (characterList.Count - 1))
            selectedCharacter = characterList[characterList.IndexOf(selectedCharacter) + 1];
        else
            selectedCharacter = characterList[0];

        //Check if character is null or dead
        if (selectedCharacter == null || selectedCharacter.GetComponent<Character>().GetState() == Character.State.dead)
        {
            NextTurn();
        }

        Debug.Log("next turn called");
        Debug.Log("This is " + selectedCharacter.name + "Turn" );

        //if Enemy turn
        if (selectedCharacter.CompareTag("Enemy"))
        {
            EnemyTurn();
            Debug.Log("Call EnmyTurn");
        }
        else //Player Turn
        {
            UIhandler.GetComponent<UIHandler>().OnCombatPanel();
        }


    }

    public void Attack()
    {
        StartCoroutine(DoAttack());
    }

    public void UseSkill()
    {
        StartCoroutine(DoSkill());
    }

    public void UseItem()
    {
        StartCoroutine(DoItem());
    }

    private void EnemyTurn()
    {
        StartCoroutine(DoEnemyAI());

    }

    public void Move()
    {
        Vector3 positionHolder = selectedCharacter.transform.position;
        int selectedIndex;
        int targetIndex;

        if (selectedCharacter.CompareTag("Player")) //Player move
        {
            selectedIndex = System.Array.IndexOf(playerLane, selectedCharacter);
            targetIndex = System.Array.IndexOf(playerLane, currentTarget);
        }
        else //Enemy move
        {
            selectedIndex = System.Array.IndexOf(enemyLane, selectedCharacter);
            targetIndex = System.Array.IndexOf(enemyLane, currentTarget);
        }

        //move to target
        selectedCharacter.transform.position = currentTarget.transform.position;
        if (selectedCharacter.CompareTag("Player"))
            playerLane[targetIndex] = selectedCharacter;
        else
            enemyLane[targetIndex] = selectedCharacter;
        //target move to selectedCharacter
        currentTarget.transform.position = positionHolder;
        if (selectedCharacter.CompareTag("Player"))
            playerLane[selectedIndex] = currentTarget;
        else
            enemyLane[selectedIndex] = currentTarget;
    }

    IEnumerator DoAttack()
    {
        int attackerAttack = selectedCharacter.GetComponent<Character>().GetAttack();
        int attackerDamage = selectedCharacter.GetComponent<Character>().GetDamage();
        int attackerDC = selectedCharacter.GetComponent<Character>().GetDC();
        int targetAC = currentTarget.GetComponent<Character>().GetAC();
        int targetSave = currentTarget.GetComponent<Character>().GetSave();
        int roll = Random.Range(1, 20);

        //attack Animation
        selectedCharacter.GetComponent<Character>().SetState(Character.State.attack);
        yield return new WaitForSeconds(0.2f);

        if (selectedCharacter.name == "Cleric")
        {
            if ((roll+targetSave) >= attackerDC) //if roll >= save equal evade success
            {
                GameObject popUpText = Instantiate(damageText, currentTarget.transform.position, Quaternion.identity);
                popUpText.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = "evade";
            }
            else
            {
                int damage = attackerDamage;
                currentTarget.GetComponent<Character>().TakeDamage(damage);
                GameObject popUpText = Instantiate(damageText, currentTarget.transform.position, Quaternion.identity);
                popUpText.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = damage.ToString();
            }
        }
        else
        {
            if ((roll += attackerAttack) >= targetAC) //if roll >= AC equal a hit
            {
                int damage = attackerDamage;
                currentTarget.GetComponent<Character>().TakeDamage(damage);
                GameObject popUpText = Instantiate(damageText, currentTarget.transform.position, Quaternion.identity);
                popUpText.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = damage.ToString();

                //Hunter's Mark
                if (selectedCharacter.name == "Ranger" && currentTarget.GetComponent<Character>().IsMark)
                {
                    int markDamage = Random.Range(1, 6);
                    yield return new WaitForSeconds(0.2f);
                    currentTarget.GetComponent<Character>().TakeDamage(markDamage);
                    GameObject markText = Instantiate(damageText, currentTarget.transform.position, Quaternion.identity);
                    markText.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = markDamage.ToString();
                }
            }
            else
            {
                GameObject popUpText = Instantiate(damageText, currentTarget.transform.position, Quaternion.identity);
                popUpText.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = "miss";
            }
        }

        yield return new WaitForSeconds(1.0f);

        //back to idle
        selectedCharacter.GetComponent<Character>().SetState(Character.State.idle);

        if (selectedCharacter.tag.Equals("Enemy"))
        {
            NextTurn();
        }
    }

    IEnumerator DoSkill()
    {
        int damage = SelectedSkill.GetDamage();

        //SkillText
        GameObject skillText = Instantiate(damageText, selectedCharacter.transform.position, Quaternion.identity);
        skillText.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = SelectedSkill.Name;

        //atack Animation
        if (SelectedSkill.GetDamage() == 0)
            selectedCharacter.GetComponent<Character>().SetState(Character.State.idle);
        else if (SelectedSkill.Target == Skill.TargetType.self)
            selectedCharacter.GetComponent<Character>().SetState(Character.State.idle);
        else
            selectedCharacter.GetComponent<Character>().SetState(Character.State.attack);


        yield return new WaitForSeconds(0.5f);


        switch (SelectedSkill.Name)
        {
            case "Magic Missile":
                damage *= 3;
                break;
            case "Hunter's Mark":
                currentTarget.GetComponent<Character>().IsMark = true;
                break;
        }

        if (damage < 0) //if Heal
        {
            //Damaging
            currentTarget.GetComponent<Character>().TakeDamage(damage);
            damage *= -1;
            GameObject popUpText = Instantiate(damageText, currentTarget.transform.position, Quaternion.identity);
            popUpText.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = damage.ToString();
            popUpText.GetComponentInChildren<TMPro.TextMeshProUGUI>().color = Color.green;
        }
        else if (damage > 0) //if selected skill is an attack skill
        {
            //Damaging
            currentTarget.GetComponent<Character>().TakeDamage(damage);
            GameObject popUpText = Instantiate(damageText, currentTarget.transform.position, Quaternion.identity);
            popUpText.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = damage.ToString();

        }


        if (SelectedSkill.OncePerFight)
            SelectedSkill.IsUsed = true;

        if (SelectedSkill.Cost > 0)
            selectedCharacter.GetComponent<Character>().ReduceCost(SelectedSkill.Cost);

        yield return new WaitForSeconds(1.0f);

        //back to idle
        selectedCharacter.GetComponent<Character>().SetState(Character.State.idle);
    }

    IEnumerator DoItem()
    {
        int damage = SelectedItem.GetDamage();

        //SkillText
        GameObject itemText = Instantiate(damageText, selectedCharacter.transform.position, Quaternion.identity);
        itemText.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = SelectedItem.Name;

        yield return new WaitForSeconds(0.5f);

        if (damage < 0) //if Heal
        {
            //Damaging
            currentTarget.GetComponent<Character>().TakeDamage(damage);
            damage *= -1;
            GameObject popUpText = Instantiate(damageText, currentTarget.transform.position, Quaternion.identity);
            popUpText.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = damage.ToString();
            popUpText.GetComponentInChildren<TMPro.TextMeshProUGUI>().color = Color.green;
        }
        else if (damage > 0)
        {
            //Damaging
            currentTarget.GetComponent<Character>().TakeDamage(damage);
            GameObject popUpText = Instantiate(damageText, currentTarget.transform.position, Quaternion.identity);
            popUpText.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = damage.ToString();
        }

        SelectedItem.Amount -= 1;

        yield return new WaitForSeconds(1.0f);

        //back to idle
        selectedCharacter.GetComponent<Character>().SetState(Character.State.idle);
    }

    //EnemyAI
    IEnumerator DoEnemyAI()
    {
        int index = System.Array.IndexOf(enemyLane, selectedCharacter);

        while (playerLane[index].GetComponent<Character>().GetState() == Character.State.dead)
        {
            if (index + 1 > 3)
                currentTarget = enemyLane[0];
            else
                currentTarget = enemyLane[index + 1];


            Move();

            Debug.Log("after move index: " + index.ToString());

            index = System.Array.IndexOf(enemyLane, selectedCharacter);

        }

        //Select Target
        currentTarget = playerLane[index];

        yield return DoAttack();

    }
}
