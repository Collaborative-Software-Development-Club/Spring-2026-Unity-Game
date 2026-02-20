using System;
using UnityEngine;

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
    public Inventory input = new Inventory(0);
    public AttributeQuantity[] requirements = new AttributeQuantity[0];
    public Contract(int numOfAttributes = 0)
    {
        Array valuesOfContractType = Enum.GetValues(typeof(ContractType));
        int randomIndex = UnityEngine.Random.Range(1, valuesOfContractType.Length);
        this.contractType = (ContractType)valuesOfContractType.GetValue(randomIndex);
    }
    public Contract(ContractType type, int numOfAttributes = 0) : this(numOfAttributes)
    {
        data.type = ItemType.Contract;
        this.contractType = type;
        if (numOfAttributes <= 0)
        {
            numOfAttributes = UnityEngine.Random.Range(1, 4);
        }
        this.input = new Inventory(numOfAttributes);
    }
    
}
