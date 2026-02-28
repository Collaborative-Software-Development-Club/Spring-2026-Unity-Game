using System;
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

    public bool AddItemToInventory(Item item, int quantity=1) {
        for (int i = 0; i < slots.Length; i++) {
            if (slots[i].IsTypeAs(item)) {
                slots[i].quantity += quantity;
                return true;
            }
        }
        for (int i = 0; i < slots.Length; i++) {
            if (slots[i].item == null) {
                slots[i] = new InventorySlot(item, quantity);
                return true;
            }
        }
        return false;
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

    // remove all of a slot up to a quantity and return it
    
    /*
    public InventorySlot RemoveFromSlot(int slot, int quantity = 1)
    {
        Assert.IsTrue(quantity >= 0);
        
        InventorySlot removedItems = new InventorySlot(slots[slot].item, 0);
        
        if (slots[slot].quantity < quantity)
        {
            removedItems.quantity = slots[slot].quantity;
            slots[slot].quantity = 0;
        }
        else
        {
            removedItems.quantity = quantity;   
            slots[slot].quantity -= quantity;
        }

        return removedItems;
    }
    */
    
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
    public InventorySlot RemoveItemFromInventory(Item item, int quantity=1) {
        InventorySlot returning = new InventorySlot();
        for (int i = slots.Length-1; i >= 0; i--) {
            if (slots[i].IsTypeAs(item)) {
                returning.Add(RemoveFromSlot(i, quantity - returning.quantity));
                if (returning.quantity == quantity) {
                    return returning;
                }
            }
        }

        if (returning.quantity == 0) {
            return new InventorySlot();
        }

        return returning;
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
