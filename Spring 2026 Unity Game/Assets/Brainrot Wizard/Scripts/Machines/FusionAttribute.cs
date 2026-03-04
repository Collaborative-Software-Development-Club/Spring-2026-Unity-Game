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

        thisMachine.AddItemToOutput(FusionAttribute(inputBrainrot1, inputBrainrot2, thisMachine.failChance));
        thisMachine.RemoveItemFromInput(inputBrainrot1);
        thisMachine.RemoveItemFromInput(inputBrainrot2);
        return true;
    }
}