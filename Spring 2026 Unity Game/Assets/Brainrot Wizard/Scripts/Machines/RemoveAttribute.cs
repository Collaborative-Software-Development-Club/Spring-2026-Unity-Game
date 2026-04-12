using System.Linq;
using UnityEngine;

public class RemoveAttribute : MachineFunctionality
{
    /// <summary>
    /// Handles removing a random attribute to the input's brainrot
    /// </summary>
    /// <param name="thisMachine">The machine to grab the inventory from.</param>
    /// <param name="indexes">In this case, unnecessary.</param>
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

        thisMachine.AddItemToOutput(RemoveRandomAttribute(inputBrainrot));
        thisMachine.RemoveItemFromInput(inputBrainrot);
        return true;
    }
    /// <summary>
    /// Removes an attribute from the provided Brainrot. This is selected at random with equal chance for each attribute.
    /// </summary>
    /// <param name="input">Source Brainrot to clone and modify.</param>
    /// <returns></returns>
    protected static Brainrot RemoveRandomAttribute(Brainrot input)
    {

        if (input == null)
        {
            Debug.LogWarning("MachineData.RemoveRandomAttribute called with null input.");
            return null;
        }

        // Instantiate a runtime copy so the asset itself is not changed.
        var clone = input.Clone() as Brainrot;

        if (clone.GetAttributes() == null)
        {
            Debug.LogWarning("Brainrot is missing Attributes!");
            return null;
        }

        // Total all attributes that currently exist on the Brainrot.
        int totalAttributes = clone.GetAttributes().Sum(aq => aq.quantity);

        // Calculate the chance to remove each attribute based on its quantity relative to the total.
        int chance = (100 / totalAttributes);

        // Remove the attributes.
        foreach (AttributeQuantity attribute in clone.GetAttributes())
        {
            int removeRoll = UnityEngine.Random.Range(0, 100);
            if (chance < removeRoll)
            {
                attribute.quantity = -1;
                if (attribute.quantity <= 0)
                {
                    clone.RemoveAttribute(attribute);
                }
            }
        }

        return clone;
    }
}