using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SkillPanel : IUIState
{
    UIHandler uIHandler;
    public SkillPanel(UIHandler uIHandler)
    {
        this.uIHandler = uIHandler;
    }

    public void UpdateUI()
    {
        if (uIHandler.currentUIState is not SkillPanel)
        {
            uIHandler.mSkillPanel.SetActive(false);
        }
        else
        {
            uIHandler.mSkillPanel.SetActive(true);
            DrawSkill();
        }
    }

    private void DrawSkill()
    {
        Skill[] playerSkill = uIHandler.battleHandler.GetSelectedCharacter().GetSkills();
        Character selectedCharacter = uIHandler.battleHandler.GetSelectedCharacter();

        for (int i = 0; i < 4; i++)
        {
            if (playerSkill[i] == null)
            {
                //set text to none
                uIHandler.mSkillPanel.transform.GetChild(i).GetComponentInChildren<TMPro.TextMeshProUGUI>().text = "None";
                //set button interactable
                uIHandler.mSkillPanel.transform.GetChild(i).GetComponent<Button>().interactable = false;
            }
            else
            {
                //set text to none
                uIHandler.mSkillPanel.transform.GetChild(i).GetComponentInChildren<TMPro.TextMeshProUGUI>().text = playerSkill[i].Name;
                //set button interactable
                uIHandler.mSkillPanel.transform.GetChild(i).GetComponent<Button>().interactable = true;
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
                uIHandler.mSkillPanel.transform.GetChild(i).GetComponent<Button>().interactable = false;
            }
        }

        //Check Once Per Fight
        for (int i = 0; i < 4; i++)
        {
            if (playerSkill[i] == null)
                continue; //skip if null
            if (playerSkill[i].OncePerFight == true && playerSkill[i].IsUsed == true)
            {
                uIHandler.mSkillPanel.transform.GetChild(i).GetComponent<Button>().interactable = false;
            }
        }

        //Magic missile
        for (int i = 0; i < 4; i++)
        {
            if (playerSkill[i] == null)
                continue; //skip if null
            if (playerSkill[i].Name == "Magic Missile" && uIHandler.isAttacked)
            {
                uIHandler.mSkillPanel.transform.GetChild(i).GetComponent<Button>().interactable = false;
            }
        }

    }
}
