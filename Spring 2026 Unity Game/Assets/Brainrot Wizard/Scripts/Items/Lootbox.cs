using UnityEngine;

public class Lootbox : Item
{
    public Inventory Items;
    public string Name;
    // the name is the name of the lootbox, the inventory is a list of all items (items) with
    // quantities being the additive rarities (e.g. item a at quantity 50 and item b at quantity 50 leaves a half/half chance of each)
    public void Initialize(string name, Inventory itemsRates) {
        Name = name;
        Items = itemsRates;
    }

    public override void Initialize(ItemData itemdata) {
        print("Lootbox not initialized, do not initialize using ItemData. Itemdata is: ");
        print(itemdata);
    }

    public override void Initialize(ItemData itemdata, string givenString) {
        print("Lootbox not initialized, do not initialize using ItemData. Itemdata and string are: ");
        print(itemdata);
        print(givenString);
    }

    public 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
