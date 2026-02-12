[System.Serializable]
public class InventorySlot
{
    public Item item;
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
        if (invSlot.quantity == quantity) {
            quantity += invSlot.quantity;
            return true;
        }
        return false;
    }
}
