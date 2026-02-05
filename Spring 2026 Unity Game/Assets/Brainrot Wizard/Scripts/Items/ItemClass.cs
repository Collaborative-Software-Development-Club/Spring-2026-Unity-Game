using UnityEngine;

public enum ItemType{
    None,
    Machine,
    Brainrot,
    Tool,
    Seed,
    Lootbox
}

public enum Rarity
{
    None,
    Common,
    Uncommon,
    Rare,
    Epic,
    Legendary,
}

[CreateAssetMenu(fileName = "New Item", menuName = "Game/BrainrotMixer/Item/Base Item")]
public class ItemClass : ScriptableObject
{
    public string itemName = "Unnamed";
    public ItemType itemType =  ItemType.None;
    public Rarity rarity = Rarity.None;
    public double value = 0;
    public Sprite itemIcon;
    public string itemText;
}