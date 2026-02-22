using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Brainrot : Item 
{
    private Category _category;
    
    // Could switch to dictionary in the future
    private List<AttributeQuantity> _attributes;


    public override void Initialize(ItemData itemData)
    {
        if (itemData is not BrainrotData brainrotData)
        {
            Debug.LogError("Brainrot requires BrainrotData");
            return;
        }

        data = brainrotData;
        _category = brainrotData.category;
        
        _attributes = brainrotData.attributes != null
            ? brainrotData.attributes
                .Select(a => new AttributeQuantity(a.attribute, a.quantity))
                .ToList()
            : new List<AttributeQuantity>();
    }
    public override void Initialize(ItemData itemData, string itemName)
    {
        Initialize(itemData);
        Name = itemName;
    }

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
        return _attributes;
    }
    public Category GetCategory()
    {
        return _category;
    }

    /// <summary>
    /// Adds the given attribute and amount
    /// </summary>
    /// <param name="attribute">The attribute to add</param>
    /// <param name="quantity">The amount to add</param>
    public void AddAttribute(Attribute attribute, int quantity)
    {
        var temp = new AttributeQuantity(attribute, quantity);
        AddAttribute(temp);
    }

    /// <summary>
    /// The attribute quantity to add
    /// </summary>
    /// <param name="attribute">The attribute quantity to add</param>
    public void AddAttribute(AttributeQuantity attribute)
    {
        if (attribute.quantity < 0)
        {
            Debug.LogError("Quantity must be >= 0");
            return;
        }

        int index = FindAttributeIndex(attribute.attribute);

        if (index < 0)
            _attributes.Add(new AttributeQuantity(attribute.attribute, attribute.quantity));
        else
            _attributes[index].quantity += attribute.quantity;
    }
    /// <summary>
    /// Removes the given attribute and amount
    /// </summary>
    /// <param name="attribute">The attribute to remove</param>
    /// <param name="quantity">The amount to remove</param>
    public void RemoveAttribute(Attribute attribute, int quantity)
    {
        var temp = new AttributeQuantity(attribute, quantity);
        RemoveAttribute(temp);
    }

    /// <summary>
    /// Removes the given attribute and amount
    /// </summary>
    /// <param name="attribute">The attribute quantity to add and remove</param>
    public void RemoveAttribute(AttributeQuantity attribute)
    {
        if (attribute.quantity < 0)
        {
            Debug.LogError("Quantity must be >= 0");
            return;
        }        
        int index = FindAttributeIndex(attribute.attribute);

        if (index < 0) return;
        
        _attributes[index].quantity -= attribute.quantity;

        if (_attributes[index].quantity > 0) return;
        
        _attributes.RemoveAt(index);
    }

    /// <summary>
    /// Finds the index of the given attribute
    /// </summary>
    /// <param name="attribute">The attribute to find</param>
    /// <returns>The index of the attribute</returns>
    public int FindAttributeIndex(Attribute attribute)
    {
        for (int i = 0; i < _attributes.Count; i++)
        {
            if (_attributes[i].attribute == attribute)
            {
                return i;
            }
        }
        
        return -1;
    }

    /// <summary>
    /// Changes the current category to the new passed in category
    /// </summary>
    /// <param name="newCategory">The category to change to</param>
    /// <returns>The old category</returns>
    public Category ChangeCategory(Category newCategory)
    {
        Category oldCategory = _category;
        _category = newCategory;
        
        return oldCategory;
    }
}
