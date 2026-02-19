using JetBrains.Annotations;
using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;

public class Machine : MonoBehaviour
{
    [Tooltip("Chance for an operation to fail, expressed as a percentage (0-100)")]
    public int failChance = 50;

    [SerializeField] protected MachineData data;

    [SerializeField] protected Inventory inventory;

    protected Inventory input;

    protected Inventory output;

    private Dictionary<machineType, Delegate> _actionMap;

    protected virtual void Awake()
    {
        if (data == null)
        {
            Debug.LogWarning($"{nameof(Machine)} on '{name}' has no MachineData assigned. Creating empty input and output inventory.");
            input = new Inventory(0);
            output = new Inventory(0);
            BuildActionMap(); // still build a map so OnInteraction is safe
            return;
        }

        // Ensure a non-negative size
        int inputSize = Mathf.Max(0, data.inputCount);
        input = new Inventory(inputSize);

        int outputSize = Mathf.Max(0, data.outputCount);
        output = new Inventory(outputSize);

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
        var removed = input.RemoveFromSlot(0);
        var item = removed.item;
        if (item == null) { Debug.Log($"{name}: no item in input slot 0."); return; }

        var itemData = GetItemData(item) as BrainrotData;
        if (itemData == null)
        {
            Debug.LogWarning($"{name}: item is not a Brainrot.");
            input.AddItemToInventory(item, removed.quantity);
            return;
        }

        var result = MachineFunctionality.AddRandomAttribute(itemData, failChance);
        if (result == null)
        {
            Debug.Log($"{name}: AddRandomAttribute returned null.");
            input.AddItemToInventory(item, removed.quantity);
            return;
        }

        if (!SetItemData(item, result))
        {
            Debug.LogWarning($"{name}: failed to set ItemData on item.");
            input.AddItemToInventory(item, removed.quantity);
            return;
        }

        if (!output.AddItemToInventory(item, removed.quantity))
        {
            Debug.LogWarning($"{name}: output full, returning item to input.");
            input.AddItemToInventory(item, removed.quantity);
        }
    }
    protected virtual void HandleRemove()
    {
        var removed = input.RemoveFromSlot(0);
        var item = removed.item;
        if (item == null) { Debug.Log($"{name}: no item in input slot 0."); return; }

        var itemData = GetItemData(item) as BrainrotData;
        if (itemData == null) { Debug.LogWarning($"{name}: item is not a Brainrot."); input.AddItemToInventory(item, removed.quantity); return; }

        var result = MachineFunctionality.RemoveRandomAttribute(itemData);
        if (result == null) { Debug.Log($"{name}: RemoveRandomAttribute returned null."); input.AddItemToInventory(item, removed.quantity); return; }

        if (!SetItemData(item, result)) { Debug.LogWarning($"{name}: failed to set ItemData on item."); input.AddItemToInventory(item, removed.quantity); return; }

        if (!output.AddItemToInventory(item, removed.quantity)) { Debug.LogWarning($"{name}: output full, returning item to input."); input.AddItemToInventory(item, removed.quantity); }
    }

    protected virtual void HandleDuplicate()
    {
        var removed = input.RemoveFromSlot(0);
        var item = removed.item;
        if (item == null) { Debug.Log($"{name}: no item in input slot 0."); return; }

        var itemData = GetItemData(item) as BrainrotData;
        if (itemData == null) { Debug.LogWarning($"{name}: item is not a Brainrot."); input.AddItemToInventory(item, removed.quantity); return; }

        int attrCount = itemData.attributes?.Count ?? 0;
        if (attrCount == 0) { Debug.Log($"{name}: no attributes to duplicate."); input.AddItemToInventory(item, removed.quantity); return; }

        int attributeIndex = UnityEngine.Random.Range(0, attrCount);
        var result = MachineFunctionality.DuplicateRandomAttribute(itemData, attributeIndex, failChance);
        if (result == null) { Debug.Log($"{name}: duplicateRandomAttribute returned null."); input.AddItemToInventory(item, removed.quantity); return; }

        if (!SetItemData(item, result)) { Debug.LogWarning($"{name}: failed to set ItemData on item."); input.AddItemToInventory(item, removed.quantity); return; }

        if (!output.AddItemToInventory(item, removed.quantity)) { Debug.LogWarning($"{name}: output full, returning item to input."); input.AddItemToInventory(item, removed.quantity); }
    }

    // DoSwap - concrete swap using caller-provided attribute indexes (no randomness here).
    public void HandleSwap(int attributeIndexA, int attributeIndexB)
    {
        var removedA = input.RemoveFromSlot(0);
        var removedB = input.RemoveFromSlot(1);
        var itemA = removedA.item;
        var itemB = removedB.item;

        if (itemA == null || itemB == null)
        {
            Debug.Log($"{name}: need two items in input slots 0 and 1 to swap.");
            if (itemA != null) input.AddItemToInventory(itemA, removedA.quantity);
            if (itemB != null) input.AddItemToInventory(itemB, removedB.quantity);
            return;
        }

        var dataA = GetItemData(itemA) as BrainrotData;
        var dataB = GetItemData(itemB) as BrainrotData;
        if (dataA == null || dataB == null)
        {
            Debug.LogWarning($"{name}: both items must be Brainrot.");
            input.AddItemToInventory(itemA, removedA.quantity);
            input.AddItemToInventory(itemB, removedB.quantity);
            return;
        }

        // Validate provided indexes against attribute lists
        if (attributeIndexA < 0 || attributeIndexA >= (dataA.attributes?.Count ?? 0) ||
            attributeIndexB < 0 || attributeIndexB >= (dataB.attributes?.Count ?? 0))
        {
            Debug.LogWarning($"{name}: provided attribute indexes out of range.");
            input.AddItemToInventory(itemA, removedA.quantity);
            input.AddItemToInventory(itemB, removedB.quantity);
            return;
        }

        var results = MachineFunctionality.SwapRandomAttribute(dataA, dataB, attributeIndexA, attributeIndexB, failChance);
        if (results == null || results.Length != 2)
        {
            Debug.LogWarning($"{name}: SwapRandomAttribute failed.");
            input.AddItemToInventory(itemA, removedA.quantity);
            input.AddItemToInventory(itemB, removedB.quantity);
            return;
        }

        if (!SetItemData(itemA, results[0]) || !SetItemData(itemB, results[1]))
        {
            Debug.LogWarning($"{name}: failed to set swapped data on items.");
            input.AddItemToInventory(itemA, removedA.quantity);
            input.AddItemToInventory(itemB, removedB.quantity);
            return;
        }

        bool addedA = output.AddItemToInventory(itemA, removedA.quantity);
        bool addedB = output.AddItemToInventory(itemB, removedB.quantity);
        if (!addedA || !addedB)
        {
            Debug.LogWarning($"{name}: output full or partial failure, returning items to input.");
            if (addedA) output.RemoveItemFromInventory(itemA, removedA.quantity);
            if (addedB) output.RemoveItemFromInventory(itemB, removedB.quantity);
            input.AddItemToInventory(itemA, removedA.quantity);
            input.AddItemToInventory(itemB, removedB.quantity);
        }
    }

    protected virtual void HandleFission()
    {
        var removed = input.RemoveFromSlot(0);
        var item = removed.item;
        if (item == null) { Debug.Log($"{name}: no item in input slot 0."); return; }

        var itemData = GetItemData(item) as BrainrotData;
        if (itemData == null) { Debug.LogWarning($"{name}: item is not a Brainrot."); input.AddItemToInventory(item, removed.quantity); return; }

        var results = MachineFunctionality.FissionAttribute(itemData, failChance);
        if (results == null) { Debug.LogWarning($"{name}: FissionAttribute failed."); input.AddItemToInventory(item, removed.quantity); return; }

        if (!SetItemData(item, results[0])) { Debug.LogWarning($"{name}: failed to set fission result on item."); input.AddItemToInventory(item, removed.quantity); return; }

        if (!output.AddItemToInventory(item, removed.quantity)) { Debug.LogWarning($"{name}: output full, returning item to input."); input.AddItemToInventory(item, removed.quantity); return; }

        if (results.Length > 1 && results[1] != null)
        {
            var cloneGo = UnityEngine.Object.Instantiate(item.gameObject);
            var cloneItem = cloneGo.GetComponent<Item>();
            if (cloneItem == null) { Debug.LogWarning($"{name}: clone has no Item component."); UnityEngine.Object.Destroy(cloneGo); return; }

            if (!SetItemData(cloneItem, results[1])) { Debug.LogWarning($"{name}: failed to set data on clone."); UnityEngine.Object.Destroy(cloneGo); return; }

            if (!output.AddItemToInventory(cloneItem, 1)) { Debug.LogWarning($"{name}: output full, destroying clone."); UnityEngine.Object.Destroy(cloneGo); }
        }
    }

    protected virtual void HandleFusion()
    {
        var removedA = input.RemoveFromSlot(0);
        var removedB = input.RemoveFromSlot(1);
        var itemA = removedA.item;
        var itemB = removedB.item;

        if (itemA == null || itemB == null) { Debug.Log($"{name}: need two items in input slots 0 and 1 to fuse."); if (itemA != null) input.AddItemToInventory(itemA, removedA.quantity); if (itemB != null) input.AddItemToInventory(itemB, removedB.quantity); return; }

        var dataA = GetItemData(itemA) as BrainrotData;
        var dataB = GetItemData(itemB) as BrainrotData;
        if (dataA == null || dataB == null) { Debug.LogWarning($"{name}: both items must be Brainrot."); input.AddItemToInventory(itemA, removedA.quantity); input.AddItemToInventory(itemB, removedB.quantity); return; }

        var fused = MachineFunctionality.FusionAttribute(dataA, dataB, failChance);
        if (fused == null) { Debug.LogWarning($"{name}: FusionAttribute failed."); input.AddItemToInventory(itemA, removedA.quantity); input.AddItemToInventory(itemB, removedB.quantity); return; }

        if (!SetItemData(itemA, fused)) { Debug.LogWarning($"{name}: failed to set fused data on item."); input.AddItemToInventory(itemA, removedA.quantity); input.AddItemToInventory(itemB, removedB.quantity); return; }

        if (!output.AddItemToInventory(itemA, 1)) { Debug.LogWarning($"{name}: output full, returning items to input."); input.AddItemToInventory(itemA, removedA.quantity); input.AddItemToInventory(itemB, removedB.quantity); return; }

        UnityEngine.Object.Destroy(itemB.gameObject);
    }

    // Call this to run the configured action for this machine.
    public void OnInteraction()
    {
        if (data == null) { Debug.LogWarning($"{name}: no MachineData assigned."); return; }

        if (_actionMap != null && _actionMap.TryGetValue(data.processType, out var del))
        {
            if (del is Action a) { a(); return; }
            // If this entry expects indexes, instruct caller to use the overload.
            if (del is Action<int, int>) { Debug.LogWarning($"{name}: this machine requires attribute indexes. Call OnInteraction(indexA,indexB)."); return; }
        }

        Debug.LogWarning($"{name}: no handler configured for machine type '{data.processType}'.");
    }

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
        return input;
    }
    // Function for retrieving the output inventory of this machine.
    public Inventory GetOutputInventory()
    {
        return output;
    }
    // Function for adding an item to the input inventory of this machine. Returns true if successful, false if the input inventory is full.
    public bool AddItemToInput(Item item, int quantity=1)
    {
        return input.AddItemToInventory(item, quantity);
    }
    // Function for adding an item to the output inventory of this machine. Returns true if successful, false if the output inventory is full.
    public bool AddItemToOutput(Item item, int quantity=1)
    {
        return output.AddItemToInventory(item, quantity);
    }
    // Function for removing an item from the input inventory of this machine. Returns true if successful, false if the input inventory is empty or does not contain the item.
    public bool RemoveItemFromInput(Item item, int quantity=1)
    {
        var removed = input.RemoveItemFromInventory(item, quantity);
        return removed.quantity > 0;
    }
    // Function for removing an item from the output inventory of this machine. Returns true if successful, false if the output inventory is empty or does not contain the item.
    public bool RemoveItemFromOutput(Item item, int quantity=1)
    {
        var removed = output.RemoveItemFromInventory(item, quantity);
        return removed.quantity > 0;
    }
}