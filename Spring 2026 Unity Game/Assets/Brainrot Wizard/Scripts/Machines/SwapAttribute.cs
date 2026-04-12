using System.Linq;
using UnityEngine;

public class SwapAttribute : MachineFunctionality
{
    /// <summary>
    /// Handles swapping 2 random attributes from the input's brainrots
    /// </summary>
    /// <param name="thisMachine">The machine to grab the inventory from.</param>
    /// <param name="indexes">The two indexes of attributes to swap between.</param>
    /// <returns>If the function is successful</returns>
    public override bool Handler(Machine thisMachine, int[] indexes)
    {
        var inputInv1 = thisMachine.GetInputFromSlot(0);
        var inputInv2 = thisMachine.GetInputFromSlot(1);
        if (inputInv1 is null || inputInv2 is null)
        {
            Debug.LogWarning("Machine is missing an input item!");
            return false;
        }
        else if (inputInv1.GetItemType() != ItemType.Brainrot || inputInv2.GetItemType() != ItemType.Brainrot)
        {
            Debug.LogWarning("One of or both of machine's input is not a brainrot!");
            return false;
        }
        else if (thisMachine.GetOutputFromSlot(0) != null || thisMachine.GetOutputFromSlot(1) != null)
        {
            Debug.LogWarning("One of or both of machine's output is not empty!");
            return false;
        }

        var inputBrainrot1 = inputInv1 as Brainrot;
        var inputBrainrot2 = inputInv2 as Brainrot;

        if (inputBrainrot1.GetAttributes() == null || inputBrainrot2.GetAttributes() == null)
        {
            Debug.LogWarning("One of or both of input brainrot has null attributes!");
            return false;
        }
        else if (inputBrainrot1.GetAttributeCount() == 0 || inputBrainrot2.GetAttributeCount() == 0) {
            Debug.LogWarning("One of or both of input brainrot has no attributes to swap!");
            return false;
        }
        else if (indexes[0] > inputBrainrot1.GetAttributeCount() || indexes[1] > inputBrainrot2.GetAttributeCount() || indexes[0] < 0 || indexes[1] < 0)
        {
            Debug.LogWarning("One of or both of provided attribute indexes are out of range!");
            return false;
        }

            Brainrot[] machineOutput = SwapRandomAttribute(inputBrainrot1, inputBrainrot2, indexes[0], indexes[1], thisMachine.failChance);

        thisMachine.AddItemToOutput(machineOutput[0]);
        thisMachine.AddItemToOutput(machineOutput[1]);
        thisMachine.RemoveItemFromInput(inputBrainrot1);
        thisMachine.RemoveItemFromInput(inputBrainrot2);

        return true;
    }
    /// <summary>
    /// Swaps a specified attribute between two Brainrot instances, with a chance for each swap to fail and select a
    /// different attribute instead.
    /// </summary>
    /// <remarks>The method creates runtime copies of the input Brainrot instances to avoid modifying the
    /// original assets. For each attribute swap, there is a chance that the intended attribute will not be swapped;
    /// instead, a different attribute may be selected at random. The original input objects remain unchanged.</remarks>
    /// <param name="input1">The first Brainrot instance to participate in the attribute swap. Cannot be null.</param>
    /// <param name="input2">The second Brainrot instance to participate in the attribute swap. Cannot be null.</param>
    /// <param name="attributeToSwap1">The zero-based index of the attribute to swap from the first Brainrot instance.</param>
    /// <param name="attributeToSwap2">The zero-based index of the attribute to swap from the second Brainrot instance.</param>
    /// <param name="failChance">The percentage chance (0-100) that each attribute swap will fail and select a different attribute instead. This chance is applied independently to each swap.</param>
    /// <returns>An array containing two new Brainrot instances with the specified attributes swapped. Returns null if either
    /// input1 or input2 is null.</returns>
    protected static Brainrot[] SwapRandomAttribute(Brainrot input1, Brainrot input2, int attributeToSwap1, int attributeToSwap2, int failChance)
    {
        if (input1 == null)
        {
            Debug.LogWarning("MachineData.SwapRandomAttribute called with null input1.");
            return null;
        }
        if (input2 == null)
        {
            Debug.LogWarning("MachineData.SwapRandomAttribute called with null input2.");
            return null;
        }

        if (input1.GetAttributes() == null)
        {
            Debug.LogWarning("Brainrot 1 is missing Attributes!");
            return null;
        }

        if (input2.GetAttributes() == null)
        {
            Debug.LogWarning("Brainrot 2 is missing Attributes!");
            return null;
        }

        // This is the percentage chance for each attribute, so since it's 2 attributes, we divide by 2.
        int chanceOfFail = failChance / 2;

        // Create clones of the attributes to swap.
        int attribute1 = attributeToSwap1;
        int attribute2 = attributeToSwap2;

        // Total all attributes that currently exist on the Brainrot.
        int totalAttributes1 = input1.GetAttributes().Sum(aq => aq.quantity);
        int totalAttributes2 = input2.GetAttributes().Sum(aq => aq.quantity);

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

        var temp = input1.GetAttributes()[attribute1];
        input1.GetAttributes()[attribute1] = input2.GetAttributes()[attribute2];
        input2.GetAttributes()[attribute2] = temp;

        return new[] { input1, input2};
    }
}