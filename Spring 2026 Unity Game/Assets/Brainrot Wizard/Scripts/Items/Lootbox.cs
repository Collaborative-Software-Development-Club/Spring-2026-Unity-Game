using System.Collections.Generic;
using UnityEngine;

public class Lootbox : Item
{
    private void Awake()
    {
        if (data != null)
            Initialize(data);
    }

    public override void Initialize(ItemData itemData)
    {
        if (itemData is not Loottable lootboxData)
        {
            Debug.LogError("Lootbox requires Loottable data");
            return;
        }

        data = lootboxData;
    }

    public override void Initialize(ItemData itemData, string itemName)
    {
        Initialize(itemData);
        Name = itemName;
    }

    private Loottable LootTable => data as Loottable;

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
