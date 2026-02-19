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
        // Inventory is broken
        // machine.AddItemToInput(item, 5);
        print(machine.GetInputInventory().ToString());
        print(machine.GetOutputInventory().ToString());
    }
}
