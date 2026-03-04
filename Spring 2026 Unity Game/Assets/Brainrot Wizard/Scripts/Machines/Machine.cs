using System;
using System.Collections.Generic;
using UnityEngine;

public class Machine : MonoBehaviour, IInteractable
{
    public bool hasUI;
    
    [Tooltip("Chance for an operation to fail, expressed as a percentage (0-100)")]
    public int failChance = 50;

    [SerializeField] protected MachineData data;

    protected Inventory Input;

    protected Inventory Output;

    private Dictionary<machineType, Delegate> _actionMap;

    public MachineFunctionality MachineFunctionality;

    protected virtual void Awake()
    {
        if (data == null)
        {
            Debug.LogWarning($"{nameof(Machine)} on '{name}' has no MachineData assigned. Creating empty input and output inventory.");
            Input = new Inventory(0);
            Output = new Inventory(0);
        }

        // Ensure a non-negative size
        int inputSize = Mathf.Max(0, data.inputCount);
        Input = new Inventory(inputSize);

        int outputSize = Mathf.Max(0, data.outputCount);
        Output = new Inventory(outputSize);
    }


    // Function that will process inputs into outputs when called. indexes for specific attributes can be carried through for swap and duplicate.
    public void ProcessFunction(int[] indexes)
    {
        if (data == null) { Debug.LogWarning($"{name}: no MachineData assigned."); return; }
        MachineFunctionality.Handler(this, indexes);
    }

    // Function for retrieving the type this machine is.
    public machineType GetMachineType() 
    {
        return data.processType;
    }

    // Function for retrieving the input inventory of this machine.
    public Inventory GetInputInventory()
    {
        return Input;
    }
    // Function for retrieving the output inventory of this machine.
    public Inventory GetOutputInventory()
    {
        return Output;
    }
    // Function for retrieving items found within Input.
    public Item GetInputFromSlot(int slot) { 
        return Input.GetItemAt(slot).item;
    }

    // Function for retrieving items found within Output.
    public Item GetOutputFromSlot(int slot)
    {
        return Output.GetItemAt(slot).item;
    }

    /// <summary>
    /// Function for adding an item to the input inventory of this machine.
    /// Returns true if successful, false if the input inventory is full.
    /// </summary>
    /// <param name="item">The item to add.</param>
    /// <param name="quantity">The quantity of the item to add. Defaults to 1.</param>
    /// <returns>
    /// True if successful; otherwise false.
    /// </returns>
    public bool AddItemToInput(Item item, int quantity = 1)
{
    int newIndex = Input.AddItemToInventory(item, quantity);

    if (hasUI && newIndex > -1)
        GameManager.Instance.GUIManager.MachineUIRef
            .UpdateSlotDisplay(newIndex, item.GetIcon(), Input.GetItemAt(newIndex).quantity);

    return newIndex > -1;
}

/// <summary>
/// Function for adding an item to the output inventory of this machine.
/// Returns true if successful, false if the output inventory is full.
/// </summary>
/// <param name="item">The item to add.</param>
/// <param name="quantity">The quantity of the item to add. Defaults to 1.</param>
/// <returns>
/// True if successful; otherwise false.
/// </returns>
public bool AddItemToOutput(Item item, int quantity = 1)
{
    int newIndex = Output.AddItemToInventory(item, quantity);

    if (hasUI && newIndex > -1)
        GameManager.Instance.GUIManager.MachineUIRef
            .UpdateSlotDisplay(newIndex, item.GetIcon(), quantity, false);

    return newIndex > -1;
}

/// <summary>
/// Function for removing an item from the input inventory of this machine.
/// Returns true if successful, false if the input inventory is empty or does not contain the item.
/// </summary>
/// <param name="item">The item to remove.</param>
/// <param name="quantity">The quantity of the item to remove. Defaults to 1.</param>
/// <returns>
/// True if successful; otherwise false.
/// </returns>
public bool RemoveItemFromInput(Item item, int quantity = 1)
{
    var removed = Input.RemoveItemFromInventory(item, quantity);

    if (hasUI && removed.Count > 0)
    {
        int index = Input.GetItemAtFirstFoundIndex(item);
        if (index > -1)
            GameManager.Instance.GUIManager.MachineUIRef
                .UpdateSlotDisplay(index, item.GetIcon(), -quantity);
    }

    return removed.Count > 0;
}

/// <summary>
/// Function for removing an item from the output inventory of this machine.
/// Returns true if successful, false if the output inventory is empty or does not contain the item.
/// </summary>
/// <param name="item">The item to remove.</param>
/// <param name="quantity">The quantity of the item to remove. Defaults to 1.</param>
/// <returns>
/// True if successful; otherwise false.
/// </returns>

// TODO: Need to change back to RemoveItemFromInventory, RemoveFromSlot is a very temp fix
public InventorySlot RemoveItemFromOutput(int index, int quantity = 1)
{
    var removed = Output.RemoveFromSlot(index, quantity);

    if (!hasUI) return removed;
        
    UpdateUI();

    return removed;
}

private void UpdateUI()
{
    if (!hasUI) return;

    for (int i = 0; i < Input.Length; i++)
    {
        InventorySlot slot = Input.slots[i];

        if (slot.item != null)
        {
            GameManager.Instance.GUIManager.MachineUIRef
                .UpdateSlotDisplay(i, slot.item.GetIcon(), slot.quantity);
        }
        else
        {
            GameManager.Instance.GUIManager.MachineUIRef
                .UpdateSlotDisplay(i, null, 0); 
        }
    }

    for (int i = 0; i < Output.Length; i++)
    {
        InventorySlot slot = Output.slots[i];

        if (slot.item != null)
        {
            GameManager.Instance.GUIManager.MachineUIRef
                .UpdateSlotDisplay(i, slot.item.GetIcon(), slot.quantity, false);
        }
        else
        {
            GameManager.Instance.GUIManager.MachineUIRef
                .UpdateSlotDisplay(i, null, 0, false); 
        }
    }
}

    public string InteractionPrompt => "Open machine";
    public bool Interact(Interacter interactor)
    {
        if (data == null) { Debug.LogWarning($"{name}: no MachineData assigned."); return false; }

        if (_actionMap != null && _actionMap.TryGetValue(data.processType, out var del) && del is Action a)
        {
            if (hasUI)
                GameManager.Instance.GUIManager.OpenMachineUI(this, a);
            else
                a();

            return true;
        }

        Debug.LogWarning($"{name}: no handler configured for machine type '{data.processType}'.");
        return false;
    }
}
