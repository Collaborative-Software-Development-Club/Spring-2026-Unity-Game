[System.Serializable]
public class InventorySlot
{
    public ItemData? itemData;
    public int quantity;

    public InventorySlot(ItemData itemData, int quantity)
    {
        this.itemData = itemData;
        this.quantity = quantity;
    }

    public InventorySlot() {
        this.itemData = null;
        this.quantity = 0;
    }

    public ItemType Type() {
        return this.itemData.type;
    }
}
