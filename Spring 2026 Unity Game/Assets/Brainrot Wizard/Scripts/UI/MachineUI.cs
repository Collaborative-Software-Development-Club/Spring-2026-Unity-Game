using UnityEngine;
using UnityEngine.UI;

public class MachineUI : MonoBehaviour
{
    public GameObject machineBackgroundUI;
    public GameObject machineSlotPrefab;
    public ScrollRect inputScrollView;
    public ScrollRect outputScrollView;

    public void Open(Machine machine)
    {
        Inventory inputs = machine.GetInputInventory();
        Inventory outputs = machine.GetOutputInventory();

        foreach (InventorySlot slot in inputs.slots)
        {
            GameObject newSlot = GameManager.Instance.GUIManager.CreateSlot(slot);
            newSlot.transform.SetParent(inputScrollView.content, false);
        }

        foreach (InventorySlot slot in outputs.slots)
        {
            GameObject newSlot = GameManager.Instance.GUIManager.CreateSlot(slot);
            newSlot.transform.SetParent(outputScrollView.content, false);
        }
        
        gameObject.SetActive(true);
    }

    public void Close()
    {
        foreach (Transform child in inputScrollView.content)
        {
            Destroy(child.gameObject);
        }

        foreach (Transform child in outputScrollView.content)
        {
            Destroy(child.gameObject);
        }
        gameObject.SetActive(false);
    }
}
