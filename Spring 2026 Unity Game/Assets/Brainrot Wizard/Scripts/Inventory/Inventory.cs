using UnityEngine;

public class Inventory
{
    InventorySlot[] slots = new InventorySlot[0];
    public Inventory(int size) {
        slots = new InventorySlot[size];
        for (int i = 0; i < size; i++) {
            slots[i] = new InventorySlot();
        }
    }

    public bool AddItemToInventory(ItemData item, int quantity=1) {
        for (int i = 0; i < slots.Length; i++) {
            if (slots[i].Type() == item.type) {
                slots[i].quantity += quantity;
                return true;
            }
        }
        for (int i = 0; i < slots.Length; i++) {
            if (slots[i] == null) {
                slots[i] = new InventorySlot(item, quantity);
                return true;
            }
        }
        return false;
    }

    public InventorySlot AddToSlot(int slot, ItemData item, int quantity=1) {
        InventorySlot replace = slots[slot];
        if (slots[slot] == null) {
            slots[slot] = new InventorySlot(item, quantity);
            return replace;
        }
        if (slots[slot].Type() == item.type) {
            slots[slot].quantity += quantity;
        }
        slots[slot] = new InventorySlot(item, quantity);
        return replace;
    }

    // remove all of a slot up to a quantity and return it
    public InventorySlot RemoveFromSlot(int slot, int quantity=1) {
        InventorySlot returning = new InventorySlot(); // null slot
        if (slots[slot].quantity > quantity) {
            slots[slot].quantity -= quantity;
            returning = new InventorySlot(slots[slot].itemData, quantity);
            return returning;
        }
        if (slots[slot].quantity > 0) { // full removal of the invslot
            returning = new InventorySlot(slots[slot].itemData, slots[slot].quantity);
            slots[slot] = new InventorySlot(); // nullify slot
            return returning;
        }
        return returning;
    }

    //parse inventory and remove all of a specific type of item, up to a quantity, and return them
    public InventorySlot RemoveItemFromInventory(ItemData item, int quantity=1) {
        InventorySlot returning = new InventorySlot(item, 0);
        for (int i = slots.Length-1; i >= 0; i--) {
            if (slots[i].Type() == item.type) {
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
}
