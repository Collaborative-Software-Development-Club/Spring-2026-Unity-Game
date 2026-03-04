using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "New Loottable", menuName = "Game/BrainrotMixer/Loottable")]
public class Loottable : ItemData 
{
    [SerializeField]
    private List<LoottableEntry> loottable = new();
    public int TotalWeight
    {
        get
        {
            return loottable == null ? 0 : loottable.Sum(entry => entry.RollAmount);
        }
    }
    public LoottableEntry GetRandomEntry()
    {
        if (loottable == null || loottable.Count == 0)
        {
            Debug.LogWarning("Loot table is empty!");
            return null;
        }

        if (TotalWeight <= 0)
        {
            Debug.LogWarning("Total weight is zero!");
            return null;
        }

        int randomValue = Random.Range(0, TotalWeight);
        int cumulative = 0;

        foreach (LoottableEntry entry in loottable)
        {
            cumulative += entry.RollAmount;
            if (randomValue < cumulative)
                return entry;
        }

        return null;
    }

    public List<string> GetDataAsString()
    {
        List<string> data = new();

        if (loottable == null || loottable.Count == 0)
            return data;

        int totalWeight = TotalWeight;

        if (totalWeight <= 0)
            return data;

        data.AddRange(from entry in loottable let chance = (float)entry.RollAmount / totalWeight select $"{chance:P2} | {entry.ItemData.name} x{entry.ItemQuantity}");

        return data;
    }
}
