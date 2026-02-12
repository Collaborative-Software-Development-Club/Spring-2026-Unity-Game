using JetBrains.Annotations;
using UnityEngine;

public class ItemProcessing : MonoBehaviour
{
    public MachineData machineData;
    public Inventory input;
    public Inventory output;

    public void ProcessItems()
    {
        if (machineData == null || input == null || output == null)
        {
            Debug.LogWarning("Missing machine data or items");
            return;
        }
    }
    public Inventory getInput()
    {
        return input;
    }

    public Inventory getOutput()
    {
        return output;
    }

    public bool addToInput(ItemData inputItem, int quantity)
    {
        //return input.AddItemToInventory(inputItem, quantity);
        return true;
    }
}