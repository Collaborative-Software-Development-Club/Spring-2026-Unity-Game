using UnityEngine;

public abstract class Item : MonoBehaviour
{
    [SerializeField] protected ItemData data; 
    
    public void PrintData()
    {
        if(!HasData())
        {
            Debug.LogWarning($"{name}: ItemData is NULL", this);
            return;
        }

        Debug.Log(GetDataAsString());
    }

    public virtual string GetDataAsString()
    {
        return $"[{GetRarity()}] {GetName()}\n" +
               $"Type: {GetType()}\n" +
               $"Value: {GetValue()}\n" +
               $"Description: {GetDescription()}";
    }
    
    public virtual string GetDisplayName()
    {
        return GetRarity() switch
        {
            Rarity.Common => GetName(),
            _ => $"{GetRarity()} {GetName()}"
        };
    }
    public virtual string GetTooltip()
    {
        return
            $"{GetDisplayName()}\n" +
            $"{GetDescription()}\n\n" +
            $"Value: {GetValue()}";
    }
    public virtual string GetName()
    {
        return data.name;
    }
    public virtual string GetDescription()
    {
        return data.text;
    }
    public virtual double GetValue()
    {
        return data.value;
    }
    public new virtual ItemType GetType()
    {
        Debug.Log(data is null);
        return data.type;
    }
    public virtual Rarity GetRarity()
    {
        return data.rarity;
    }
    public virtual Sprite GetIcon()
    {
        return data.icon;
    }
    public bool HasData()
    {
        return !(data is null);
    }
    protected virtual void OnValidate()
    {
        if (HasData()) return;
        
        Debug.LogWarning($"{name} has no ItemData assigned", this);
    }
}