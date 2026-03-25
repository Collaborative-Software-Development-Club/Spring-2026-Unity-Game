using System;
using UnityEngine;


public class GUIManager : MonoBehaviour
{
    [SerializeField] private MainGUI mainGUIRef;
    [SerializeField] private MachineUI machineUIRef;
    public LootboxUI lootboxUIRef;
    [SerializeField] private RentGUI rentGUIRef;
    [SerializeField] private TooltipUI tooltipUIRef;

    [SerializeField] private GameObject slotPrefab;

    public MainGUI MainGUIRef => mainGUIRef;
    public MachineUI MachineUIRef => machineUIRef;
    public TooltipUI TooltipUIRef => tooltipUIRef;

    private void Start()
    {
        if (MainGUIRef == null)
            Debug.LogWarning("Main GUI is not inputted!");
        if (MachineUIRef == null)
            Debug.LogWarning("Machine UI is not inputted!");
    }
    
    public void OpenMachineUI(Machine machine)
    {
        MachineUIRef.Open(machine);
    }
    
    
    public void OpenMachineUI(Machine machine, Action action)
    {
        MachineUIRef.Open(machine, action);
    }

    public void CloseMachineUI()
    {
        MachineUIRef.Close();
    }

    public void ShowMainGUI()
    {
        MainGUIRef.Show();
    }

    public void HideMainGUI()
    {
        MainGUIRef.Hide();
    }

    public void ShowLootboxUI(Lootbox lootbox)
    {
        lootboxUIRef.OpenLootboxUI(lootbox);
    }

    public void HideLootboxUI()
    {
        lootboxUIRef.CloseLootboxUI();
    }

    public void ShowRentGUI(bool passedRentCycle)
    {
        rentGUIRef.OpenUI(passedRentCycle);
    }

    public void HideRentGUI()
    {
        rentGUIRef.CloseUI();
    }

    /// <summary>
    /// Creates a new slot game object to be used in ui
    /// </summary>
    /// <param name="slot">The slot to use to fill out the slot prefab</param>
    /// <returns></returns>
    public GameObject CreateSlot(InventorySlot slot)
    {
        GameObject newSlot = Instantiate(slotPrefab);
        var inventorySlotUI = newSlot.GetComponent<InventorySlotUI>();

        if (!slot.item) return newSlot;
        inventorySlotUI.SetItem(slot.item);
        
        if(slot.item.GetIcon())
            inventorySlotUI.UpdateIcon(slot.item.GetIcon());

        if (slot.quantity > 0)
            inventorySlotUI.SetQuantityText(slot.quantity);

        return newSlot;
    }
    /// <summary>
    /// Creates a new slot game object to be used in ui
    /// </summary>
    /// <param name="slot">The slot to use to fill out the slot prefab</param>
    /// <returns></returns>
    public GameObject CreateSlot(AttributeQuantity slot)
    {
        GameObject newSlot = Instantiate(slotPrefab);
        var inventorySlotUI = newSlot.GetComponent<InventorySlotUI>();

        if (slot.attribute == Attribute.None) return newSlot;

        inventorySlotUI.UpdateName(StringUtils.PlaceSeparators(slot.attribute.ToString()));

        if (slot.quantity > 0)
            inventorySlotUI.SetQuantityText(slot.quantity);

        return newSlot;
    }
}
