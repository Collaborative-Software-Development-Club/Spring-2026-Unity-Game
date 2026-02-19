using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Brainrot : Item 
{
    private BrainrotData BrainrotData => data as BrainrotData;
 

    public override string GetDataAsString()
    {
        string dataAsString = base.GetDataAsString();
    
        dataAsString += "\nCategory: " + GetCategory();
    
        var attributes = GetAttributes();
        
        if (attributes != null && attributes.Count > 0)
        {
            dataAsString += "\nAttributes:";
            foreach (var attribute in attributes)
            {
                dataAsString += $"\n  • {attribute.attribute}: {attribute.quantity}";
            }
        }
        else
        {
            dataAsString += "\nAttributes: None";
        }

        return dataAsString;
    }

    //protected override ItemType RequiredItemType { get; }

    public List<AttributeQuantity> GetAttributes()
    {
        return BrainrotData.attributes;
    }
    public Category GetCategory()
    {
        return BrainrotData.category;
    }
}
