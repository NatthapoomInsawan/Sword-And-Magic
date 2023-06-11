using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TargetPlayerPanel : IUIState
{
    UIHandler uIHandler;

    public TargetPlayerPanel(UIHandler uIHandler)
    {
        this.uIHandler = uIHandler;
    }

    public void UpdateUI()
    {

        if (uIHandler.currentUIState is not TargetPlayerPanel)
        {
            uIHandler.mTargetPlayerPanel.SetActive(false);
        }
        else
        {
            uIHandler.mTargetPlayerPanel.SetActive(true);
            DrawPlayerTargetting();
        }
    }

    private void DrawPlayerTargetting()
    {
        Character[] playerLane = uIHandler.battleHandler.GetPlayerLane();

        //Draw player selection name
        for (int i = 0; i < 4; i++)
        {
            if (playerLane[i] == null)
            {
                //change text
                uIHandler.mTargetPlayerPanel.transform.GetChild(i).GetComponentInChildren<TMPro.TextMeshProUGUI>().text = "None";
                //set interactable to false
                uIHandler.mTargetPlayerPanel.transform.GetChild(i).GetComponent<Button>().interactable = false;
            }
            else
            {
                //change text
                uIHandler.mTargetPlayerPanel.transform.GetChild(i).GetComponentInChildren<TMPro.TextMeshProUGUI>().text = playerLane[i].name;
                //set interactable to true
                uIHandler.mTargetPlayerPanel.transform.GetChild(i).GetComponent<Button>().interactable = true;
            }

        }
    }
}
