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
}