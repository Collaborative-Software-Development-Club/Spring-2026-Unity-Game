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
        machine.ProcessFunction(new int[2]);
        machine.GetOutputFromSlot(0).PrintData();
        print(machine.GetOutputInventory().GetTotalItemCount());
    }
}
