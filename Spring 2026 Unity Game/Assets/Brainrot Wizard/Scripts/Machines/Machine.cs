using System;
using UnityEngine;

public class Machine : Item
{
    public bool hasUI;
    
    [Tooltip("Chance for an operation to fail, expressed as a percentage (0-100)")]
    public int failChance = 50;

    private MachineData machineData => data as MachineData;

    protected Inventory Input;

    protected Inventory Output;

    private MachineFunctionality _machineFunctionality;
    

    public void SetFunctionality(int[] args)
    {
        _machineFunctionality.Handler(this, args);
    }
    
    // Function for retrieving the type this machine is.
    public machineType GetMachineType() 
    {
        return machineData.processType;
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
    int index = -1;
    for (int i = 0; i < Input.Length && index == -1; i++)
    {
        var slotItem = Input.GetItemAt(i);
        if (slotItem != null && slotItem.item == item) 
            index = i;
    }

    if (index == -1)
    {
        Debug.LogWarning($"Item {item.GetName()} not found in input inventory!");
        return false;
    }

    InventorySlot removed = Input.RemoveFromSlot(index, quantity);

    if (hasUI && removed != null && removed.quantity > 0)
    {
        GameManager.Instance.GUIManager.MachineUIRef
            .UpdateSlotDisplay(index, removed.item?.GetIcon(), removed.quantity > 0 ? removed.quantity * -1 : 0);
    }

    return removed != null && removed.quantity > 0;
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

    public Machine(MachineData itemData)
    {
        data = itemData;
        
        int inputSize = Mathf.Max(0, machineData.inputCount);
        Input = new Inventory(inputSize);

        int outputSize = Mathf.Max(0, machineData.outputCount);
        Output = new Inventory(outputSize);

        switch (machineData.processType)
        {
            case machineType.None:
                Debug.Log("No machine type!");
                break;
            case machineType.Add:
                _machineFunctionality = new AddAttribute();
                break;
            case machineType.Remove:
                _machineFunctionality = new RemoveAttribute();
                break;
            case machineType.Duplicate:
                _machineFunctionality = new DuplicateAttribute();
                break;
            case machineType.Swap:
                _machineFunctionality = new SwapAttribute();
                break;
            case machineType.Fission:
                _machineFunctionality = new FissionAttribute();
                break;
            case machineType.Fusion:
                _machineFunctionality = new FusionAttribute();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
    public Machine(MachineData itemData, string itemName) : this(itemData)
    {
        Name = itemName;
    }
}
