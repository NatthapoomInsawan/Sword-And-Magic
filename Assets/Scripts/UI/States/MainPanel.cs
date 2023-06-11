using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainPanel : IUIState
{
    UIHandler uIHandler;

    public MainPanel(UIHandler uIHandler)
    {
        this.uIHandler = uIHandler;
    }

    public void UpdateUI()
    {

        if (uIHandler.currentUIState is not MainPanel)
        {
            uIHandler.mCombatPanel.SetActive(false);
        }
        else
        {
            uIHandler.mCombatPanel.SetActive(true);
            SetButtonInteraction();
        }
    }

    private void SetButtonInteraction()
    {
        if (uIHandler.battleHandler.GetSelectedCharacter().CompareTag("Enemy"))
        {
            uIHandler.mCombatPanel.transform.GetChild(0).GetComponent<Button>().interactable = false;
            uIHandler.mCombatPanel.transform.GetChild(1).GetComponent<Button>().interactable = false;
            uIHandler.mCombatPanel.transform.GetChild(2).GetComponent<Button>().interactable = false;
            uIHandler.mCombatPanel.transform.GetChild(3).GetComponent<Button>().interactable = false;
            uIHandler.gameObject.transform.GetChild(5).GetComponent<Button>().interactable = false;
        }
        else //Player Turn
        {
            if (uIHandler.itemUsed)
                uIHandler.mCombatPanel.transform.GetChild(2).GetComponent<Button>().interactable = false;
            else
                uIHandler.mCombatPanel.transform.GetChild(2).GetComponent<Button>().interactable = true;

            if (uIHandler.isAttacked)
            {
                uIHandler.mCombatPanel.transform.GetChild(0).GetComponent<Button>().interactable = false;
                //can't use Item if attacked
                uIHandler.mCombatPanel.transform.GetChild(2).GetComponent<Button>().interactable = false;
            }
            else
                uIHandler.mCombatPanel.transform.GetChild(0).GetComponent<Button>().interactable = true;

            if (uIHandler.skillUsed)
                uIHandler.mCombatPanel.transform.GetChild(1).GetComponent<Button>().interactable = false;
            else
                uIHandler.mCombatPanel.transform.GetChild(1).GetComponent<Button>().interactable = true;

            if (uIHandler.isMoved)
                uIHandler.mCombatPanel.transform.GetChild(3).GetComponent<Button>().interactable = false;
            else
                uIHandler.mCombatPanel.transform.GetChild(3).GetComponent<Button>().interactable = true;

            uIHandler.gameObject.transform.GetChild(5).GetComponent<Button>().interactable = true;
        }
    }

}
