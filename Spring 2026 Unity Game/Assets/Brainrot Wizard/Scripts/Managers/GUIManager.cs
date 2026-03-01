using System;
using UnityEngine;


public class GUIManager : MonoBehaviour
{
    [SerializeField] private MainGUI mainGUIRef;
    [SerializeField] private MachineUI machineUIRef;

    [SerializeField] private GameObject slotPrefab;

    public MainGUI MainGUIRef => mainGUIRef;
    public MachineUI MachineUIRef => machineUIRef;

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
        
        if(slot.item.GetIcon())
            inventorySlotUI.slotIcon.sprite = slot.item.GetIcon();
            
        if (slot.quantity > 0)
            inventorySlotUI.quantityText.text = slot.quantity.ToString();

        return newSlot;
    }
}
