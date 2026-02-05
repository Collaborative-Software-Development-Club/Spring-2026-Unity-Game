using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Brainrot : Item 
{
    [SerializeField] private BrainrotData brainrotData;
    protected override ItemData GetItemData() => brainrotData;

    public override string GetData()
    {
        string data = base.GetData();
    
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

    public List<AttributeQuantity> GetAttributes()
    {
        return brainrotData.attributes;
    }
    public Category GetCategory()
    {
        return brainrotData.category;
    }
}
