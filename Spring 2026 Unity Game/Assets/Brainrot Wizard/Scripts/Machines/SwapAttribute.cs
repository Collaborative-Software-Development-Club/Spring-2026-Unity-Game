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
}