using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Brainrot : Item 
{
    protected new BrainrotData Data; 

    public override string GetDataAsString()
    {
        string data = base.GetDataAsString();
    
        data += "\nCategory: " + GetCategory();
    
        var attributes = GetAttributes();

        if (attributes != null && attributes.Count > 0)
        {
            data += "\nAttributes:";
            foreach (var attribute in attributes)
            {
                data += $"\n  • {attribute.attribute}: {attribute.quantity}";
            }
        }
        else
        {
            data += "\nAttributes: None";
        }

        return data;
    }

    //protected override ItemType RequiredItemType { get; }

    public List<AttributeQuantity> GetAttributes()
    {
        return Data.attributes;
    }
    public Category GetCategory()
    {
        return Data.category;
    }
}
