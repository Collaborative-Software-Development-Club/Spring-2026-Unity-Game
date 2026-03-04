using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public enum ContractType
{
    None,
    AtLeastAmount,
    AtMostAmount,
    MoreThanAmount,
    LessThanAmount,
    IncludeAmount,
    ExcludeAmount
}

public class Contract : Item
{
    public ContractType contractType = ContractType.None;
    private string _personName;
    public Inventory input = new Inventory(0);
    public AttributeQuantity[] requirements = new AttributeQuantity[0];

    private int _currentTurnCount = 0;
    
    private ContractData Data => data as ContractData;

    /// <summary>
    /// Shared initialization used instead of constructors.
    /// Call this after instantiating the GameObject that contains this Component.
    /// </summary>
    /// <param name="type">If null, a random contract type will be chosen.</param>
    /// <param name="numOfAttributes">If 0 or negative, a random size (1-3) will be chosen.</param>
    public void Initialize(ContractType? type = null, int numOfAttributes = 0)
    {
        // Choose or assign contract type
        if (type.HasValue)
        {
            contractType = type.Value;
        }
        else
        {
            Array values = Enum.GetValues(typeof(ContractType));
            int randomIndex = UnityEngine.Random.Range(1, values.Length); // skip None
            contractType = (ContractType)values.GetValue(randomIndex);
        }

        // Determine input inventory size
        if (numOfAttributes <= 0)
        {
            numOfAttributes = UnityEngine.Random.Range(1, 4);
        }

        input = new Inventory(numOfAttributes);

        // Assign a new random name.
        int randomNameIndex = UnityEngine.Random.Range(0, ContractData.ContractNames.Length);

        // Initialize requirements with random attributes and quantities.
        int requirementsCount = UnityEngine.Random.Range(1, 4);
        requirements = new AttributeQuantity[requirementsCount];
        for (int i = 0; i < requirementsCount; i++)
        {
            // For simplicity, we'll just use a random attribute name and quantity.
            Array values = Enum.GetValues(typeof(Attribute));
            int randomIndex = UnityEngine.Random.Range(1, values.Length); // skip None
            Attribute newAttribute = (Attribute)values.GetValue(randomIndex);
            int quantity = UnityEngine.Random.Range(1, 5);
            requirements[i] = new AttributeQuantity { attribute = newAttribute, quantity = quantity };
        }

    }

    // Keep compatibility with the project's Initialize pattern (override from Item).
    // These call Init so all logic is in one place.

    /// <summary>
    /// Initializes this Contract from the provided <paramref name="itemData"/>.
    /// Validates that <paramref name="itemData"/> has type <see cref="ItemType.Contract"/>.
    /// Assigns the provided ItemData to this contract and delegates to the parameterized Initialize
    /// to ensure the input inventory and requirements are set up.
    /// </summary>
    /// <param name="itemData">ItemData instance expected to be of type <see cref="ItemType.Contract"/>.</param>
    public override void Initialize(ItemData itemData)
    {
        if (itemData is not ContractData contractData)
        {
            Debug.LogError("Invalid item data type for contract: " + itemData.type);
            return;
        }

        // Assign the ItemData reference so the base Item behaviour can use it.
        data = contractData;

        // Default init: if data contains any designer defaults you may read them here.
        // We'll create a reasonable input size from the data if present, otherwise use defaults.
        // Simplified: ensure an input inventory exists
        if (input == null)
        {
            Initialize(null, 0);
        } else
        {
            int numOfAttributes = input.Length;
            Initialize(null, numOfAttributes);
        }

        _personName = ContractData.ContractNames[Random.Range(0, ContractData.ContractNames.Length - 1)];
    }

    /// <summary>
    /// Initializes this Contract using <paramref name="itemData"/> and then sets the contract's display name
    /// to <paramref name="itemName"/> if initialization succeeded.
    /// </summary>
    /// <param name="itemData">ItemData instance expected to be of type <see cref="ItemType.Contract"/>.</param>
    /// <param name="itemName">Display name to assign to the contract's ItemData.</param>
    public override void Initialize(ItemData itemData, string itemName)
    {
        Initialize(itemData);
        if (Data != null)
            Data.name = itemName;
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

    public override double GetValue()
    {
        return base.GetValue() * ContractData.DifficultyMultiplier[Data.difficulty];
    }

    public static Contract GenerateRandomContract()
    {
        return (Contract) ItemFactory.CreateItem(GameManager.Instance.ContractManager.GetContractDatabase().GetRandom());
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
        lines.Add($"Type: {contractType}");
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