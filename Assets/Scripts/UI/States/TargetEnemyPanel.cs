using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TargetEnemyPanel : IUIState
{
    UIHandler uIHandler;
    public TargetEnemyPanel(UIHandler uIHandler)
    {
        this.uIHandler = uIHandler;
    }

    public void UpdateUI()
    {
        if (uIHandler.currentUIState is not TargetEnemyPanel)
        {
            uIHandler.mTargetEnemyPanel.SetActive(false);
        }
        else
        {
            uIHandler.mTargetEnemyPanel.SetActive(true);
            DrawEnemyTargetting();
        }
    }

    private void DrawEnemyTargetting()
    {
        Character[] playerLane = uIHandler.battleHandler.GetPlayerLane();
        Character selectedCharacter = uIHandler.battleHandler.GetSelectedCharacter();
        Character[] enemyLane = uIHandler.battleHandler.GetEnemyLane();

        //Draw enemy Selection name
        for (int i = 0; i < 4; i++)
        {
            if (enemyLane[i] == null)
            {
                //change text
                uIHandler.transform.GetChild(i).GetComponentInChildren<TMPro.TextMeshProUGUI>().text = "None";
                //set interactable to false
                uIHandler.transform.GetChild(i).GetComponent<Button>().interactable = false;
            }
            else
            {
                //Marked Enemy name
                if (enemyLane[i].GetComponent<Character>().IsMark && selectedCharacter.name == "Ranger")
                    uIHandler.mTargetEnemyPanel.transform.GetChild(i).GetComponentInChildren<TMPro.TextMeshProUGUI>().color = Color.green;
                else
                    uIHandler.mTargetEnemyPanel.transform.GetChild(i).GetComponentInChildren<TMPro.TextMeshProUGUI>().color = Color.black;

                //change text
                uIHandler.mTargetEnemyPanel.transform.GetChild(i).GetComponentInChildren<TMPro.TextMeshProUGUI>().text = enemyLane[i].name;
                //set interactable to true
                uIHandler.mTargetEnemyPanel.transform.GetChild(i).GetComponent<Button>().interactable = true;
            }

        }

        //Fighter
        if (selectedCharacter.name == "Fighter")
        {
            for (int i = 0; i < 4; i++)
            {
                //set interactable to false if not in the same lane
                if (System.Array.IndexOf(playerLane, selectedCharacter) == i)
                    uIHandler.mTargetEnemyPanel.transform.GetChild(i).GetComponent<Button>().interactable = true;
                else
                    uIHandler.mTargetEnemyPanel.transform.GetChild(i).GetComponent<Button>().interactable = false;
            }
        }

        //check if enemy is dead
        for (int i = 0; i < 4; i++)
        {
            //set button interactable to false if enemy is dead
            if (enemyLane[i].GetComponent<Character>().GetState() == Character.State.dead)
                uIHandler.mTargetEnemyPanel.transform.GetChild(i).GetComponent<Button>().interactable = false;
        }
    }
}
