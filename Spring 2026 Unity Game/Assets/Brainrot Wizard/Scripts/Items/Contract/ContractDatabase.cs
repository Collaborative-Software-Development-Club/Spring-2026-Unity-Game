using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Contract Database", menuName = "Game/BrainrotMixer/Contracts/ContractDatabase")]
public class ContractDatabase : ScriptableObject
{
    public List<ContractData> validContracts = new List<ContractData>();

    public ContractData GetRandom()
    {
        if (validContracts.Count == 0)
        {
            Debug.LogError("No valid contracts assigned in ContractDatabase.");
            return null;
        }

        int index = Random.Range(0, validContracts.Count);
        return validContracts[index];
    }
}