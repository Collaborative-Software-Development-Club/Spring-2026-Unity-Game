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
        machine.AddItemToInput(item, 5);
        Inventory test = new Inventory(1);
        test.AddItemToInventory(item, 5);
        print(machine.GetInputInventory().GetTotalItemCount());
        print(machine.GetInputInventory().Length);
        print(test.GetTotalItemCount());
        print(test.Length);
        item.PrintData();
        //machine.GetInputInventory().GetItemAt(0).item.PrintData();
        print(machine.GetOutputInventory().GetTotalItemCount());
    }
}
