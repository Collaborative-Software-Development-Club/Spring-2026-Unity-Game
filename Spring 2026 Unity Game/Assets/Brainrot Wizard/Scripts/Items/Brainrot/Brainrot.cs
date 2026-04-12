using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Brainrot : Item 
{
    private Category _category;
    private int _stability = 0;
    
    // Could switch to dictionary in the future
    private List<AttributeQuantity> _attributes; 

    public Brainrot(BrainrotData brainrotData)  
    {
        data = brainrotData;
        _category = brainrotData.category;
        
        _attributes = brainrotData.attributes != null
            ? brainrotData.attributes
                .Select(a => new AttributeQuantity(a.attribute, a.quantity))
                .ToList()
            : new List<AttributeQuantity>();
    }
    public Brainrot(BrainrotData brainrotData, Category category, List<AttributeQuantity> attributes)  
    {
        data = brainrotData;
        _category = category;
        
        _attributes = attributes != null
            ? attributes 
                .Select(a => new AttributeQuantity(a.attribute, a.quantity))
                .ToList()
            : new List<AttributeQuantity>();
    }
    public Brainrot(BrainrotData itemData, string itemName) : this(itemData)
    {
        Name = itemName;
    }

    public Brainrot(Brainrot other) : this(other.data as BrainrotData, other.GetCategory(), other.GetAttributes())
    {
        Name = other.Name;
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

    /// <summary>
    /// Gets the number of attributes on the brainrot.
    /// </summary>
    /// <returns>The number of attributes.</returns>
    public int GetAttributeCount()
    {
        return _attributes.Count;
    }
    
    public override bool Equals(object obj)
    {
        if (obj is not Brainrot other)
            return false;

        if (GetCategory() != other.GetCategory())
            return false;

        if (GetAttributeCount() != other.GetAttributeCount())
            return false;

        foreach (AttributeQuantity attr in _attributes)
        {
            int index = other.FindAttributeIndex(attr.attribute);
            if (index < 0)
                return false;
            
            if (other.GetAttributes()[index].quantity != attr.quantity)
                return false;
        }

        return true;
    }
    
    public override int GetHashCode()
    {
        int hash = HashCode.Combine(base.GetHashCode(), _category);

        return _attributes.Aggregate(hash, (current, attr) => HashCode.Combine(current, attr.attribute, attr.quantity));
    }

    public override Item Clone()
    {
        var clone = (Brainrot)MemberwiseClone();
        
        clone._attributes = new List<AttributeQuantity>();

        foreach (AttributeQuantity attr in _attributes)
        {
            clone._attributes.Add(new AttributeQuantity(attr.attribute, attr.quantity));
        }

        return clone;
    }

    public void ChangeStability(int amount)
    {
        _stability += amount;
        
        if(_stability < 0)
            _stability = 0;
        else if (_stability > 100)
            _stability = 100;
    }
}
