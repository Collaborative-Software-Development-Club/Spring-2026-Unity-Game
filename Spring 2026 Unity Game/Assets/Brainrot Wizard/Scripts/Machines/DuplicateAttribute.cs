using UnityEngine;

public class DuplicateAttribute : MachineFunctionality
{
    /// <summary>
    /// Handles duplicating a specific attribute to the input's brainrot
    /// </summary>
    /// <param name="thisMachine">The machine to grab the inventory from.</param>
    /// <param name="indexes">First in list is the index of the attribute to duplicate</param>
    /// <returns>If the function is successful</returns>
    public override bool Handler(Machine thisMachine, int[] indexes)
    {
        var inputInv = thisMachine.GetInputFromSlot(0);
        if (inputInv is null)
        {
            Debug.LogWarning("Machine has no input item in its first slot!");
            return false;
        }
        else if (inputInv.GetItemType() != ItemType.Brainrot)
        {
            Debug.LogWarning("Machine's input is not a brainrot!");
            return false;
        }
        else if (thisMachine.GetOutputFromSlot(0) != null)
        {
            Debug.LogWarning("Machine's output is not empty!");
            return false;
        }

            var inputBrainrot = inputInv as Brainrot;

        if (inputBrainrot.GetAttributes() == null)
        {
            Debug.LogWarning("Input brainrot has null attributes!");
            return false;
        }
        else if (inputBrainrot.GetAttributeCount() == 0) {
            Debug.LogWarning("Input brainrot has no attributes to remove!");
            return false;
        }
        else if (indexes[0] > inputBrainrot.GetAttributeCount() || indexes[0] < 0)
        {
            Debug.LogWarning("provided attribute index is out of range!");
            return false;
        }

        thisMachine.AddItemToOutput(DuplicateRandomAttribute(inputBrainrot, indexes[0], thisMachine.failChance));
        thisMachine.RemoveItemFromInput(inputBrainrot);
        return true;
    }
    /// <summary>
    /// Creates a copy of the specified Brainrot and attempts to duplicate or decrement a selected attribute based
    /// on a random chance.
    /// </summary>
    /// <remarks>The original input object is not modified. If the duplication fails and the attribute's
    /// quantity is 1, the attribute is removed from the list in the returned copy.</remarks>
    /// <param name="input">The Brainrot instance to copy and modify. Cannot be null.</param>
    /// <param name="attributeToDuplicate">The zero-based index of the attribute in the attributes list to attempt to duplicate or decrement.</param>
    /// <param name="failChance">The percentage chance (0 to 100) that the duplication will fail, resulting in the attribute being decremented
    /// instead of incremented.</param>
    /// <returns>A new Brainrot instance with the selected attribute incremented or decremented based on the random chance;
    /// or null if input is null.</returns>
    protected static Brainrot DuplicateRandomAttribute(Brainrot input, int attributeToDuplicate, int failChance)
    {
        if (input == null)
        {
            Debug.LogWarning("MachineData.duplicateRandomAttribute called with null input.");
            return null;
        }

        // Instantiate a runtime copy so the asset itself is not changed.
        var clone = MonoBehaviour.Instantiate(input);

        if (clone.GetAttributes() == null)
        {
            Debug.LogWarning("Brainrot is missing Attributes!");
            return null;
        }

        // Pull the attribute to duplicate.
        var existing = clone.GetAttributes()[attributeToDuplicate];

        // Roll a random chance to see if duplicate fails.
        if (UnityEngine.Random.Range(0, 100) < failChance)
        {
            // If it fails, remove 1 from the attribute instead of adding.
            if (existing.quantity > 1)
            {
                clone.RemoveAttribute(existing);
                existing.quantity -= 1;
                clone.AddAttribute(existing);
            }
            else
            {
                clone.RemoveAttribute(existing);
            }
        }
        else
        {
            // If it succeeds, add 1 to the attribute.
            clone.RemoveAttribute(existing);
            existing.quantity += 1;
            clone.AddAttribute(existing);
        }
        return clone;
    }
}