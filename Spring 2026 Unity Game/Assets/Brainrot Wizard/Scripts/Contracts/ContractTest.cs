using UnityEngine;

public class ContractTest : MonoBehaviour
{
    /// <summary>
    /// Test helper that demonstrates creating a Contract and a Brainrot at runtime,
    /// initializing the contract, adding the Brainrot to the contract's input inventory,
    /// and logging the contract type, name and current inventory count.
    /// </summary>
    /// <remarks>
    /// MonoBehaviours must be created via GameObject.AddComponent; this method shows the correct pattern for quick runtime tests.
    /// In real unit tests prefer using Unity Test Framework and editor play mode tests.
    /// </remarks>
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // You cannot `new` a MonoBehaviour. Create a GameObject and AddComponent instead.
        var contractGO = new GameObject("TestContract");
        var test = contractGO.AddComponent<Contract>();

        // Initialize the contract (null = random ContractType, 3 input slots)
        test.Initialize(null, 3);

        // Create a Brainrot MonoBehaviour instance on its own GameObject and add it to the inventory.
        // Note: Brainrot is a MonoBehaviour so it must be created via AddComponent.
        var brainGO = new GameObject("TestBrainrot");
        var brain = brainGO.AddComponent<Brainrot>();
        // If you have BrainrotData, call brain.Initialize(brainrotData) here to give it valid ItemData.

        // Add the brainrot item to the contract input inventory
        test.input.AddItemToInventory(brain);

        // Print results
        Debug.Log(test.contractType);
        Debug.Log(test.getContractName());
        Debug.Log(test.input.GetTotalItemCount());
    }
}