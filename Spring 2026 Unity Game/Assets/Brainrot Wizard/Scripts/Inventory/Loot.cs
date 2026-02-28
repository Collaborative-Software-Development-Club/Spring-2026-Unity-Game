using UnityEngine;
using System;
using System.Collections.Generic;

public class Loot
{
    public Dictionary<InventorySlot, int> loot;
    public InventorySlot[] items;

    public Loot(Inventory items, int[] amounts) {
        loot = new Dictionary<InventorySlot, int>();
        this.items = items.slots;
        for (int i = 0; i < items.slots.Length; i++) {
            loot.Add(items.slots[i], amounts[i]);
        }

    }

    public InventorySlot pick() {
        Dictionary<int, InventorySlot> maximums = new Dictionary<int, InventorySlot>();
        List<int> maximumsInt = new List<int>();
        int count = 0;
        for (int i = 0; i < items.Length; i++) {
            count += loot[items[i]];
            maximums[count] = items[i];
            maximumsInt.Add(count);
        }
        System.Random random = new System.Random();
        int check = random.Next(1, maximumsInt[-1]+1);
        int returnInt = 0; // the index of the maximum for which this falls into
        for (int i = 0; i < maximumsInt.Count; i++) {
            if (check <= maximumsInt[i]) returnInt = i;
            if (check > maximumsInt[i]) break;
        }
        return maximums[maximumsInt[returnInt]];
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
