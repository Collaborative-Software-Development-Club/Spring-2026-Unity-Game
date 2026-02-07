using UnityEngine;

public class Inventory : MonoBehaviour
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
                slots[i].quantity++;
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
}
