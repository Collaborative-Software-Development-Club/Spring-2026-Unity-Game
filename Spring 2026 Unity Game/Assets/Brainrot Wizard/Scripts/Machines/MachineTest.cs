using System;
using UnityEngine;

public class MachineTest : MonoBehaviour
{
    public Machine machine;
    public Item item;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        print(machine.GetMachineType());
        print(machine.AddItemToInput(item, 5));
        print(machine.GetInputInventory().GetTotalItemCount());
        print(machine.GetInputInventory().Length);
        //machine.OnInteraction();
        machine.GetInputInventory().GetItemAt(0).item.PrintData();
        print(machine.GetOutputInventory().GetTotalItemCount());
    }
}
