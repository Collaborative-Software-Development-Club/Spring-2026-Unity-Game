using System;
using System.Linq;
using UnityEngine;

public class AddAttribute : MachineFunctionality
{
    /// <summary>
    /// Handles adding a random attribute to the input's brainrot
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

        if (inputBrainrot.GetAttributes() == null) {
            Debug.LogWarning("Input brainrot has null attributes!");
            return false;
        }

        thisMachine.AddItemToOutput(AddRandomAttribute(inputBrainrot, thisMachine.failChance));
        thisMachine.RemoveItemFromInput(inputBrainrot);
        return true;
    }
    /// <summary>
    /// Clone the provided Brainrot and add a random attribute (or increment existing). Also removes a random quantity of existing attributes based on the total number of attributes.
    /// Returns the cloned Brainrot with the modification. Returns null and logs a warning
    /// if the input is null.
    /// </summary>
    /// <param name="input">Source Brainrot to clone and modify.</param>
    /// <param name="failChance">The percentage chance (0-100) that each existing attribute will lose a quantity instead of the new attribute being added. This chance is applied independently to each existing attribute.</param>
    protected static Brainrot AddRandomAttribute(Brainrot input, int failChance)
    {
        if (input == null)
        {
            Debug.LogWarning("MachineData.AddRandomAttribute called with null input.");
            return null;
        }

        // Instantiate a runtime copy so the asset itself is not changed.
        var clone = input.Clone() as Brainrot;

        if (clone.GetAttributes() == null)
        {
            Debug.LogWarning("Brainrot is missing Attributes!");
            return null;
        }

        // Get all possible Attribute enum values defined in the project.
        var values = (Attribute[])Enum.GetValues(typeof(Attribute));
        if (values == null || values.Length == 0)
        {
            Debug.LogWarning("No attributes available to add.");
            return clone;
        }

        // Pick one at random.
        var chosen = values[UnityEngine.Random.Range(0, values.Length)];

        // UnityEngine.Randomize the number of attributes to add (1-3, weighted more towards 1).
        int quantity = 1;
        int roll = UnityEngine.Random.Range(0, 100);
        if (roll >= 90)
        {
            quantity = 3;
        }
        else if (roll >= 50)
        {
            quantity = 2;
        }

        // Total all attributes that currently exist, divide 1 by that, and then randomize a chance to possibly remove a quantity of that attribute.
        int totalAttributes = clone.GetAttributes().Sum(aq => aq.quantity);
        if (totalAttributes > 0)
        {
            double chanceToRemove = (1.0 / totalAttributes) * (failChance * 2);
            foreach (AttributeQuantity attribute in input.GetAttributes())
            {
                int removeRoll = UnityEngine.Random.Range(0, 100);
                if (chanceToRemove < removeRoll)
                {
                    clone.RemoveAttribute(attribute);
                    attribute.quantity -= 1;
                    if (attribute.quantity > 0)
                    {
                        clone.AddAttribute(attribute);
                    }
                }
            }
        }

        // Find existing attribute entry and increment if present, otherwise add new.
        var existing = clone.GetAttributes().Find(aq => aq.attribute == chosen);
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
            clone.AddAttribute(newAQ);
        }

        return clone;
    }
}