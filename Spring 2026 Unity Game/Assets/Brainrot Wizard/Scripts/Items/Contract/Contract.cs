using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public enum ContractTier
{
    PerfectMatch, // 100%
    HighQuality, // 80 - 90%
    Standard, // 50 - 79%
    Rejected // <50%
}

public enum AttributeRequirement
{
    Exact, // 1-2 from requirement number
    GreaterThan, // greater than requirement number
    LessThan, // less than requirement number
}

public enum ExtraRequirement
{
    BrainrotStability, // how stable the brainrot is
    RarityRequirement, // minimum rarity required for the brainrot
    AttributeArchetypeSynergy, // The amount of requirements the attributes belong to
    AttributeDiversity, // How many different attributes there are
}

public enum AttributeRequirementPriority
{
    Primary, // 50% of score
    Secondary, // 30% of score
    Optional, // 20% of score (bonus payout)
}

public class Contract 
{
    private string _personName;
    public Inventory input = new Inventory(0);
    private Rarity _rarityRequirement;
    private List<AttributeRequirementElement> _attributeRequirements = new(); 
    private List<ExtraRequirement> _extraRequirements = new();

    private int _currentTurnCount = 0;
    private int turnDuration = 0;


    public bool IsPastDuration()
    {
        return _currentTurnCount < turnDuration;
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

    public static Contract GenerateRandomContract()
    {
        return ContractFactory.GenerateRandomContract(GameManager.Instance.CurrentTurnCount);
    }

    public string GetPersonName()
    {
        return _personName;
    }

    public new List<string> GetDataAsString()
    {
        var lines = new List<string>();

        lines.Add($"{_personName}'s Contract");
        lines.Add($"Duration: {turnDuration} turns");
        lines.Add(""); 
        lines.Add("Requirements:");

        foreach (var req in _attributeRequirements)
        {
            string symbol = "";
            
            switch (req.AttributeRequirements)
            {
                case AttributeRequirement.Exact:
                    symbol = "=";
                    break;
                case AttributeRequirement.GreaterThan:
                    symbol = ">";
                    break;
                case AttributeRequirement.LessThan:
                    symbol = "<";
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            lines.Add($"- {symbol} {req.AttributeQuantity.attribute}: {req.AttributeQuantity.quantity}");
        }

        foreach (ExtraRequirement req in _extraRequirements)
        {
            lines.Add(StringUtils.PlaceSeparators(req.ToString()));
        }

        return lines;
    }

    public void AddRequirement(AttributeRequirementElement attributeRequirement)
    {
        _attributeRequirements.Add(attributeRequirement);
    }

    public void AddExtra(ExtraRequirement extraRequirement)
    {
        _extraRequirements.Add(extraRequirement);
    }

    public void SetRarityRequirement(Rarity rarity)
    {
        _rarityRequirement = rarity;
        _extraRequirements.Add(ExtraRequirement.RarityRequirement);
    }

    public void SetTurnDuration(int duration)
    {
        if (turnDuration <= 0)
            turnDuration = 1;
        else
            turnDuration = duration;
    }

    public void SetPersonName(string name)
    {
        _personName = name;
    }
}