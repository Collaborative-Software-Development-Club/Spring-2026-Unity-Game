using UnityEngine;

public class ContractTest : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Contract test = new Contract(3);
        test.input.AddItemToInventory(new Brainrot(), 4);
        print(test.contractType);
        print(test.input.GetTotalItemCount());
    }
}