using JetBrains.Annotations;
using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using Unity.VisualScripting;
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

    protected virtual void Awake()
    {
        if (data == null)
        {
            Debug.LogWarning($"{nameof(Machine)} on '{name}' has no MachineData assigned. Creating empty input and output inventory.");
            Input = new Inventory(0);
            Output = new Inventory(0);
            BuildActionMap(); // still build a map so OnInteraction is safe
            return;
        }

        // Ensure a non-negative size
        int inputSize = Mathf.Max(0, data.inputCount);
        Input = new Inventory(inputSize);

        int outputSize = Mathf.Max(0, data.outputCount);
        Output = new Inventory(outputSize);

        BuildActionMap();
    }

    protected virtual void BuildActionMap()
    {
        // store parameterless for most types, store Action<int,int> for Swap
        _actionMap = new Dictionary<machineType, Delegate>
        {
            { machineType.Add, new Action(HandleAdd) },
            { machineType.Remove, new Action(HandleRemove) },
            { machineType.Duplicate, new Action(HandleDuplicate) },
            { machineType.Swap, new Action<int,int>(HandleSwap) }, // maps directly to HandleSwap(indexA,indexB)
            { machineType.Fission, new Action(HandleFission) },
            { machineType.Fusion, new Action(HandleFusion) }
        };
    }

    // ---------------- Reflection helpers ----------------
    // Note: Item has a GetType() method that returns ItemType in this codebase.
    // To obtain the System.Type of the Item instance we must call the Object implementation:
    //   ((object)item).GetType()
    // Use that System.Type when reflecting into the Item instance to read/write its protected 'data' field.

    private static FieldInfo GetItemDataField(Type systemItemType)
    {
        return systemItemType.GetField(
            "data",
            BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.FlattenHierarchy
        );
    }

    private static ItemData GetItemData(Item item)
    {
        if (item == null) return null;
        var systemType = ((object)item).GetType();
        var field = GetItemDataField(systemType);
        return field?.GetValue(item) as ItemData;
    }

    private static bool SetItemData(Item item, ItemData newData)
    {
        if (item == null) return false;
        var systemType = ((object)item).GetType();
        var field = GetItemDataField(systemType);
        if (field == null) return false;
        field.SetValue(item, newData);
        return true;
    }

    // ---------------- Handlers (adapters that call MachineFunctionality) ----------------

    protected virtual void HandleAdd()
    {
        var removed = Input.RemoveFromSlot(0);
        var item = removed.item;
        if (item == null) { Debug.Log($"{name}: no item in input slot 0."); return; }

        var itemData = GetItemData(item) as BrainrotData;
        if (itemData == null)
        {
            Debug.LogWarning($"{name}: item is not a Brainrot.");
            Input.AddItemToInventory(item, removed.quantity);
            return;
        }

        var result = MachineFunctionality.AddRandomAttribute(itemData, failChance);
        if (result == null)
        {
            Debug.LogWarning($"{name}: AddRandomAttribute returned null.");
            Input.AddItemToInventory(item, removed.quantity);
            return;
        }

        if (!SetItemData(item, result))
        {
            Debug.LogWarning($"{name}: failed to set ItemData on item.");
            Input.AddItemToInventory(item, removed.quantity);
            return;
        }

        if (Output.AddItemToInventory(item, removed.quantity) < 0)
        {
            Debug.LogWarning($"{name}: output full, returning item to input.");
            Input.AddItemToInventory(item, removed.quantity);
        }
    }
    protected virtual void HandleRemove()
    {
        var removed = Input.RemoveFromSlot(0);
        var item = removed.item;
        if (item == null) { Debug.LogWarning($"{name}: no item in input slot 0."); return; }

        var itemData = GetItemData(item) as BrainrotData;
        if (itemData == null) { Debug.LogWarning($"{name}: item is not a Brainrot."); Input.AddItemToInventory(item, removed.quantity); return; }

        var result = MachineFunctionality.RemoveRandomAttribute(itemData);
        if (result == null) { Debug.LogWarning($"{name}: RemoveRandomAttribute returned null."); Input.AddItemToInventory(item, removed.quantity); return; }

        if (!SetItemData(item, result)) { Debug.LogWarning($"{name}: failed to set ItemData on item."); Input.AddItemToInventory(item, removed.quantity); return; }

        if (Output.AddItemToInventory(item, removed.quantity) < 0) { Debug.LogWarning($"{name}: output full, returning item to input."); Input.AddItemToInventory(item, removed.quantity); }
    }

    protected virtual void HandleDuplicate()
    {
        var removed = Input.RemoveFromSlot(0);
        var item = removed.item;
        if (item == null) { Debug.LogWarning($"{name}: no item in input slot 0."); return; }

        var itemData = GetItemData(item) as BrainrotData;
        if (itemData == null) { Debug.LogWarning($"{name}: item is not a Brainrot."); Input.AddItemToInventory(item, removed.quantity); return; }

        int attrCount = itemData.attributes?.Count ?? 0;
        if (attrCount == 0) { Debug.LogWarning($"{name}: no attributes to duplicate."); Input.AddItemToInventory(item, removed.quantity); return; }

        int attributeIndex = UnityEngine.Random.Range(0, attrCount);
        var result = MachineFunctionality.DuplicateRandomAttribute(itemData, attributeIndex, failChance);
        if (result == null) { Debug.LogWarning($"{name}: duplicateRandomAttribute returned null."); Input.AddItemToInventory(item, removed.quantity); return; }

        if (!SetItemData(item, result)) { Debug.LogWarning($"{name}: failed to set ItemData on item."); Input.AddItemToInventory(item, removed.quantity); return; }

        if (Output.AddItemToInventory(item, removed.quantity) < 0) { Debug.LogWarning($"{name}: output full, returning item to input."); Input.AddItemToInventory(item, removed.quantity); }
    }

    // DoSwap - concrete swap using caller-provided attribute indexes (no randomness here).
    public void HandleSwap(int attributeIndexA, int attributeIndexB)
    {
        var removedA = Input.RemoveFromSlot(0);
        var removedB = Input.RemoveFromSlot(1);
        var itemA = removedA.item;
        var itemB = removedB.item;

        if (itemA == null || itemB == null)
        {
            Debug.Log($"{name}: need two items in input slots 0 and 1 to swap.");
            if (itemA != null) Input.AddItemToInventory(itemA, removedA.quantity);
            if (itemB != null) Input.AddItemToInventory(itemB, removedB.quantity);
            return;
        }

        var dataA = GetItemData(itemA) as BrainrotData;
        var dataB = GetItemData(itemB) as BrainrotData;
        if (dataA == null || dataB == null)
        {
            Debug.LogWarning($"{name}: both items must be Brainrot.");
            Input.AddItemToInventory(itemA, removedA.quantity);
            Input.AddItemToInventory(itemB, removedB.quantity);
            return;
        }

        // Validate provided indexes against attribute lists
        if (attributeIndexA < 0 || attributeIndexA >= (dataA.attributes?.Count ?? 0) ||
            attributeIndexB < 0 || attributeIndexB >= (dataB.attributes?.Count ?? 0))
        {
            Debug.LogWarning($"{name}: provided attribute indexes out of range.");
            Input.AddItemToInventory(itemA, removedA.quantity);
            Input.AddItemToInventory(itemB, removedB.quantity);
            return;
        }

        var results = MachineFunctionality.SwapRandomAttribute(dataA, dataB, attributeIndexA, attributeIndexB, failChance);
        if (results == null || results.Length != 2)
        {
            Debug.LogWarning($"{name}: SwapRandomAttribute failed.");
            Input.AddItemToInventory(itemA, removedA.quantity);
            Input.AddItemToInventory(itemB, removedB.quantity);
            return;
        }

        if (!SetItemData(itemA, results[0]) || !SetItemData(itemB, results[1]))
        {
            Debug.LogWarning($"{name}: failed to set swapped data on items.");
            Input.AddItemToInventory(itemA, removedA.quantity);
            Input.AddItemToInventory(itemB, removedB.quantity);
            return;
        }

        int addedA = Output.AddItemToInventory(itemA, removedA.quantity);
        int addedB = Output.AddItemToInventory(itemB, removedB.quantity);
        if (addedA < 0 || addedB < 0)
        {
            Debug.LogWarning($"{name}: output full or partial failure, returning items to input.");
            if (addedA > -1) Output.RemoveItemFromInventory(itemA, removedA.quantity);
            if (addedB > -1) Output.RemoveItemFromInventory(itemB, removedB.quantity);
            Input.AddItemToInventory(itemA, removedA.quantity);
            Input.AddItemToInventory(itemB, removedB.quantity);
        }
    }

    protected virtual void HandleFission()
    {
        var removed = Input.RemoveFromSlot(0);
        var item = removed.item;
        if (item == null) { Debug.LogWarning($"{name}: no item in input slot 0."); return; }

        var itemData = GetItemData(item) as BrainrotData;
        if (itemData == null) { Debug.LogWarning($"{name}: item is not a Brainrot."); Input.AddItemToInventory(item, removed.quantity); return; }

        var results = MachineFunctionality.FissionAttribute(itemData, failChance);
        if (results == null) { Debug.LogWarning($"{name}: FissionAttribute failed."); Input.AddItemToInventory(item, removed.quantity); return; }

        if (!SetItemData(item, results[0])) { Debug.LogWarning($"{name}: failed to set fission result on item."); Input.AddItemToInventory(item, removed.quantity); return; }

        if (Output.AddItemToInventory(item, removed.quantity) < 0) { Debug.LogWarning($"{name}: output full, returning item to input."); Input.AddItemToInventory(item, removed.quantity); return; }

        if (results.Length > 1 && results[1] != null)
        {
            var cloneGo = UnityEngine.Object.Instantiate(item.gameObject);
            var cloneItem = cloneGo.GetComponent<Item>();
            if (cloneItem == null) { Debug.LogWarning($"{name}: clone has no Item component."); UnityEngine.Object.Destroy(cloneGo); return; }

            if (!SetItemData(cloneItem, results[1])) { Debug.LogWarning($"{name}: failed to set data on clone."); UnityEngine.Object.Destroy(cloneGo); return; }

            if (Output.AddItemToInventory(cloneItem, 1) < 0) { Debug.LogWarning($"{name}: output full, destroying clone."); UnityEngine.Object.Destroy(cloneGo); }
        }
    }

    protected virtual void HandleFusion()
    {
        var removedA = Input.RemoveFromSlot(0);
        var removedB = Input.RemoveFromSlot(1);
        var itemA = removedA.item;
        var itemB = removedB.item;

        if (itemA == null || itemB == null) { Debug.LogWarning($"{name}: need two items in input slots 0 and 1 to fuse."); if (itemA != null) Input.AddItemToInventory(itemA, removedA.quantity); if (itemB != null) Input.AddItemToInventory(itemB, removedB.quantity); return; }

        var dataA = GetItemData(itemA) as BrainrotData;
        var dataB = GetItemData(itemB) as BrainrotData;
        if (dataA == null || dataB == null) { Debug.LogWarning($"{name}: both items must be Brainrot."); Input.AddItemToInventory(itemA, removedA.quantity); Input.AddItemToInventory(itemB, removedB.quantity); return; }

        var fused = MachineFunctionality.FusionAttribute(dataA, dataB, failChance);
        if (fused == null) { Debug.LogWarning($"{name}: FusionAttribute failed."); Input.AddItemToInventory(itemA, removedA.quantity); Input.AddItemToInventory(itemB, removedB.quantity); return; }

        if (!SetItemData(itemA, fused)) { Debug.LogWarning($"{name}: failed to set fused data on item."); Input.AddItemToInventory(itemA, removedA.quantity); Input.AddItemToInventory(itemB, removedB.quantity); return; }

        if (Output.AddItemToInventory(itemA, 1) < 0) { Debug.LogWarning($"{name}: output full, returning items to input."); Input.AddItemToInventory(itemA, removedA.quantity); Input.AddItemToInventory(itemB, removedB.quantity); return; }

        UnityEngine.Object.Destroy(itemB.gameObject);
    }

    // Confused about this because it should support more than two based off the below comment
    // Could make it use args... in the future for infinite amount of attributes
    
    // overloaded call for operations that require two integers (e.g., Swap)
    public void OnInteraction(int attributeFromBrainrot1, int attributeFromBrainrot2)
    {
        if (data == null) { Debug.LogWarning($"{name}: no MachineData assigned."); return; }

        if (_actionMap != null && _actionMap.TryGetValue(data.processType, out var del))
        {
            if (del is Action<int, int> ai) { ai(attributeFromBrainrot1, attributeFromBrainrot2); return; }
            if (del is Action a) { a(); return; } // fall back if typed delegate not present
        }

        Debug.LogWarning($"{name}: no handler configured for machine type '{data.processType}' that accepts two indexes.");
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
            .UpdateSlotDisplay(newIndex, item.GetIcon(), quantity);

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
public bool RemoveItemFromOutput(Item item, int quantity = 1)
{
    var removed = Output.RemoveItemFromInventory(item, quantity);

    if (!hasUI || removed.Count <= 0) return removed.Count > 0;
    int index = Output.GetItemAtFirstFoundIndex(item);
    if (index > -1)
        GameManager.Instance.GUIManager.MachineUIRef
            .UpdateSlotDisplay(index, item.GetIcon(), -quantity, false);

    return removed.Count > 0;
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