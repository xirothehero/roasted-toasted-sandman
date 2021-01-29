using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class cButton_NPC_Interactions : MonoBehaviour
{
    public Text dialogueText;
    public GameObject[] interactionButtons;

    public Text[] interactionButtonsText;
    public GameObject holder;

    public sInteractableNPC curNPC;

    private void Start()
    {
        Debug.Log("Started");
        interactionButtonsText = new Text[interactionButtons.Length];
        Debug.Log(interactionButtonsText.Length);
        for (int i = 0; i < interactionButtonsText.Length; i++)
        {
            interactionButtonsText[i] = interactionButtons[i].transform.GetChild(0).GetComponent<Text>();
        }
    }

    public void SetupNextModule(int _idx)
    {
        ResetButtons();
        if (curNPC.curModule.replies.Length > 0)
        {
            curNPC.GoToNextModule(curNPC.curModule.replies[_idx]);
        }
        else
        {
            holder.SetActive(false);
            curNPC.interactionHUD.SetActive(true);
            Gamemanager.instance.mainCamera.followPlayer = true;
            Gamemanager.instance.thePlayer.allowInput = true;
            curNPC.interacting = false;
            if (curNPC.repeat)
                curNPC.curModule = curNPC.startingModule;
        }

    }
    
    public void ResetButtons()
    {
        for (int i = 0; i < interactionButtons.Length; i++)
        {
            interactionButtons[i].SetActive(false);
        }
    }

    public void EnableButtonsAndTexts(string[] _options)
    {
        if (_options.Length <= interactionButtons.Length)
        {
            for (int i = 0; i < _options.Length; i++)
            {
                Debug.Log("Enabling buttons");
                Debug.Log(interactionButtons[i].name);
                interactionButtons[i].SetActive(true);
                interactionButtonsText[i].text = _options[i];
            }
        }
        else
        {
            Debug.LogError("Too many interactions at the moment. Add more buttons in the interactionButtons array or remove some options from this module: " + curNPC.curModule.name);
        }

    }

    //public void SetText(int _amount, string[] _options)
    //{
    //    if (_amount <= interactionButtonsText.Length)
    //    {
    //        for (int i = 0; i < _amount; i++)
    //        {
                
    //        }
    //    }
    //    else
    //    {
    //        Debug.LogError("Not all texts from the buttons are in the interactionButtonText array");
    //    }
    //}
}
