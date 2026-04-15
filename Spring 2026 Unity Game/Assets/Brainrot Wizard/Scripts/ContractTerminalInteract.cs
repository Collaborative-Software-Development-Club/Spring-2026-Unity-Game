using System;
using UnityEngine;

public class ContractTerminalInteract : MonoBehaviour, IInteractable
{
    public string InteractionPrompt => "Open contract terminal"; 
    public bool Interact(Interacter interactor)
    {
        if(GameManager.Instance.GUIManager.ContractTerminalUIRef.IsVisible())
            GameManager.Instance.GUIManager.HideContractTerminalUI();
        else
            GameManager.Instance.GUIManager.ShowContractTerminalUI();

        return true;
    }
}
