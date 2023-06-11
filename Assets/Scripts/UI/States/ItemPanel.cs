using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemPanel : IUIState
{
    UIHandler uIHandler;

    public ItemPanel(UIHandler uIHandler)
    {
        this.uIHandler = uIHandler;
    }

    public void UpdateUI()
    {

        if (uIHandler.currentUIState is not ItemPanel)
        {
            uIHandler.mItemPanel.SetActive(false);
        }
        else
        {
            uIHandler.mItemPanel.SetActive(true);
            DrawItem();

        }
    }

    private void DrawItem()
    {

        Item[] inventory = uIHandler.battleHandler.GetInventory();

        for (int i = 0; i < 4; i++)
        {
            if (inventory[i] == null)
            {
                //set text to none
                uIHandler.mItemPanel.transform.GetChild(i).GetComponentInChildren<TMPro.TextMeshProUGUI>().text = "None";
                //set button interactable
                uIHandler.mItemPanel.transform.GetChild(i).GetComponent<Button>().interactable = false;
            }
            else
            {
                //set text to none
                uIHandler.mItemPanel.transform.GetChild(i).GetComponentInChildren<TMPro.TextMeshProUGUI>().text = inventory[i].Name;
                //set button interactable
                uIHandler.mItemPanel.transform.GetChild(i).GetComponent<Button>().interactable = true;
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
                uIHandler.mItemPanel.transform.GetChild(i).GetComponent<Button>().interactable = false;
            }
        }

    }
}
