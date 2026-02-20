using UnityEngine;

public class ChuckTesta : MonoBehaviour
{
    Inventory main = new Inventory(10);
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Debug.Log("BRAINROT TEST SUITE: CHUCK TESTA");

        Debug.Log("INVENTORY SHOULD BE TEN ITEMS LONG, LET'S SEE");
        Debug.Log(main.Length());

        Debug.Log("ADDING A NULL BRAINROT");
        print(main.AddItemToInventory(new Brainrot()));

        Debug.Log("BRAINROT SHOULD BE AT SLOT ONE, LET'S SEE");
        Debug.Log("SLOT: " + main.GetItemAt(0));
        Debug.Log("TYPE: " + main.GetItemAt(0).item);
        Debug.Log("QUANTITY: " + main.GetItemAt(0).quantity);

        Debug.Log("LET'S SEE IF THE INVENTORY KNOWS HOW MANY ITEMS IT HAS");
        Debug.Log("GetTotalItemCount: " + main.GetTotalItemCount());
    }

}
