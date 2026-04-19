using System;
using UnityEngine;

public class MachineInteraction : MonoBehaviour, IInteractable
{
    [SerializeField] private MachineData machineData;

    [SerializeField] private bool hasUI;
    private Machine _machine;
    public Action onMachineUsed; 

    private void Awake()
    {
        if (machineData == null) return;
        _machine = new Machine(machineData); 
        _machine.hasUI = hasUI;
    }
    public string InteractionPrompt => "Open machine " + _machine.GetName(); 
    public bool Interact(Interacter interactor)
    {
        if (_machine == null)
        {
            Debug.LogWarning($"{_machine.GetName()}: no MachineData assigned.");
            return false;
        }

        int[] args = new int[2];

        Action machineHandler = () =>
        {
            _machine.SetFunctionality(args);
        };

        machineHandler += onMachineUsed.Invoke;
        
        if (_machine.hasUI)
            GameManager.Instance.GUIManager.OpenMachineUI(_machine, machineHandler);
        else
            machineHandler.Invoke();

        return true;
    }
}
