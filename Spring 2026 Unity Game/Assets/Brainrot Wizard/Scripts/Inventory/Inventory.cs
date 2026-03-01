using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public struct InventoryChange
{
    public int Index;        
    public int NewQuantity;  
}
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

    /// <summary>
    /// Removes up to <paramref name="quantity"/> of a specific <paramref name="item"/> from the inventory.
    /// Returns a list of changes, each with the slot index and the new quantity after removal.
    /// </summary>
    /// <param name="item">The item to remove.</param>
    /// <param name="quantity">The maximum quantity to remove. Defaults to 1.</param>
    /// <returns>
    /// A list of InventoryChange structs containing the slot index and the updated quantity.
    /// </returns>
    public List<InventoryChange> RemoveItemFromInventory(Item item, int quantity = 1)
    {
        List<InventoryChange> changes = new();
        int remainingToRemove = quantity;

        for (int i = slots.Length - 1; i >= 0; i--)
        {
            if (remainingToRemove <= 0)
                break;

            // removing problem probably starts here
            if (!slots[i].IsTypeAs(item))
                continue;

            int available = slots[i].quantity;
            int removeAmount = Mathf.Min(available, remainingToRemove);

            if (removeAmount <= 0)
                continue;

            slots[i].quantity -= removeAmount;
            remainingToRemove -= removeAmount;

            if (slots[i].quantity <= 0)
            {
                slots[i].quantity = 0;
                slots[i].item = null;
            }

            changes.Add(new InventoryChange
            {
                Index = i,
                NewQuantity = slots[i].quantity
            });
        }

        return changes;
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

    /// <summary>
    /// Gets the first instance of the passed in item.
    /// </summary>
    /// <param name="item">The item to look for.</param>
    /// <returns>Returns the index or -1 if not found.</returns>
    public int GetItemAtFirstFoundIndex(Item item)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == item) return i;
        }
        
        return -1;
    }
}
