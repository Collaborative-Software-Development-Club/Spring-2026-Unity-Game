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

        Brainrot[] machineOutput = FissionAttribute(inputBrainrot, thisMachine.failChance);

        thisMachine.AddItemToOutput(machineOutput[0]);
        if (machineOutput[1] != null) {
            thisMachine.AddItemToOutput(machineOutput[1]);
        }
        thisMachine.RemoveItemFromInput(inputBrainrot);
        return true;
    }
}