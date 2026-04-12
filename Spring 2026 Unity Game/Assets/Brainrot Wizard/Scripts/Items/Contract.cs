using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class Contract : Item
{
    private string _personName;
    public Inventory input = new Inventory(0);
    public AttributeQuantity[] requirements = new AttributeQuantity[0];

    private int _currentTurnCount = 0;
    
    private ContractData Data => data as ContractData;

    // Keep compatibility with the project's Initialize pattern (override from Item).
    // These call Init so all logic is in one place.

    /// <summary>
    /// Initializes this Contract from the provided <paramref name="itemData"/>.
    /// Validates that <paramref name="itemData"/> has type <see cref="ItemType.Contract"/>.
    /// Assigns the provided ItemData to this contract and delegates to the parameterized Initialize
    /// to ensure the input inventory and requirements are set up.
    /// </summary>
    /// <param name="itemData">ItemData instance expected to be of type <see cref="ItemType.Contract"/>.</param>
    public Contract(ContractData data)
    {
        this.data = data;
        
        _personName = ContractData.ContractNames[Random.Range(0, ContractData.ContractNames.Length - 1)];
    }

    /// <summary>
    /// Initializes this Contract using <paramref name="itemData"/> and then sets the contract's display name
    /// to <paramref name="itemName"/> if initialization succeeded.
    /// </summary>
    /// <param name="itemData">ItemData instance expected to be of type <see cref="ItemType.Contract"/>.</param>
    /// <param name="itemName">Display name to assign to the contract's ItemData.</param>
    public Contract(ContractData itemData, string itemName) : this(itemData) 
    {
        Name = itemName;
    }

    /// <summary>
    /// Returns the contract display name from its <see cref="ItemData"/> if present; otherwise returns "Unnamed Contract".
    /// </summary>
    /// <returns>Contract name string.</returns>
    public string getContractName() 
    {
        return Data != null ? Data.name : "Unnamed Contract";
    }

    public bool IsPastDuration()
    {
        return _currentTurnCount < Data.TurnDuration;
    }

    public void IncrementDuration()
    {
        _currentTurnCount++;
    }

    public void DecrementDuration()
    {
        _currentTurnCount--;
    }

    public int GetTurnCount()
    {
        return _currentTurnCount;
    }

    public override double GetValue()
    {
        return base.GetValue() * ContractData.DifficultyMultiplier[Data.difficulty];
    }

    public static Contract GenerateRandomContract()
    {
        return new Contract(GameManager.Instance.ContractManager.GetContractDatabase().GetRandom());
    }

    public string GetPersonName()
    {
        return _personName;
    }

    public new List<string> GetDataAsString()
    {
        var lines = new List<string>();

        if (Data == null)
            return lines;

        lines.Add($"{_personName}'s Contract");
        lines.Add($"Difficulty: {Data.difficulty}");
        lines.Add($"Duration: {Data.TurnDuration} turns");
        lines.Add(""); 
        lines.Add("Requirements:");

        foreach (var req in requirements)
        {
            lines.Add($"- {req.attribute}: {req.quantity}");
        }

        return lines;
    }
}