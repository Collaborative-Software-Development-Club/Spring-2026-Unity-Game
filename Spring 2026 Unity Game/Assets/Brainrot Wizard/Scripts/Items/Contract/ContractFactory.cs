using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public static class ContractFactory
{
    private const int DifficultyMultiplier = 5;
    private const int DifficultyMaxBonus = 5;
    private const int QtyMultiplier = 2; 
    private const int QtyMaxBonus = 3;

    private const int CostPerAttribute = 7;
    private const int CostPerStability = 9;
    private const int AttributeDiversityCost = 15;
    private const int OptionalBonusCost = 5;

    private const int MinScoreForStability = 20;
    private const int MinScoreForRarity = 25;
    private const int MinScoreForDiversity = 25;
    private const int MinScoreForOptional = 10;

    private static readonly Dictionary<Rarity, int> RarityCosts = new Dictionary<Rarity, int>
    {
        { Rarity.Common, 0 },
        { Rarity.Uncommon, 1 },
        { Rarity.Rare, 5 },
        { Rarity.Epic, 12 },
        { Rarity.Legendary, 25 }
    };

    private static readonly int AttributeCount = Enum.GetValues(typeof(Attribute)).Length;

    public static Contract GenerateRandomContract(int level)
    {
        var contract = new Contract();
        var possibleAttributes = new List<Attribute>((Attribute[])Enum.GetValues(typeof(Attribute)));
        
        contract.SetPersonName(ContractData.ContractNames[Random.Range(0, ContractData.ContractNames.Length)]);
        contract.SetTurnDuration(CalculateTurnLimit(level));
        
        int initialBudget = (level * DifficultyMultiplier) + Random.Range(0, DifficultyMaxBonus);
        int remainingBudget = initialBudget;

        // Reserve budget for at least one Primary attribute
        int reservedForCore = CostPerAttribute;
        int spendableOnExtras = initialBudget - reservedForCore;

        // --- Extra Requirements ---
        if (initialBudget >= MinScoreForStability && spendableOnExtras >= CostPerStability)
        {
            if (Random.value > 0.5f)
            {
                contract.AddExtra(ExtraRequirement.BrainrotStability);

                float normalizedLevel = level / 10f;

                float minReq = Mathf.Lerp(50f, 80f, normalizedLevel);
                float maxReq = Mathf.Lerp(60f, 90f, normalizedLevel);

                int stabilityRequirement = Mathf.RoundToInt(Random.Range(minReq, maxReq));

                contract.SetStabilityRequirement(stabilityRequirement);

                remainingBudget -= CostPerStability;
                spendableOnExtras -= CostPerStability;
            }
        }

        if (initialBudget >= MinScoreForRarity)
        {
            Rarity randomRarity = (Rarity)Random.Range(1, Enum.GetValues(typeof(Rarity)).Length);
            int rarityCost = RarityCosts[randomRarity];

            if (spendableOnExtras >= rarityCost)
            {
                contract.SetRarityRequirement(randomRarity);
                remainingBudget -= rarityCost;
                spendableOnExtras -= rarityCost;
            }
        }

        if (initialBudget >= MinScoreForDiversity && spendableOnExtras >= AttributeDiversityCost)
        {
            if (Random.value > 0.7f)
            {
                contract.AddExtra(ExtraRequirement.AttributeDiversity);
                remainingBudget -= AttributeDiversityCost;
                spendableOnExtras -= AttributeDiversityCost;
            }
        }

        var primary = GenerateRequirement(level, initialBudget, ref possibleAttributes);

        if (primary != null)
        {
            primary.Priority = AttributeRequirementPriority.Primary;
            contract.AddRequirement(primary);
            remainingBudget -= CostPerAttribute;
        }
        else
        {
            Debug.LogWarning("Couldn't produce a primary requirement!");
        }
        
        // --- Attribute Requirements ---
        int attributeCount = 0;
        while (remainingBudget >= CostPerAttribute)
        {
            var req = GenerateRequirement(level, initialBudget, ref possibleAttributes);

            if (req == null)
                break;
            
            // Assign priority based on creation order
            req.Priority = attributeCount switch
            {
                0 => AttributeRequirementPriority.Primary,
                1 => AttributeRequirementPriority.Secondary,
                _ => AttributeRequirementPriority.Optional
            };

            if (req.Priority == AttributeRequirementPriority.Optional)
            {
                // Gatekeeper for optional attributes
                if (initialBudget < MinScoreForOptional || remainingBudget < (CostPerAttribute + OptionalBonusCost))
                    break; 
                
                remainingBudget -= OptionalBonusCost;
            }

            contract.AddRequirement(req);
            remainingBudget -= CostPerAttribute;
            attributeCount++;
        }
        
        return contract;
    }

    private static AttributeRequirementElement GenerateRequirement(int level, int initialDifficulty, ref List<Attribute> possibleAttributes)
    {
        if (possibleAttributes.Count <= 0)
        {
            Debug.LogWarning("No more attributes available");
            return null;
        }
        
        var randomAttribute = possibleAttributes[Random.Range(0, possibleAttributes.Count)];
        possibleAttributes.Remove(randomAttribute);
        
        return new AttributeRequirementElement
        {
            AttributeQuantity = new AttributeQuantity
            {
                attribute = randomAttribute, 
                quantity = (level * QtyMultiplier) + Random.Range(0, QtyMaxBonus)
            },
            AttributeRequirements = GetRandomRequirementType(initialDifficulty)
        };
    }

    private static AttributeRequirement GetRandomRequirementType(int difficulty)
    {
        if (difficulty < 15)
        {
            return Random.value > 0.5f ? AttributeRequirement.GreaterThan : AttributeRequirement.LessThan;
        }

        return Random.Range(0, 3) switch
        {
            1 => AttributeRequirement.GreaterThan,
            2 => AttributeRequirement.LessThan,
            _ => AttributeRequirement.Exact
        };
    }
    
    private static int CalculateTurnLimit(int level)
    {
        int baseTurns = 10;          
        int reductionPerLevel = 1;  
        int minTurns = 3;            

        int turns = baseTurns - (level * reductionPerLevel);

        return Math.Max(minTurns, turns);
    }
}