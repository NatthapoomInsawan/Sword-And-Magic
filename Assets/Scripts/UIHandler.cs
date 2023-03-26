using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIHandler : MonoBehaviour
{
    public enum TurnState
    {
        combatPanel,
        attack,
        skill,
        skillTarget,
        move,
        item,
        itemTarget
    }

    //Panel
    [Header("Panel")]
    public GameObject mCombatPanel;
    public GameObject mSkillPanel;
    public GameObject mTargetEnemyPanel;
    public GameObject mTargetPlayerPanel;
    public GameObject mItemPanel;
    [Header("Cost and Item")]
    public GameObject costAndAmount;
    [Header("Battle Handler reference")]
    public GameObject battleHandler;

    private TurnState turnState;

    private bool isAttack;
    private bool isSkill;
    private bool isItem;
    private bool isMove;

    // Start is called before the first frame update
    void Start()
    {
        OnCombatPanel();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateUI();
        UpdateCostAndAmount();
    }

    public void OnCombatPanel()
    {
        turnState = TurnState.combatPanel;

        if(battleHandler.GetComponent<BattleHandler>().GetSelectedCharacter().CompareTag("Enemy"))
        {
            mCombatPanel.transform.GetChild(0).GetComponent<Button>().interactable = false;
            mCombatPanel.transform.GetChild(1).GetComponent<Button>().interactable = false;
            mCombatPanel.transform.GetChild(2).GetComponent<Button>().interactable = false;
            mCombatPanel.transform.GetChild(3).GetComponent<Button>().interactable = false;
            gameObject.transform.GetChild(5).GetComponent<Button>().interactable = false;
        }
        else //Player Turn
        {
            if (isItem)
                mCombatPanel.transform.GetChild(2).GetComponent<Button>().interactable = false;
            else
                mCombatPanel.transform.GetChild(2).GetComponent<Button>().interactable = true;

            if (isAttack)
            {
                mCombatPanel.transform.GetChild(0).GetComponent<Button>().interactable = false;
                //can't use Item if attacked
                mCombatPanel.transform.GetChild(2).GetComponent<Button>().interactable = false;
            }
            else
                mCombatPanel.transform.GetChild(0).GetComponent<Button>().interactable = true;

            if (isSkill)
                mCombatPanel.transform.GetChild(1).GetComponent<Button>().interactable = false;
            else
                mCombatPanel.transform.GetChild(1).GetComponent<Button>().interactable = true;

            if (isMove)
                mCombatPanel.transform.GetChild(3).GetComponent<Button>().interactable = false;
            else
                mCombatPanel.transform.GetChild(3).GetComponent<Button>().interactable = true;

            gameObject.transform.GetChild(5).GetComponent<Button>().interactable = true;
        }

    }

    public void OnTurnEndEvent()
    {
        isAttack = false;
        isSkill = false;
        isItem = false;
        isMove = false;

        //Call combat panel
        OnCombatPanel();
    }

    public void OnAttackEvent()
    {
        turnState = TurnState.attack;

        DrawEnemyTargetting();

    }

    public void OnSkillEvent()
    {
        turnState = TurnState.skill;

        DrawSkill();
    }

    public void OnItemEvent()
    {
        turnState = TurnState.item;

        DrawItem();
    }

    public void OnMoveEvent()
    {
        turnState = TurnState.move;

        DrawPlayerTargetting();
    }

    public void OnResetButton()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public TurnState GetTurnState()
    {
        return turnState;
    }

    private void UpdateUI()
    {
        switch (turnState)
        {
            case TurnState.combatPanel:
                mCombatPanel.SetActive(true);
                mSkillPanel.SetActive(false);
                mTargetEnemyPanel.SetActive(false);
                mTargetPlayerPanel.SetActive(false);
                mItemPanel.SetActive(false);
                break;
            case TurnState.attack:
                mCombatPanel.SetActive(false);
                mSkillPanel.SetActive(false);
                mTargetEnemyPanel.SetActive(true);
                mTargetPlayerPanel.SetActive(false);
                mItemPanel.SetActive(false);
                break;
            case TurnState.skill:
                mCombatPanel.SetActive(false);
                mSkillPanel.SetActive(true);
                mTargetEnemyPanel.SetActive(false);
                mTargetPlayerPanel.SetActive(false);
                mItemPanel.SetActive(false);
                break;
            case TurnState.item:
                mCombatPanel.SetActive(false);
                mSkillPanel.SetActive(false);
                mTargetEnemyPanel.SetActive(false);
                mTargetPlayerPanel.SetActive(false);
                mItemPanel.SetActive(true);
                break;
            case TurnState.move:
                mCombatPanel.SetActive(false);
                mSkillPanel.SetActive(false);
                mTargetEnemyPanel.SetActive(false);
                mTargetPlayerPanel.SetActive(true);
                mItemPanel.SetActive(false);
                break;
            case TurnState.skillTarget:
                mCombatPanel.SetActive(false);
                mSkillPanel.SetActive(false);
                mItemPanel.SetActive(false);
                break;
            case TurnState.itemTarget:
                mCombatPanel.SetActive(false);
                mSkillPanel.SetActive(false);
                mItemPanel.SetActive(false);
                break;
        }
    }

    private void DrawEnemyTargetting()
    {
        GameObject[] playerLane = battleHandler.GetComponent<BattleHandler>().GetPlayerLane();
        GameObject selectedCharacter = battleHandler.GetComponent<BattleHandler>().GetSelectedCharacter();
        GameObject[] enemyLane = battleHandler.GetComponent<BattleHandler>().GetEnemyLane();

        //Draw enemy Selection name
        for (int i = 0; i < 4; i++)
        {
            if (enemyLane[i] == null)
            {
                //change text
                mTargetEnemyPanel.transform.GetChild(i).GetComponentInChildren<TMPro.TextMeshProUGUI>().text = "None";
                //set interactable to false
                mTargetEnemyPanel.transform.GetChild(i).GetComponent<Button>().interactable = false;
            }
            else
            {
                //Marked Enemy name
                if (enemyLane[i].GetComponent<Character>().IsMark && selectedCharacter.name == "Ranger")
                    mTargetEnemyPanel.transform.GetChild(i).GetComponentInChildren<TMPro.TextMeshProUGUI>().color = Color.green;
                else
                    mTargetEnemyPanel.transform.GetChild(i).GetComponentInChildren<TMPro.TextMeshProUGUI>().color = Color.black;

                //change text
                mTargetEnemyPanel.transform.GetChild(i).GetComponentInChildren<TMPro.TextMeshProUGUI>().text = enemyLane[i].name;
                //set interactable to true
                mTargetEnemyPanel.transform.GetChild(i).GetComponent<Button>().interactable = true;
            }

        }

        //Fighter
        if (selectedCharacter.name == "Fighter")
        {
            for (int i = 0; i < 4; i++)
            {
                //set interactable to false if not in the same lane
                if (System.Array.IndexOf(playerLane, selectedCharacter) == i)
                    mTargetEnemyPanel.transform.GetChild(i).GetComponent<Button>().interactable = true;
                else
                    mTargetEnemyPanel.transform.GetChild(i).GetComponent<Button>().interactable = false;
            }
        }

        //check if enemy is dead
        for (int i = 0; i < 4; i++)
        {
            //set button interactable to false if enemy is dead
            if (enemyLane[i].GetComponent<Character>().GetState() == Character.State.dead)
                mTargetEnemyPanel.transform.GetChild(i).GetComponent<Button>().interactable = false;
        }
    }

    private void DrawPlayerTargetting()
    {
        GameObject[] playerLane = battleHandler.GetComponent<BattleHandler>().GetPlayerLane();

        //Draw player selection name
        for (int i = 0; i < 4; i++)
        {
            if (playerLane[i] == null)
            {
                //change text
                mTargetPlayerPanel.transform.GetChild(i).GetComponentInChildren<TMPro.TextMeshProUGUI>().text = "None";
                //set interactable to false
                mTargetPlayerPanel.transform.GetChild(i).GetComponent<Button>().interactable = false;
            }
            else
            {
                //change text
                mTargetPlayerPanel.transform.GetChild(i).GetComponentInChildren<TMPro.TextMeshProUGUI>().text = playerLane[i].name;
                //set interactable to true
                mTargetPlayerPanel.transform.GetChild(i).GetComponent<Button>().interactable = true;
            }

        }
    }

    public void SelectTarget(Button button)
    {
        string buttonText = button.transform.GetChild(0).GetComponentInChildren<TMPro.TextMeshProUGUI>().text;

        if (turnState == TurnState.attack) //call Attack
        {
            for (int i = 0; i < 4; i++)
            {
                string enemyName = battleHandler.GetComponent<BattleHandler>().GetEnemyLane()[i].name;

                if (buttonText == enemyName)
                {
                    //set currentTarget
                    battleHandler.GetComponent<BattleHandler>().SetCurrentTarget(battleHandler.GetComponent<BattleHandler>().GetEnemyLane()[i]);
                }
            }

            battleHandler.GetComponent<BattleHandler>().Attack();
            isAttack = true;

            OnCombatPanel();
        }
        else if (turnState == TurnState.skillTarget) //call UseSkill
        {
            Skill selectedSkill = battleHandler.GetComponent<BattleHandler>().SelectedSkill;

            if (selectedSkill.Target == Skill.TargetType.enemy)
            {
                for (int i = 0; i < 4; i++)
                {

                    string enemyName = battleHandler.GetComponent<BattleHandler>().GetEnemyLane()[i].name;

                    if (buttonText == enemyName)
                    {
                        //set currentTarget
                        battleHandler.GetComponent<BattleHandler>().SetCurrentTarget(battleHandler.GetComponent<BattleHandler>().GetEnemyLane()[i]);
                    }
                }
            }
            else if (selectedSkill.Target == Skill.TargetType.ally)
            {
                for (int i = 0; i < 4; i++)
                {

                    string allyName = battleHandler.GetComponent<BattleHandler>().GetPlayerLane()[i].name;

                    if (buttonText == allyName)
                    {
                        //set currentTarget
                        battleHandler.GetComponent<BattleHandler>().SetCurrentTarget(battleHandler.GetComponent<BattleHandler>().GetPlayerLane()[i]);
                    }
                }
            }

            battleHandler.GetComponent<BattleHandler>().UseSkill();
            isSkill = true;
            OnCombatPanel();

        }
        else if (turnState == TurnState.itemTarget) //call UseItem
        {
            Item selectedItem = battleHandler.GetComponent<BattleHandler>().SelectedItem;

            if (selectedItem.Target == Item.TargetType.enemy)
            {
                for (int i = 0; i < 4; i++)
                {

                    string enemyName = battleHandler.GetComponent<BattleHandler>().GetEnemyLane()[i].name;

                    if (buttonText == enemyName)
                    {
                        //set currentTarget
                        battleHandler.GetComponent<BattleHandler>().SetCurrentTarget(battleHandler.GetComponent<BattleHandler>().GetEnemyLane()[i]);
                    }
                }
            }
            else if (selectedItem.Target == Item.TargetType.ally)
            {
                for (int i = 0; i < 4; i++)
                {

                    string allyName = battleHandler.GetComponent<BattleHandler>().GetPlayerLane()[i].name;

                    if (buttonText == allyName)
                    {
                        //set currentTarget
                        battleHandler.GetComponent<BattleHandler>().SetCurrentTarget(battleHandler.GetComponent<BattleHandler>().GetPlayerLane()[i]);
                    }
                }
            }

            battleHandler.GetComponent<BattleHandler>().UseItem();
            isItem = true;
            isAttack = true;
            OnCombatPanel();
        }
        else if (turnState == TurnState.move) //call Move
        {
            for (int i = 0; i < 4; i++)
            {
                string allyName = battleHandler.GetComponent<BattleHandler>().GetPlayerLane()[i].name;

                if (buttonText == allyName)
                {
                    //set currentTarget
                    battleHandler.GetComponent<BattleHandler>().SetCurrentTarget(battleHandler.GetComponent<BattleHandler>().GetPlayerLane()[i]);
                }
            }

            battleHandler.GetComponent<BattleHandler>().Move();
            isMove = true;

            OnCombatPanel();
        }
    }

    public void OnSkillSelectEvent(Button button)
    {
        //read Skill
        string buttonText = button.transform.GetChild(0).GetComponentInChildren<TMPro.TextMeshProUGUI>().text;
        Skill[] skills = battleHandler.GetComponent<BattleHandler>().GetSelectedCharacter().GetComponent<Character>().GetSkills();

        for (int i = 0; i < 4; i++)
        {
            if (skills[i] == null)
                continue; //skip if null
            if (buttonText == skills[i].Name)
            {
                battleHandler.GetComponent<BattleHandler>().SelectedSkill = skills[i];
            }
        }

        Skill selectedSkill = battleHandler.GetComponent<BattleHandler>().SelectedSkill;

        switch (selectedSkill.Target)
        {
            case Skill.TargetType.enemy:
                turnState = TurnState.skillTarget;
                DrawEnemyTargetting();
                mTargetEnemyPanel.SetActive(true);
                if (selectedSkill.Name == "Magic Missile")
                {
                    isAttack = true;
                    isItem = true;
                }
                break;
            case Skill.TargetType.ally:
                turnState = TurnState.skillTarget;
                DrawPlayerTargetting();
                mTargetPlayerPanel.SetActive(true);
                break;
            case Skill.TargetType.self:
                //set currentTarget
                battleHandler.GetComponent<BattleHandler>().SetCurrentTarget(battleHandler.GetComponent<BattleHandler>().GetSelectedCharacter());
                battleHandler.GetComponent<BattleHandler>().UseSkill();
                isSkill = true;
                OnCombatPanel();
                break;
        }

    }

    private void DrawSkill()
    {
        Skill[] playerSkill = battleHandler.GetComponent<BattleHandler>().GetSelectedCharacter().GetComponent<Character>().GetSkills();
        GameObject selectedCharacter = battleHandler.GetComponent<BattleHandler>().GetSelectedCharacter();

        for (int i = 0; i < 4; i++)
        {
            if (playerSkill[i] == null)
            {
                //set text to none
                mSkillPanel.transform.GetChild(i).GetComponentInChildren<TMPro.TextMeshProUGUI>().text = "None";
                //set button interactable
                mSkillPanel.transform.GetChild(i).GetComponent<Button>().interactable = false;
            }
            else
            {
                //set text to none
                mSkillPanel.transform.GetChild(i).GetComponentInChildren<TMPro.TextMeshProUGUI>().text = playerSkill[i].Name;
                //set button interactable
                mSkillPanel.transform.GetChild(i).GetComponent<Button>().interactable = true;
            }
        }

        //Check cost
        for (int i = 0; i < 4; i++)
        {
            if (playerSkill[i] == null)
                continue; //skip if null
            int playerCost = selectedCharacter.GetComponent<Character>().GetCost(); //Player cost
            if (playerCost < playerSkill[i].Cost)
            {
                mSkillPanel.transform.GetChild(i).GetComponent<Button>().interactable = false;
            }
        }

        //Check Once Per Fight
        for (int i = 0; i < 4; i++)
        {
            if (playerSkill[i] == null)
                continue; //skip if null
            if (playerSkill[i].OncePerFight == true && playerSkill[i].IsUsed == true)
            {
                mSkillPanel.transform.GetChild(i).GetComponent<Button>().interactable = false;
            }
        }

        //Magic missile
        for (int i = 0; i < 4; i++)
        {
            if (playerSkill[i] == null)
                continue; //skip if null
            if (playerSkill[i].Name == "Magic Missile" && isAttack)
            {
                mSkillPanel.transform.GetChild(i).GetComponent<Button>().interactable = false;
            }
        }

    }

    private void DrawItem()
    {
        Item[] inventory = battleHandler.GetComponent<BattleHandler>().GetInventory();

        for (int i = 0; i < 4; i++)
        {
            if (inventory[i] == null)
            {
                //set text to none
                mItemPanel.transform.GetChild(i).GetComponentInChildren<TMPro.TextMeshProUGUI>().text = "None";
                //set button interactable
                mItemPanel.transform.GetChild(i).GetComponent<Button>().interactable = false;
            }
            else
            {
                //set text to none
                mItemPanel.transform.GetChild(i).GetComponentInChildren<TMPro.TextMeshProUGUI>().text = inventory[i].Name;
                //set button interactable
                mItemPanel.transform.GetChild(i).GetComponent<Button>().interactable = true;
            }
        }

        //Check cost
        for (int i = 0; i < 4; i++)
        {
            if (inventory[i] == null)
                continue; //skip if null
            int itemAmount = inventory[i].Amount; //Player cost
            if (itemAmount <= 0)
            {
                mItemPanel.transform.GetChild(i).GetComponent<Button>().interactable = false;
            }
        }

    }

    public void OnItemSelectEvent(Button button)
    {
        //read Skill
        string buttonText = button.transform.GetChild(0).GetComponentInChildren<TMPro.TextMeshProUGUI>().text;
        Item[] items = battleHandler.GetComponent<BattleHandler>().GetInventory();

        for (int i = 0; i < 4; i++)
        {
            if (items[i] == null)
                continue; //skip if null
            if (buttonText == items[i].Name)
            {
                battleHandler.GetComponent<BattleHandler>().SelectedItem = items[i];
            }
        }

        Item selectedItem = battleHandler.GetComponent<BattleHandler>().SelectedItem;

        switch (selectedItem.Target)
        {
            case Item.TargetType.enemy:
                turnState = TurnState.itemTarget;
                DrawEnemyTargetting();
                mTargetEnemyPanel.SetActive(true);
                break;
            case Item.TargetType.ally:
                turnState = TurnState.itemTarget;
                DrawPlayerTargetting();
                mTargetPlayerPanel.SetActive(true);
                break;
        }

    }

    private void UpdateCostAndAmount ()
    {
        int characterCost = battleHandler.GetComponent<BattleHandler>().GetSelectedCharacter().GetComponent<Character>().GetCost();
        int itemAmount = battleHandler.GetComponent<BattleHandler>().GetInventory()[0].Amount;

        switch(turnState)
        {
            case TurnState.skill:
                costAndAmount.transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = "COST: " + characterCost.ToString();
                costAndAmount.SetActive(true);
                break;
            case TurnState.item:
                costAndAmount.transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = "AMOUNT: " + itemAmount.ToString();
                costAndAmount.SetActive(true);
                break;
            default:
                costAndAmount.SetActive(false);
                break;
        }
    }

}
