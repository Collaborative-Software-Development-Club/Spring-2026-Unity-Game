using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class Inventory
{
    public InventorySlot[] slots = new InventorySlot[0];
    public Inventory(int size) {
        slots = new InventorySlot[size];
        for (int i = 0; i < size; i++) {
            slots[i] = new InventorySlot();
        }
    }

    public int AddItemToInventory(Item item, int quantity=1) {
        for (int i = 0; i < slots.Length; i++) {
            if (slots[i].IsTypeAs(item)) {
                slots[i].quantity += quantity;
                return i;
            }
        }
        for (int i = 0; i < slots.Length; i++) {
            if (slots[i].item == null) {
                slots[i] = new InventorySlot(item, quantity);
                return i;
            }
        }
        return -1;
    }

    public InventorySlot AddToSlot(int slot, Item item, int quantity=1) {
        InventorySlot replace = slots[slot];
        if (slots[slot] == null) {
            slots[slot] = new InventorySlot(item, quantity);
            return replace;
        }
        if (slots[slot].IsTypeAs(item)) {
            slots[slot].quantity += quantity;
        }
        slots[slot] = new InventorySlot(item, quantity);
        return replace;
    }
    
    public InventorySlot RemoveFromSlot(int slot, int quantity=1) {
        InventorySlot returning = new InventorySlot(); // null slot

        //slots[slot].quantity = Math.Clamp(slots[slot].quantity - quantity, 0, slots.Length);
        if (quantity <= 0) {
            Debug.Log("You asked for a negative or zero amount of something at invslot " + slot + "!");
            return returning;
        }

        if (slots[slot].quantity > quantity) {
            slots[slot].quantity -= quantity;
            returning.Add(slots[slot].item, quantity);
            return returning;
        }
        if (slots[slot].quantity > 0) { // full removal of the invslot
            returning.Add(slots[slot].item, slots[slot].quantity);
            slots[slot] = new InventorySlot(); // nullify slot
            return returning;
        }
        return returning;
    }

    //parse inventory and remove all of a specific type of item, up to a quantity, and return them
    public List<int> RemoveItemFromInventory(Item item, int quantity = 1)
    {
        List<int> modifiedSlots = new();
        int removed = 0;

        for (int i = slots.Length - 1; i >= 0; i--)
        {
            if (slots[i].IsTypeAs(item))
            {
                InventorySlot removedSlot = RemoveFromSlot(i, quantity - removed);

                if (removedSlot.quantity > 0)
                {
                    modifiedSlots.Add(i);
                    removed += removedSlot.quantity;
                }

                if (removed >= quantity)
                    break;
            }
        }

        return modifiedSlots;
    }
    // Return the total number of items within the inventory.
    public int GetTotalItemCount() {
        int count = 0;
        for (int i = 0; i < slots.Length; i++) {
            count += slots[i].quantity;
        }
        return count;
    }

    public int Length {
        get {
            return slots.Length;
        }
    }

    public InventorySlot GetItemAt(int slot) {
        if (slot > slots.Length) return null;
        return slots[slot];
    }
}
