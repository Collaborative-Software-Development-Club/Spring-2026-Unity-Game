using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class MachineFunctionalityTemp 
{
    // In the future have an allowed types paramater for now it's just default to brainrot for testing
    
    public static Brainrot FuseItems(List<Brainrot> brainrots, int failChance)
    {
        if (brainrots == null || brainrots.Count < 2)
        {
            Debug.LogWarning("FuseItems requires at least two Brainrots.");
            return null;
        }

        // Pick a random category from one of the brainrots
        Category fusedCategory = brainrots[UnityEngine.Random.Range(0, brainrots.Count)].GetCategory();

        // Dictionary to accumulate attributes
        Dictionary<Attribute, int> fusedAttributes = new Dictionary<Attribute, int>();

        foreach (var brainrot in brainrots)
        {
            foreach (var attr in brainrot.GetAttributes())
            {
                // Apply fail chance to possibly skip this attribute
                if (UnityEngine.Random.Range(0, 100) < failChance)
                    continue;

                if (fusedAttributes.TryGetValue(attr.attribute, out int existingQuantity))
                {
                    // If attribute already exists, pick a random quantity between existing and new
                    int min = Math.Min(existingQuantity, attr.quantity);
                    int max = Math.Max(existingQuantity, attr.quantity);
                    fusedAttributes[attr.attribute] = UnityEngine.Random.Range(min, max + 1);
                }
                else
                {
                    fusedAttributes[attr.attribute] = attr.quantity;
                }
            }
        }

        // If no attributes survived the fail chance, pick one at random from the originals
        if (fusedAttributes.Count == 0)
        {
            var allAttributes = brainrots.SelectMany(b => b.GetAttributes()).ToList();
            if (allAttributes.Count > 0)
            {
                var randomAttr = allAttributes[UnityEngine.Random.Range(0, allAttributes.Count)];
                fusedAttributes[randomAttr.attribute] = randomAttr.quantity;
            }
        }

        // Create a new Brainrot to hold the fusion
        Brainrot fusedBrainrot = new Brainrot();

        BrainrotData fusedData = new BrainrotData
        {
            category = fusedCategory,
            attributes = fusedAttributes
                .Select(kv => new AttributeQuantity(kv.Key, kv.Value))
                .ToList()
        };

        fusedBrainrot.Initialize(fusedData, "Fused Brainrot");

        Debug.Log($"Fused {brainrots.Count} Brainrots into one with {fusedBrainrot.GetAttributes().Count} attributes and category {fusedBrainrot.GetCategory()}");

        return fusedBrainrot;
    }

    public static void AddAttributesToItem(Brainrot brainrot, List<AttributeQuantity> attributes)
    {
        
    }
}
