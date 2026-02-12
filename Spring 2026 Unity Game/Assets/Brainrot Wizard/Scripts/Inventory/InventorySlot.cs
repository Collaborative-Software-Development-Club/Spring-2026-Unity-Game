[System.Serializable]
public class InventorySlot
{
    public Item? item;
    public int quantity;

    public InventorySlot(Item item, int quantity)
    {
        this.item = item;
        this.quantity = quantity;
    }

    public InventorySlot() {
        item = null;
        quantity = 0;
    }

    public ItemType Type() {
        return item.GetType();
    }

    public bool Add(InventorySlot invSlot) {
        return Add(invSlot.item, invSlot.quantity);
    }

    public bool Add(Item item, int quantity) {
        if (this.quantity == quantity) {
            this.quantity += quantity;
            return true;
        }
        if (this.quantity == 0 || item is null) {
            this.item = item;
            this.quantity += quantity;
            return true;
        }
        return false;
    }
}
