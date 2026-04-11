using System;
using UnityEngine;

public class ContractTerminalInteract : MonoBehaviour, IInteractable
{
    private void Start()
    {
        GameManager.Instance.ContractManager.AddContract(Contract.GenerateRandomContract());
        GameManager.Instance.ContractManager.AddContract(Contract.GenerateRandomContract());
        GameManager.Instance.ContractManager.AddContract(Contract.GenerateRandomContract());
        GameManager.Instance.ContractManager.AddContract(Contract.GenerateRandomContract());
    }

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
