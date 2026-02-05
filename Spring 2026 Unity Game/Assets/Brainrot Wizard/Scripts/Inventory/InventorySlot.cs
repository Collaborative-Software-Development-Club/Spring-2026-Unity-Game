[System.Serializable]
public class InventorySlot
{
    public ItemClass item;
    public int quantity;

    public InventorySlot(ItemClass item, int quantity)
    {
        this.item = item;
        this.quantity = quantity;
    }
}
