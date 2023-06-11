using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIHandler : MonoBehaviour
{

    public IUIState currentUIState;

    public enum PlayerAction
    {
        attack,
        useSkill,
        useItem,
        move,
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
    public BattleHandler battleHandler;

    private PlayerAction playerAction;

    public bool isAttacked;
    public bool skillUsed;
    public bool itemUsed;
    public bool isMoved;

    // Start is called before the first frame update
    void Start()
    {
        currentUIState = new MainPanel(this);
    }

    #region Combat Panel Button
    public void OnCombatPanel()
    {
        UpdateUIState(new MainPanel(this));
    }

    public void OnTurnEndEvent()
    {
        isAttacked = false;
        skillUsed = false;
        itemUsed = false;
        isMoved = false;

        //Call combat panel
        OnCombatPanel();
    }

    public void OnAttackEvent()
    {
        playerAction = PlayerAction.attack;
        UpdateCostAndAmount();
        UpdateUIState(new TargetEnemyPanel(this));
    }

    public void OnSkillEvent()
    {
        playerAction = PlayerAction.useSkill;
        UpdateCostAndAmount();
        UpdateUIState(new SkillPanel(this));
    }

    public void OnItemEvent()
    {
        playerAction = PlayerAction.useItem;
        UpdateCostAndAmount();
        UpdateUIState(new ItemPanel(this));
    }

    public void OnMoveEvent()
    {
        playerAction = PlayerAction.move;
        UpdateUIState(new TargetPlayerPanel(this));
    }

    public void OnResetButton()
    {
        SceneManager.LoadScene("SampleScene");
    }

    #endregion

    #region Select Target Button
    public void SelectTarget(Button button)
    {
        string buttonText = button.transform.GetChild(0).GetComponentInChildren<TMPro.TextMeshProUGUI>().text;
        UpdateCostAndAmount();

        if (playerAction == PlayerAction.attack) //call Attack
        {
            for (int i = 0; i < 4; i++)
            {
                string enemyName = battleHandler.GetEnemyLane()[i].name;

                if (buttonText == enemyName)
                {
                    //set currentTarget
                    battleHandler.SetCurrentTarget(battleHandler.GetEnemyLane()[i]);
                }
            }

            battleHandler.Attack();
            isAttacked = true;

            OnCombatPanel();
        }
        else if (playerAction == PlayerAction.useSkill) //call UseSkill
        {
            Skill selectedSkill = battleHandler.SelectedSkill;

            if (selectedSkill.Target == Skill.TargetType.enemy)
            {
                for (int i = 0; i < 4; i++)
                {

                    string enemyName = battleHandler.GetEnemyLane()[i].name;

                    if (buttonText == enemyName)
                    {
                        //set currentTarget
                        battleHandler.GetComponent<BattleHandler>().SetCurrentTarget(battleHandler.GetEnemyLane()[i]);
                    }
                }
            }
            else if (selectedSkill.Target == Skill.TargetType.ally)
            {
                for (int i = 0; i < 4; i++)
                {

                    string allyName = battleHandler.GetPlayerLane()[i].name;

                    if (buttonText == allyName)
                    {
                        //set currentTarget
                        battleHandler.GetComponent<BattleHandler>().SetCurrentTarget(battleHandler.GetPlayerLane()[i]);
                    }
                }
            }

            battleHandler.GetComponent<BattleHandler>().UseSkill();
            skillUsed = true;
            UpdateCostAndAmount();
            OnCombatPanel();

        }
        else if (playerAction == PlayerAction.useItem) //call UseItem
        {
            Item selectedItem = battleHandler.SelectedItem;

            if (selectedItem.Target == Item.TargetType.enemy)
            {
                for (int i = 0; i < 4; i++)
                {

                    string enemyName = battleHandler.GetEnemyLane()[i].name;

                    if (buttonText == enemyName)
                    {
                        //set currentTarget
                        battleHandler.SetCurrentTarget(battleHandler.GetEnemyLane()[i]);
                    }
                }
            }
            else if (selectedItem.Target == Item.TargetType.ally)
            {
                for (int i = 0; i < 4; i++)
                {

                    string allyName = battleHandler.GetPlayerLane()[i].name;

                    if (buttonText == allyName)
                    {
                        //set currentTarget
                        battleHandler.SetCurrentTarget(battleHandler.GetPlayerLane()[i]);
                    }
                }
            }

            battleHandler.UseItem();
            itemUsed = true;
            isAttacked = true;
            UpdateCostAndAmount();
            OnCombatPanel();
        }
        else if (playerAction == PlayerAction.move) //call Move
        {
            for (int i = 0; i < 4; i++)
            {
                string allyName = battleHandler.GetPlayerLane()[i].name;

                if (buttonText == allyName)
                {
                    //set currentTarget
                    battleHandler.SetCurrentTarget(battleHandler.GetPlayerLane()[i]);
                }
            }

            battleHandler.Move();
            isMoved = true;

            OnCombatPanel();
        }
    }
    #endregion

    #region Skill Select Button
    public void OnSkillSelectEvent(Button button)
    {
        playerAction = PlayerAction.useSkill;

        //read Skill
        string buttonText = button.transform.GetChild(0).GetComponentInChildren<TMPro.TextMeshProUGUI>().text;
        Skill[] skills = battleHandler.GetSelectedCharacter().GetSkills();

        for (int i = 0; i < 4; i++)
        {
            if (skills[i] == null)
                continue; //skip if null
            if (buttonText == skills[i].Name)
            {
                battleHandler.SelectedSkill = skills[i];
            }
        }

        Skill selectedSkill = battleHandler.SelectedSkill;

        switch (selectedSkill.Target)
        {
            case Skill.TargetType.enemy:
                UpdateUIState(new TargetEnemyPanel(this));
                if (selectedSkill.Name == "Magic Missile")
                {
                    isAttacked = true;
                    itemUsed = true;
                }
                break;
            case Skill.TargetType.ally:
                UpdateUIState(new TargetPlayerPanel(this));
                break;
            case Skill.TargetType.self:
                //set currentTarget
                battleHandler.SetCurrentTarget(battleHandler.GetSelectedCharacter());
                battleHandler.UseSkill();
                skillUsed = true;
                OnCombatPanel();
                break;
        }

        UpdateCostAndAmount();
    }

    #endregion

    #region Item Select Button
    public void OnItemSelectEvent(Button button)
    {
        playerAction = PlayerAction.useItem;

        //read Skill
        string buttonText = button.transform.GetChild(0).GetComponentInChildren<TMPro.TextMeshProUGUI>().text;
        Item[] items = battleHandler.GetInventory();

        for (int i = 0; i < 4; i++)
        {
            if (items[i] == null)
                continue; //skip if null
            if (buttonText == items[i].Name)
            {
                battleHandler.SelectedItem = items[i];
            }
        }

        Item selectedItem = battleHandler.SelectedItem;

        switch (selectedItem.Target)
        {
            case Item.TargetType.enemy:
                UpdateUIState(new TargetEnemyPanel(this));
                mTargetEnemyPanel.SetActive(true);
                break;
            case Item.TargetType.ally:
                UpdateUIState(new TargetPlayerPanel(this));
                mTargetPlayerPanel.SetActive(true);
                break;
        }

        UpdateCostAndAmount();

    }

    #endregion

    public void UpdateCostAndAmount ()
    {
        int characterCost = battleHandler.GetSelectedCharacter().GetCost();
        int itemAmount = battleHandler.GetInventory()[0].Amount;

        switch(playerAction)
        {
            case PlayerAction.useSkill:
                costAndAmount.transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = "COST: " + characterCost.ToString();
                costAndAmount.SetActive(true);
                break;
            case PlayerAction.useItem:
                costAndAmount.transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = "AMOUNT: " + itemAmount.ToString();
                costAndAmount.SetActive(true);
                break;
            default:
                costAndAmount.SetActive(false);
                break;
        }
    }

    private void UpdateUIState(IUIState newState)
    {
        IUIState oldState = currentUIState;
        currentUIState = newState;

        newState.UpdateUI();
        oldState.UpdateUI();
    }

    public PlayerAction GetCurrentPlayerAction()
    {
        return playerAction;
    }

}
