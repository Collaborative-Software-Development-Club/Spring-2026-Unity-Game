using System.Collections.Generic;
using UnityEngine;

public class Lootbox : Item
{
    public Lootbox(Loottable itemData)
    {
        data = itemData;
    }

    public Lootbox(Loottable itemData, string itemName) : this(itemData)
    {
        Name = itemName;
    }

    private Loottable LootTable => data as Loottable;

    public override void Consume()
    {
        base.Consume();

        LoottableEntry entry = Roll();
        if (entry != null)
        {
            Item runtimeItem = ItemFactory.CreateItem(entry.ItemData);

            if (runtimeItem != null)
            {
                GameManager.Instance.playerInventory.AddItemToInventory(runtimeItem, entry.ItemQuantity);
                GameManager.Instance.playerInventory.RemoveItemFromInventory(this);
            }
        }
        
        GameManager.Instance.GUIManager.lootboxUIRef.onLootboxOpened?.Invoke(this);
        GameManager.Instance.GUIManager.TooltipUIRef.Hide();
    }

    public LoottableEntry Roll()
    {
        if (LootTable != null)
            return LootTable.GetRandomEntry();

        Debug.LogError("Lootbox has no Loottable assigned");
        return null;
    }

    public List<string> GetDropSummary()
    {
        if (LootTable != null)
            return LootTable.GetDataAsString();

        return new List<string> { "No loot table assigned" };
    }

    public override string GetDataAsString()
    {
        string dataAsString = base.GetDataAsString();
        dataAsString += "\nLoot Table: " + (LootTable != null ? LootTable.name : "None");
        return dataAsString;
    }

    public override Item Clone()
    {
        var clone = (Lootbox)MemberwiseClone();
        return clone;
    }
}
