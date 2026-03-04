using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FissionAttribute : MachineFunctionality
{
    /// <summary>
    /// Handles fissioning random attributes to two separate brainrots from the input's brainrot
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
        else if (thisMachine.GetOutputFromSlot(0) != null || thisMachine.GetOutputFromSlot(1) != null)
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
            Debug.LogWarning("Input brainrot has no attributes to fission from!");
            return false;
        }

        Brainrot[] machineOutput = FissionAttributes(inputBrainrot, thisMachine.failChance);

        thisMachine.AddItemToOutput(machineOutput[0]);
        if (machineOutput[1] != null) {
            thisMachine.AddItemToOutput(machineOutput[1]);
        }
        thisMachine.RemoveItemFromInput(inputBrainrot);
        return true;
    }
    /// <summary>
    /// Splits the attributes of a given Brainrot instance into two separate Brainrot objects, with a chance to
    /// remove attributes based on the specified failure rate.
    /// </summary>
    /// <remarks>The method creates a runtime copy of the input to avoid modifying the original asset.
    /// Attribute removal is randomized and influenced by the failChance parameter. The split only occurs if at least
    /// two attributes remain after removal; otherwise, only the modified original is returned. The returned array
    /// always contains two elements.</remarks>
    /// <param name="input">The Brainrot instance whose attributes are to be split. Cannot be null.</param>
    /// <param name="failChance">The percentage chance (0�100) that an attribute will be removed during the splitting process. Higher values
    /// increase the likelihood of attribute removal.</param>
    /// <returns>An array containing two Brainrot objects: the modified original and a new instance with half of the
    /// remaining attributes. If there are fewer than two attributes after removal, the second element will be null.
    /// Returns null if input is null.</returns>

    protected static Brainrot[] FissionAttributes(Brainrot input, int failChance)
    {
        if (input == null)
        {
            Debug.LogWarning("MachineData.fissionAttribute called with null input.");
            return null;
        }

        // Instantiate a runtime copy so the asset itself is not changed.
        var clone = MonoBehaviour.Instantiate(input);

        if (clone.GetAttributes() == null)
        {
            Debug.LogWarning("Brainrot is missing Attributes!");
            return null;
        }
        List<AttributeQuantity> attributesCopy = new List<AttributeQuantity>();

        foreach (AttributeQuantity thing in clone.GetAttributes())
        {
            attributesCopy.Add(thing);
        }

        // UnityEngine.Randomly remove some attributes based on the fail chance. Uses same randomization as AddRandomAttribute, with independent rolls.
        int totalAttributes = clone.GetAttributes().Sum(aq => aq.quantity);
        if (totalAttributes > 0)
        {
            double chanceToRemove = (1.0 / totalAttributes) * (failChance * 2);
            foreach (AttributeQuantity attribute in attributesCopy)
            {
                int removeRoll = UnityEngine.Random.Range(0, 100);
                if (chanceToRemove < removeRoll)
                {
                    attribute.quantity = -1;
                    if (attribute.quantity <= 0)
                    {
                        clone.RemoveAttribute(attribute);
                    }

                    // Remove one from total
                    totalAttributes -= 1;
                }
            }
        }

        // As long as there's at least 2 more attributes left, we can randomly pull half of the remaining attributes into a new Brainrot.
        if (totalAttributes >= 2)
        {
            Brainrot newBrainrot = clone.Clone() as Brainrot;
            foreach (AttributeQuantity attribute in newBrainrot.GetAttributes())
            {
                newBrainrot.RemoveAttribute(attribute);
            }

            int halfOfTotal = totalAttributes / 2;

            int[] attributesToTransfer = new int[halfOfTotal];

            for (int i = 0; i < attributesToTransfer.Length; i++)
            {
                int attributeSelected = UnityEngine.Random.Range(0, totalAttributes);
                do
                {
                    attributeSelected = UnityEngine.Random.Range(0, totalAttributes);
                } while (attributesToTransfer.Contains(attributeSelected));
                attributesToTransfer[i] = attributeSelected;
            }

            // Move the attributes selected above over.
            foreach (int index in attributesToTransfer)
            {
                newBrainrot.AddAttribute(clone.GetAttributes()[index]);
            }

            return new Brainrot[] { clone, newBrainrot };
        }
        else
        {
            return new Brainrot[] { clone, null };
        }
    }
}