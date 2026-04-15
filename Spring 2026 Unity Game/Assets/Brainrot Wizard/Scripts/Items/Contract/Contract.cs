using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
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

    private int stabilityRequirement;
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
    public void AddRequirement(AttributeRequirementElement attributeRequirement)
    {
        _attributeRequirements.Add(attributeRequirement);
    }

    public void AddExtra(ExtraRequirement extraRequirement)
    {
        _extraRequirements.Add(extraRequirement);
    }

    public void SetStabilityRequirement(int stability)
    {
        stabilityRequirement = stability;
    }
    public void SetRarityRequirement(Rarity rarity)
    {
        _rarityRequirement = rarity;
        _extraRequirements.Add(ExtraRequirement.RarityRequirement);
    }

    public void SetTurnDuration(int duration)
    {
        turnDuration = turnDuration <= 0 ? 1 : duration;
        _currentTurnCount = turnDuration;
    }

    public void SetPersonName(string name)
    {
        _personName = name;
    }

    public List<string> GetPrimaryAsString()
    {
        return _attributeRequirements
            .Where(ar => ar.Priority == AttributeRequirementPriority.Primary)
            .Select(FormatAttribute) 
            .ToList();
    }

    public List<string> GetSecondaryAsString()
    {
        return _attributeRequirements
            .Where(ar => ar.Priority == AttributeRequirementPriority.Secondary)
            .Select(FormatAttribute)
            .ToList();
    }

    public List<string> GetOptionalAsString()
    {
        return _attributeRequirements
            .Where(ar => ar.Priority == AttributeRequirementPriority.Optional)
            .Select(FormatAttribute)
            .ToList();
    }

    private string FormatAttribute(AttributeRequirementElement requirement)
    {
        string rawName = requirement.AttributeQuantity.attribute.ToString();
        string formattedName = StringUtils.PlaceSeparators(rawName).Replace("T V", "TV");
    
        return $"{GetSymbolFromRequirement(requirement.AttributeRequirements)}{requirement.AttributeQuantity.quantity} {formattedName}";
    }

    public List<string> GetExtraAsString()
    {
        List<string> extras = new List<string>();
        
        foreach (ExtraRequirement req in _extraRequirements)
        {
            switch (req)
            {
                case ExtraRequirement.BrainrotStability:
                    extras.Add(StringUtils.PlaceSeparators(req.ToString()) + " = " + stabilityRequirement);
                    break;
                case ExtraRequirement.RarityRequirement:
                    extras.Add("Required rarity: " + _rarityRequirement);
                    break;
                case ExtraRequirement.AttributeArchetypeSynergy:
                    Debug.LogWarning("Synergies not implemented yet");
                    break;
                case ExtraRequirement.AttributeDiversity:
                    Debug.LogWarning("Diversty not implemented yet");
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        return extras;
    }
    public static string GetSymbolFromRequirement(AttributeRequirement requirement)
    {
        var symbol = requirement switch
        {
            AttributeRequirement.Exact => "=",
            AttributeRequirement.GreaterThan => ">",
            AttributeRequirement.LessThan => "<",
            _ => throw new ArgumentOutOfRangeException()
        };

        return symbol;
    }
}