using System.Linq;
using UnityEngine;

public class FusionAttribute : MachineFunctionality
{
    /// <summary>
    /// Handles Fusing two input brainrots into a single outputted brainrot.
    /// </summary>
    /// <param name="thisMachine">The machine to grab the inventory from.</param>
    /// <param name="indexes">In this case, unnecessary.</param>
    /// <returns>If the function is successful</returns>
    public override bool Handler(Machine thisMachine, int[] indexes)
    {
        var inputInv1 = thisMachine.GetInputFromSlot(0);
        var inputInv2 = thisMachine.GetInputFromSlot(1);
        if (inputInv1 is null || inputInv2 is null)
        {
            Debug.LogWarning("Machine is missing one or both input items in their slots!");
            return false;
        }
        else if (inputInv1.GetItemType() != ItemType.Brainrot || inputInv2.GetItemType() != ItemType.Brainrot)
        {
            Debug.LogWarning("One of or both of machine's inputs are not brainrots!");
            return false;
        }
        else if (thisMachine.GetOutputFromSlot(0) != null)
        {
            Debug.LogWarning("Machine's output is not empty!");
            return false;
        }

        var inputBrainrot1 = inputInv1 as Brainrot;
        var inputBrainrot2 = inputInv2 as Brainrot;

        if (inputBrainrot1.GetAttributes() == null || inputBrainrot2.GetAttributes() == null)
        {
            Debug.LogWarning("One of or both of input brainrots have null attributes!");
            return false;
        }
        else if (inputBrainrot1.GetAttributeCount() == 0 || inputBrainrot2.GetAttributeCount() == 0) {
            Debug.LogWarning("One of or both of input brainrots have no attributes to remove!");
            return false;
        }

        thisMachine.AddItemToOutput(FusionAttributes(inputBrainrot1, inputBrainrot2, thisMachine.failChance));
        thisMachine.RemoveItemFromInput(inputBrainrot1);
        thisMachine.RemoveItemFromInput(inputBrainrot2);
        return true;
    }
    /// <summary>
    /// Combines the attributes of two Brainrot instances to create a new fused Brainrot with randomized
    /// attribute selection and optional attribute loss.
    /// </summary>
    /// <remarks>The fusion process randomly selects the category from one of the input instances and combines
    /// shared attributes by randomly choosing their quantities. Non-shared attributes are included based on the
    /// specified fail chance. The original input instances are not modified.</remarks>
    /// <param name="input1">The first Brainrot instance to use as input for the fusion. Cannot be null.</param>
    /// <param name="input2">The second Brainrot instance to use as input for the fusion. Cannot be null.</param>
    /// <param name="failChance">The percentage chance (0�100) that a non-shared attribute from either input will be omitted from the fused
    /// result. Higher values increase the likelihood of attribute loss.</param>
    /// <returns>A new Brainrot instance containing a randomized combination of categories and attributes from the input
    /// instances, or null if either input is null.</returns>

    public static Brainrot FusionAttributes(Brainrot input1, Brainrot input2, int failChance)
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

        // Create the new Brainrot to return, which will combine attributes from both inputs at random.
        Brainrot newBrainrot; 

        if (Random.Range(0, 2) == 1)
        {
            newBrainrot = new Brainrot(input1);
        }
        else
        {
            newBrainrot = new Brainrot(input2);
        }

        // Check if the parents share any attributes.
        foreach (AttributeQuantity aq1 in input1.GetAttributes())
        {
            var matchingAQ = input2.GetAttributes().Find(aq2 => aq2.attribute == aq1.attribute);
            if (matchingAQ != null)
            {
                // If they do, UnityEngine.Randomize between the two quantities to keep.
                newBrainrot.AddAttribute(new AttributeQuantity
                {
                    attribute = aq1.attribute,
                    quantity = Random.Range(aq1.quantity, matchingAQ.quantity)
                });
            }
        }

        // For the rest of the attributes that aren't shared, randomize a chance to keep each based on the fail chance.
        foreach (AttributeQuantity aq1 in input1.GetAttributes())
        {
            if (!newBrainrot.GetAttributes().Any(aq => aq.attribute == aq1.attribute))
            {
                if (UnityEngine.Random.Range(0, 100) > failChance)
                {
                    newBrainrot.AddAttribute(new AttributeQuantity
                    {
                        attribute = aq1.attribute,
                        quantity = aq1.quantity
                    });
                }
            }
        }
        foreach (AttributeQuantity aq2 in input2.GetAttributes())
        {
            if (!newBrainrot.GetAttributes().Any(aq => aq.attribute == aq2.attribute))
            {
                if (UnityEngine.Random.Range(0, 100) > failChance)
                {
                    newBrainrot.AddAttribute(new AttributeQuantity
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