using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Utility MonoBehaviour that can take a BrainrotData instance, clone it and add (or increment)
/// a random attribute on the clone. The original ScriptableObject asset is not modified.
/// </summary>
public class MachineAdd : MonoBehaviour
{
    /// <summary>
    /// Clone the provided BrainrotData and add a random attribute (or increment existing).
    /// Returns the cloned BrainrotData with the modification. Returns null and logs a warning
    /// if the input is null.
    /// </summary>
    /// <param name="input">Source BrainrotData to clone and modify.</param>
    public BrainrotData AddRandomAttribute(BrainrotData input)
    {
        if (input == null)
        {
            Debug.LogWarning("MachineAdd.AddRandomAttribute called with null input.");
            return null;
        }

        // Instantiate a runtime copy so the asset itself is not changed.
        var clone = Instantiate(input);

        if (clone.attributes == null)
        {
            clone.attributes = new List<AttributeQuantity>();
        }

        // Get all possible Attribute enum values defined in the project.
        var values = (Attribute[])System.Enum.GetValues(typeof(Attribute));
        if (values == null || values.Length == 0)
        {
            Debug.LogWarning("No attributes available to add.");
            return clone;
        }

        // Pick one at random.
        var chosen = values[Random.Range(0, values.Length)];

        // Randomize the number of attributes to add (1-3, weighted more towards 1).
        int quantity = 1;
        int roll = Random.Range(0, 100);
        if (roll >= 90)
        {
            quantity = 3;
        }
        else if (roll >= 50)
        {
            quantity = 2;
        }

        
        

        // Total all attributes that currently exist, divide 1 by that, and then randomize a chance to possibly remove a quantity of that attribute.
        int totalAttributes = clone.attributes.Sum(aq => aq.quantity);
        if (totalAttributes > 0)
        {
            double chanceToRemove = 1.0 / totalAttributes * 100;
            foreach (AttributeQuantity attribute in clone.attributes) {
                int removeRoll = Random.Range(0, 100);
                if (chanceToRemove < removeRoll) {
                    attribute.quantity = -1;
                    if (attribute.quantity <= 0) {
                        clone.attributes.Remove(attribute);
                    }
                }
            }
        }

        // Find existing attribute entry and increment if present, otherwise add new.
        var existing = clone.attributes.Find(aq => aq.attribute == chosen);
        if (existing != null)
        {
            existing.quantity += quantity;
        }
        else
        {
            var newAQ = new AttributeQuantity
            {
                attribute = chosen,
                quantity = Mathf.Max(1, quantity)
            };
            clone.attributes.Add(newAQ);
        }

        return clone;
    }
}