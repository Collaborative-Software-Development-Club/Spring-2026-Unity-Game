using UnityEngine;

public class ChuckTesta : MonoBehaviour
{
    Inventory main = new Inventory(10);
    Inventory hotbar = new Inventory(5);
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Debug.Log("BRAINROT TEST SUITE: CHUCK TESTA");

        Debug.Log("INVENTORY SHOULD BE TEN ITEMS LONG, LET'S SEE");
        Debug.Log(main.Length);

        Debug.Log("HOTBAR SHOULD BE FIVE ITEMS LONG. LET'S SEE EVERYTHING");
        Debug.Log("INVENTORY LENGTH: " + main.Length);
        Debug.Log("HOTBAR LENGTH: " + hotbar.Length);
        Debug.Log("INVENTORY GETTOTAL: " + main.GetTotalItemCount());
        Debug.Log("HOTBAR GETTOTAL: " + hotbar.GetTotalItemCount());

        Debug.Log("ADDING A NULL BRAINROT");
        print("ADDED:" + main.AddItemToInventory(new Brainrot(), 4));

        Debug.Log("BRAINROT SHOULD BE AT SLOT ONE, LET'S SEE");
        Debug.Log("SLOT: " + main.GetItemAt(0));
        Debug.Log("TYPE: " + main.GetItemAt(0).item);
        Debug.Log("QUANTITY: " + main.GetItemAt(0).quantity);

        Debug.Log("LET'S SEE IF THE INVENTORY KNOWS HOW MANY ITEMS IT HAS");
        Debug.Log("GetTotalItemCount: " + main.GetTotalItemCount());

        Debug.Log("NOW I'M GOING TO SWAP OUT ITEMS. GIVE ME A BRAINROT!");
        Debug.Log("BRAINROT ADDED TO HOTBAR: " + hotbar.AddItemToInventory(new Brainrot()));
        Debug.Log("HOTBAR SLOT ONE QUANTITY: " + hotbar.GetItemAt(0).quantity);
        Debug.Log("I DON'T WANT A NULL BRAINROT ANYMORE!");
        Debug.Log("BRAINROT TYPE: " + hotbar.GetItemAt(0).item.GetItemType());
        //hotbar.GetItemAt(0).item.type = ItemType.Brainrot;
        Debug.Log("BRAINROT TYPE: " + hotbar.GetItemAt(0).item.GetItemType());
        Debug.Log("CHUCK TESTA WILL RETURN LATER!");
    }

}
