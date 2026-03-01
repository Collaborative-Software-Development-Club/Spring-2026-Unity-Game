using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MachineUI : MonoBehaviour
{
    public GameObject machineBackgroundUI;
    public GameObject machineSlotPrefab;
    
    public ScrollRect inputScrollView;
    public ScrollRect outputScrollView;
    
    private List<InventorySlotUI> _inputSlots = new();
    private List<InventorySlotUI> _outputSlots = new();
    
    public Button actionButtonPrefab;
    public Transform buttonContainer;

    public Machine CurrentMachine { get; private set; }

    public void Open(Machine machine)
    {
        if (CurrentMachine != null) return;
        
        CurrentMachine = machine;
        
        Inventory inputs = machine.GetInputInventory();
        Inventory outputs = machine.GetOutputInventory();

        foreach (InventorySlot slot in inputs.slots)
        {
            GameObject newSlot = GameManager.Instance.GUIManager.CreateSlot(slot);
            newSlot.transform.SetParent(inputScrollView.content, false);
            _inputSlots.Add(newSlot.GetComponent<InventorySlotUI>());
        }

        foreach (InventorySlot slot in outputs.slots)
        {
            GameObject newSlot = GameManager.Instance.GUIManager.CreateSlot(slot);
            newSlot.transform.SetParent(outputScrollView.content, false);
            _outputSlots.Add(newSlot.GetComponent<InventorySlotUI>());
        }
        
        gameObject.SetActive(true);
    }
    
    
    public void Open(Machine machine, Action action)
    {
        if (CurrentMachine != null) return;
        
        Open(machine);
        
        Button newButton = Instantiate(actionButtonPrefab, buttonContainer);

        newButton.onClick.AddListener(() => action?.Invoke());
    }

    public void Close()
    {
        foreach (Transform child in inputScrollView.content)
        {
            Destroy(child.gameObject);
        }

        foreach (Transform child in outputScrollView.content)
        {
            Destroy(child.gameObject);
        }
        
        CurrentMachine = null;
        gameObject.SetActive(false);
    }

    /// <summary>
    /// Updates the given slot with the new icon and quantity.
    /// </summary>
    /// <param name="index">The slot to change.</param>
    /// <param name="icon">The new icon to set the slot ui to.</param>
    /// <param name="quantity">The new quantity to set the slot ui to.</param>
    /// <param name="isInput">Whether the slot is a input or output.</param>
    public void UpdateSlotDisplay(int index, Sprite icon, int quantity, bool isInput = true)
    {
        if (isInput)
        {
            _inputSlots[index].UpdateIcon(icon);
            _inputSlots[index].SetQuantityText(quantity);
        }
        else
        {
            _outputSlots[index].UpdateIcon(icon);
            _outputSlots[index].SetQuantityText(quantity);
        }
    }
}
