using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class MachineFunctionality
{
    /// <summary>
    /// Clone the provided BrainrotData and add a random attribute (or increment existing). Also removes a random quantity of existing attributes based on the total number of attributes.
    /// Returns the cloned BrainrotData with the modification. Returns null and logs a warning
    /// if the input is null.
    /// </summary>
    /// <param name="input">Source BrainrotData to clone and modify.</param>
    /// <param name="failChance">The percentage chance (0-100) that each existing attribute will lose a quantity instead of the new attribute being added. This chance is applied independently to each existing attribute.</param>
    public static BrainrotData AddRandomAttribute(BrainrotData input, int failChance)
    {
        if (input == null)
        {
            Debug.LogWarning("MachineData.AddRandomAttribute called with null input.");
            return null;
        }

        // Instantiate a runtime copy so the asset itself is not changed.
        var clone = MonoBehaviour.Instantiate(input);

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
    public static BrainrotData RemoveRandomAttribute(BrainrotData input)
    {

        if (input == null)
        {
            Debug.LogWarning("MachineData.RemoveRandomAttribute called with null input.");
            return null;
        }

        // Instantiate a runtime copy so the asset itself is not changed.
        var clone = MonoBehaviour.Instantiate(input);

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
    public static BrainrotData[] SwapRandomAttribute(BrainrotData input1, BrainrotData input2, int attributeToSwap1, int attributeToSwap2, int failChance)
    {
        if (input1 == null)
        {
            Debug.LogWarning("MachineData.SwapRandomAttribute called with null input1.");
            return null;
        }
        else if (input2 == null)
        {
            Debug.LogWarning("MachineData.SwapRandomAttribute called with null input2.");
            return null;
        }

        // Instantiate a runtime copy so the asset itself is not changed.
        var clone1 = MonoBehaviour.Instantiate(input1);
        // Instantiate a runtime copy so the asset itself is not changed.
        var clone2 = MonoBehaviour.Instantiate(input2);

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
    public static BrainrotData DuplicateRandomAttribute(BrainrotData input, int attributeToDuplicate, int failChance)
    {
        if (input == null)
        {
            Debug.LogWarning("MachineData.duplicateRandomAttribute called with null input.");
            return null;
        }

        // Instantiate a runtime copy so the asset itself is not changed.
        var clone = MonoBehaviour.Instantiate(input);

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
    /// <summary>
    /// Splits the attributes of a given BrainrotData instance into two separate BrainrotData objects, with a chance to
    /// remove attributes based on the specified failure rate.
    /// </summary>
    /// <remarks>The method creates a runtime copy of the input to avoid modifying the original asset.
    /// Attribute removal is randomized and influenced by the failChance parameter. The split only occurs if at least
    /// two attributes remain after removal; otherwise, only the modified original is returned. The returned array
    /// always contains two elements.</remarks>
    /// <param name="input">The BrainrotData instance whose attributes are to be split. Cannot be null.</param>
    /// <param name="failChance">The percentage chance (0–100) that an attribute will be removed during the splitting process. Higher values
    /// increase the likelihood of attribute removal.</param>
    /// <returns>An array containing two BrainrotData objects: the modified original and a new instance with half of the
    /// remaining attributes. If there are fewer than two attributes after removal, the second element will be null.
    /// Returns null if input is null.</returns>

    public static BrainrotData[] FissionAttribute(BrainrotData input, int failChance)
    {
        if (input == null)
        {
            Debug.LogWarning("MachineData.fissionAttribute called with null input.");
            return null;
        }

        // Instantiate a runtime copy so the asset itself is not changed.
        var clone = MonoBehaviour.Instantiate(input);

        if (clone.attributes == null)
        {
            clone.attributes = new List<AttributeQuantity>();
        }

        // Randomly remove some attributes based on the fail chance. Uses same randomization as AddRandomAttribute, with independent rolls.
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

                    // Remove one from total
                    totalAttributes -= 1;
                }
            }
        }

        // As long as there's at least 2 more attributes left, we can randomly pull half of the remaining attributes into a new BrainrotData.
        if (totalAttributes >= 2)
        {
            BrainrotData newBrainrot = new BrainrotData();
            newBrainrot.attributes = new List<AttributeQuantity>();

            int halfOfTotal = totalAttributes / 2;

            int[] attributesToTransfer = new int[halfOfTotal];

            for (int i = 0; i < attributesToTransfer.Length; i++)
            {
                int attributeSelected = Random.Range(0, totalAttributes);
                do
                {
                    attributeSelected = Random.Range(0, totalAttributes);
                } while (attributesToTransfer.Contains(attributeSelected));
                attributesToTransfer[i] = attributeSelected;
            }

            newBrainrot.category = clone.category;

            // Move the attributes selected above over.
            newBrainrot.attributes = clone.attributes.Where((aq, index) => attributesToTransfer.Contains(index)).Select(aq => new AttributeQuantity
            {
                attribute = aq.attribute,
                quantity = aq.quantity
            }).ToList();

            clone.attributes = clone.attributes.Where((aq, index) => !attributesToTransfer.Contains(index)).ToList();

            return new BrainrotData[] { clone, newBrainrot };
        }
        else
        {
            return new BrainrotData[] { clone, null };
        }
    }
    /// <summary>
    /// Combines the attributes of two BrainrotData instances to create a new fused BrainrotData with randomized
    /// attribute selection and optional attribute loss.
    /// </summary>
    /// <remarks>The fusion process randomly selects the category from one of the input instances and combines
    /// shared attributes by randomly choosing their quantities. Non-shared attributes are included based on the
    /// specified fail chance. The original input instances are not modified.</remarks>
    /// <param name="input1">The first BrainrotData instance to use as input for the fusion. Cannot be null.</param>
    /// <param name="input2">The second BrainrotData instance to use as input for the fusion. Cannot be null.</param>
    /// <param name="failChance">The percentage chance (0–100) that a non-shared attribute from either input will be omitted from the fused
    /// result. Higher values increase the likelihood of attribute loss.</param>
    /// <returns>A new BrainrotData instance containing a randomized combination of categories and attributes from the input
    /// instances, or null if either input is null.</returns>

    public static BrainrotData FusionAttribute(BrainrotData input1, BrainrotData input2, int failChance)
    {
        if (input1 == null)
        {
            Debug.LogWarning("MachineData.fusionAttribute called with null input1.");
            return null;
        }
        else if (input2 == null)
        {
            Debug.LogWarning("MachineData.fusionAttribute called with null input2.");
            return null;
        }

        // Instantiate a runtime copy so the asset itself is not changed.
        var clone1 = MonoBehaviour.Instantiate(input1);
        // Instantiate a runtime copy so the asset itself is not changed.
        var clone2 = MonoBehaviour.Instantiate(input2);

        // Create the new BrainrotData to return, which will combine attributes from both inputs at random.
        BrainrotData newBrainrot = new BrainrotData();

        // Randomize between the two categories for the new BrainrotData.
        newBrainrot.category = clone1.category;
        if (Random.Range(0, 1) == 1)
        {
            newBrainrot.category = clone2.category;
        }

        // Check if the parents share any attributes.
        foreach (AttributeQuantity aq1 in clone1.attributes)
        {
            var matchingAQ = clone2.attributes.Find(aq2 => aq2.attribute == aq1.attribute);
            if (matchingAQ != null)
            {
                // If they do, Randomize between the two quantities to keep.
                newBrainrot.attributes.Add(new AttributeQuantity
                {
                    attribute = aq1.attribute,
                    quantity = Random.Range(aq1.quantity, matchingAQ.quantity)
                });
            }
        }

        // For the rest of the attributes that aren't shared, randomize a chance to keep each based on the fail chance.
        foreach (AttributeQuantity aq1 in clone1.attributes)
        {
            if (!newBrainrot.attributes.Any(aq => aq.attribute == aq1.attribute))
            {
                if (Random.Range(0, 100) > failChance)
                {
                    newBrainrot.attributes.Add(new AttributeQuantity
                    {
                        attribute = aq1.attribute,
                        quantity = aq1.quantity
                    });
                }
            }
        }
        foreach (AttributeQuantity aq2 in clone2.attributes)
        {
            if (!newBrainrot.attributes.Any(aq => aq.attribute == aq2.attribute))
            {
                if (Random.Range(0, 100) > failChance)
                {
                    newBrainrot.attributes.Add(new AttributeQuantity
                    {
                        attribute = aq2.attribute,
                        quantity = aq2.quantity
                    });
                }
            }
        }

        return newBrainrot;
    }
}
