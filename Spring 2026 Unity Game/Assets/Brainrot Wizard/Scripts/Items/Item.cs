using UnityEngine;

public abstract class Item : MonoBehaviour
{
    private ItemData itemData;
    protected virtual ItemData GetItemData() => itemData; // what does this do

    public void PrintData()
    {
        if(!HasData())
        {
            Debug.LogWarning($"{name}: ItemData is NULL", this);
            return;
        }

        Debug.Log(GetData());
    }

    public virtual string GetData()
    {
        return $"[{GetRarity()}] {GetName()}\n" +
               $"Type: {GetItemType()}\n" +
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
        return GetItemData().name;
    }
    public virtual string GetDescription()
    {
        return GetItemData().text;
    }
    public virtual double GetValue()
    {
        return GetItemData().value;
    }
    public virtual ItemType GetItemType()
    {
        return GetItemData().type;
    }
    public virtual Rarity GetRarity()
    {
        return GetItemData().rarity;
    }
    public virtual Sprite GetIcon()
    {
        return GetItemData().icon;
    }
    public bool HasData()
    {
        return GetItemData() != null;
    }
    protected virtual void OnValidate()
    {
        if (HasData()) return;
        
        Debug.LogWarning($"{name} has no ItemData assigned", this);
    }
}