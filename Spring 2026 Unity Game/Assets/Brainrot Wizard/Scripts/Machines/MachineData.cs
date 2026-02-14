using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Windows;

public enum machineType
{ 
    add,
    remove,
    copy,
    swap
}

[CreateAssetMenu(fileName = "New Machine", menuName = "Game/BrainrotMixer/Machine/Base Machine")]
public class MachineData : ScriptableObject
{
    public new string name = "Unnamed";
    public machineType processType;
    public Sprite texture;
    /// <summary>
    /// Clone the provided BrainrotData and add a random attribute (or increment existing). Also removes a random quantity of existing attributes based on the total number of attributes.
    /// Returns the cloned BrainrotData with the modification. Returns null and logs a warning
    /// if the input is null.
    /// </summary>
    /// <param name="input">Source BrainrotData to clone and modify.</param>
    /// <param name="failChance">The percentage chance (0-100) that each existing attribute will lose a quantity instead of the new attribute being added. This chance is applied independently to each existing attribute.</param>
    public BrainrotData AddRandomAttribute(BrainrotData input, int failChance)
    {
        if (input == null)
        {
            Debug.LogWarning("MachineData.AddRandomAttribute called with null input.");
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
            double chanceToRemove = (1.0 / totalAttributes) * (failChance * 2);
            foreach (AttributeQuantity attribute in clone.attributes)
            {
                int removeRoll = Random.Range(0, 100);
                if (chanceToRemove < removeRoll)
                {
                    attribute.quantity = -1;
                    if (attribute.quantity <= 0)
                    {
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
    /// <summary>
    /// Removes an attribute from the provided BrainrotData. This is selected at random with equal chance for each attribute.
    /// </summary>
    /// <param name="input">Source BrainrotData to clone and modify.</param>
    /// <returns></returns>
    public BrainrotData RemoveRandomAttribute(BrainrotData input) {
        
        if (input == null)
        {
            Debug.LogWarning("MachineData.RemoveRandomAttribute called with null input.");
            return null;
        }

        // Instantiate a runtime copy so the asset itself is not changed.
        var clone = Instantiate(input);

        if (clone.attributes == null)
        {
            clone.attributes = new List<AttributeQuantity>();
        }

        // Total all attributes that currently exist on the BrainrotData.
        int totalAttributes = clone.attributes.Sum(aq => aq.quantity);

        // Calculate the chance to remove each attribute based on its quantity relative to the total.
        int chance = (100 / totalAttributes);

        // Remove the attributes.
        foreach (AttributeQuantity attribute in clone.attributes)
        {
            int removeRoll = Random.Range(0, 100);
            if (chance < removeRoll)
            {
                attribute.quantity = -1;
                if (attribute.quantity <= 0)
                {
                    clone.attributes.Remove(attribute);
                }
            }
        }

        return clone;
    }
    /// <summary>
    /// Swaps a specified attribute between two BrainrotData instances, with a chance for each swap to fail and select a
    /// different attribute instead.
    /// </summary>
    /// <remarks>The method creates runtime copies of the input BrainrotData instances to avoid modifying the
    /// original assets. For each attribute swap, there is a chance that the intended attribute will not be swapped;
    /// instead, a different attribute may be selected at random. The original input objects remain unchanged.</remarks>
    /// <param name="input1">The first BrainrotData instance to participate in the attribute swap. Cannot be null.</param>
    /// <param name="input2">The second BrainrotData instance to participate in the attribute swap. Cannot be null.</param>
    /// <param name="attributeToSwap1">The zero-based index of the attribute to swap from the first BrainrotData instance.</param>
    /// <param name="attributeToSwap2">The zero-based index of the attribute to swap from the second BrainrotData instance.</param>
    /// <param name="failChance">The percentage chance (0-100) that each attribute swap will fail and select a different attribute instead. This chance is applied independently to each swap.</param>
    /// <returns>An array containing two new BrainrotData instances with the specified attributes swapped. Returns null if either
    /// input1 or input2 is null.</returns>
    public BrainrotData[] SwapRandomAttribute(BrainrotData input1, BrainrotData input2, int attributeToSwap1, int attributeToSwap2, int failChance)
    {
        if (input1 == null)
        {
            Debug.LogWarning("MachineData.SwapRandomAttribute called with null input1.");
            return null;
        } else if (input2 == null)
        {
            Debug.LogWarning("MachineData.SwapRandomAttribute called with null input2.");
            return null;
        }

        // Instantiate a runtime copy so the asset itself is not changed.
        var clone1 = Instantiate(input1);
        // Instantiate a runtime copy so the asset itself is not changed.
        var clone2 = Instantiate(input2);

        if (clone1.attributes == null)
        {
            clone1.attributes = new List<AttributeQuantity>();
        }

        if (clone2.attributes == null)
        {
            clone2.attributes = new List<AttributeQuantity>();
        }

        // This is the percentage chance for each attribute, so since it's 2 attributes, we divide by 2.
        int chanceOfFail = failChance / 2;

        // Create clones of the attributes to swap.
        int attribute1 = attributeToSwap1;
        int attribute2 = attributeToSwap2;

        // Total all attributes that currently exist on the BrainrotData.
        int totalAttributes1 = clone1.attributes.Sum(aq => aq.quantity);
        int totalAttributes2 = clone2.attributes.Sum(aq => aq.quantity);

        // For each clone, roll the chance to fail, and if it fails, randomize to a different attribute.
        if (Random.Range(0, 100) < chanceOfFail)
        {
            int randAttribute1 = attribute1;
            do
            {
                randAttribute1 = Random.Range(0, totalAttributes1);
            } while (randAttribute1 == attribute1);
            attribute1 = randAttribute1;
        }

        // For each clone, roll the chance to fail, and if it fails, randomize to a different attribute.
        if (Random.Range(0, 100) < chanceOfFail)
        {
            int randAttribute2 = attribute2;
            do
            {
                randAttribute2 = Random.Range(0, totalAttributes1);
            } while (randAttribute2 == attribute1);
            attribute2 = randAttribute2;
        }

        // Swap the attributes.

        var temp = clone1.attributes[attribute1];
        clone1.attributes[attribute1] = clone2.attributes[attribute2];
        clone2.attributes[attribute2] = temp;

        return new BrainrotData[] { clone1, clone2 };
    }
    /// <summary>
    /// Creates a copy of the specified BrainrotData and attempts to duplicate or decrement a selected attribute based
    /// on a random chance.
    /// </summary>
    /// <remarks>The original input object is not modified. If the duplication fails and the attribute's
    /// quantity is 1, the attribute is removed from the list in the returned copy.</remarks>
    /// <param name="input">The BrainrotData instance to copy and modify. Cannot be null.</param>
    /// <param name="attributeToDuplicate">The zero-based index of the attribute in the attributes list to attempt to duplicate or decrement.</param>
    /// <param name="failChance">The percentage chance (0 to 100) that the duplication will fail, resulting in the attribute being decremented
    /// instead of incremented.</param>
    /// <returns>A new BrainrotData instance with the selected attribute incremented or decremented based on the random chance;
    /// or null if input is null.</returns>
    public static BrainrotData duplicateRandomAttribute(BrainrotData input, int attributeToDuplicate, int failChance)
    {
        if (input == null)
        {
            Debug.LogWarning("MachineData.duplicateRandomAttribute called with null input.");
            return null;
        }

        // Instantiate a runtime copy so the asset itself is not changed.
        var clone = Instantiate(input);

        if (clone.attributes == null)
        {
            clone.attributes = new List<AttributeQuantity>();
        }

        // Pull the attribute to duplicate.
        var existing = clone.attributes.Find(aq => aq.attribute == clone.attributes[attributeToDuplicate].attribute);

        // Roll a random chance to see if duplicate fails.
        if (Random.Range(0, 100) < failChance)
        {
            // If it fails, remove 1 from the attribute instead of adding.
            if (existing.quantity > 1)
            {
                existing.quantity -= 1;
                clone.attributes[attributeToDuplicate] = existing;
            }
            else
            {
                clone.attributes.Remove(existing);
            }
        }
        else
        {
            // If it succeeds, add 1 to the attribute.
            existing.quantity += 1;
            clone.attributes[attributeToDuplicate] = existing;
        }
        return clone;
    }
}