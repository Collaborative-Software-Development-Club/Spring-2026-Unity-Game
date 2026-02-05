using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    [SerializeField]
    private InventorySlot[] inventory =  new InventorySlot[9];
    private int currentlySelectedSlot = 0;

    private enum SelectedItemType { None, Mineral }

    private SelectedItemType currentEquippedItemType = SelectedItemType.None;
    //private SelectedItemType lastEquippedItemType = SelectedItemType.None;

    public GameObject itemPrefab;
    private SpriteRenderer itemSpriteRenderer;

    public GameObject[] inventorySlots = new  GameObject[9];
    public GameObject hotbarSelect;

    public GameObject displaySelect;
    
    private void Awake()
    {
        itemSpriteRenderer = itemPrefab.GetComponent<SpriteRenderer>();
    }
    
    public bool AddItemToInventory(ItemClass item, int amount)
    {
        for (int i = 0; i < inventory.Length; i++)
        {
            var slot = inventory[i];
            GameObject itemQuantityText = inventorySlots[i].transform.GetChild(0).gameObject;

            if (slot != null && slot.item == item)
            {
                slot.quantity += amount;
                itemQuantityText.GetComponent<TextMeshProUGUI>().text = slot.quantity.ToString();
                return true;
            }

            if (slot != null && slot.item != null) continue;
            inventory[i] = new InventorySlot(item, amount);
            inventorySlots[i].gameObject.SetActive(true);
            inventorySlots[i].GetComponent<Image>().sprite = item.itemIcon;
            itemQuantityText.SetActive(true);
            itemQuantityText.GetComponent<TextMeshProUGUI>().text = amount.ToString();

            return true;
        }

        // Add full functionality later
        Debug.Log("Inventory is full");
        return false;
    }

    public void RemoveItemFromInventory(int itemIndex, int amount)
    {
        InventorySlot slot = inventory[itemIndex];
        if (slot == null || slot.quantity == 0) return;
        
        TextMeshProUGUI itemQuantityText = inventorySlots[itemIndex].transform.GetChild(0).GetComponent<TextMeshProUGUI>();
    
        slot.quantity -= amount;
    
        if (slot.quantity <= 0)
        {
            inventorySlots[itemIndex].gameObject.SetActive(false);
            inventory[itemIndex] = null;
            itemQuantityText.gameObject.SetActive(false);
            inventorySlots[itemIndex].GetComponent<Image>().sprite = null;
            
            return;
        }
    
        itemQuantityText.text = slot.quantity.ToString();
    }
    // Carson 2 4 2026
    public void TaxItems(float percent)
    {
        for (int i = 0; i < inventory.Length; i++)
        {
            InventorySlot slot = inventory[i];
            if (slot == null || slot.quantity == 0) continue;

            int taxAmount = Mathf.CeilToInt(slot.quantity * (percent / 100f));
            slot.quantity -= taxAmount;

            if (slot.quantity < 0)
                slot.quantity = 0;
        }
    }

    public void OnSlotClicked(InputAction.CallbackContext context)
    {
        if (!context.started) return;

        int slotIndex = int.Parse(context.control.name) - 1;
        
        if (slotIndex < 0 || slotIndex >= inventory.Length + 1)
        {
            Debug.LogWarning("Invalid slot index.");
            return;
        }

        if (currentlySelectedSlot != slotIndex)
        {
            hotbarSelect.transform.position = new Vector3(inventorySlots[slotIndex].transform.position.x, hotbarSelect.transform.position.y, hotbarSelect.transform.position.z);
            hotbarSelect.SetActive(true);
            currentlySelectedSlot = slotIndex;
            currentEquippedItemType = SelectedItemType.Mineral;
            displaySelect.SetActive(false);
        }
        else
        {
            hotbarSelect.SetActive(false);
            currentlySelectedSlot = -1;
            currentEquippedItemType = SelectedItemType.None;
        }
    }


    // Move to player inventory subclass
    // public void Drop(InputAction.CallbackContext context)
    // {
    //     if (context.started)
    //     {
    //         DropItem();
    //     }
    // }
    
    private void DropItem()
    {
        // Probably could combine these in the future
        ItemType itemTypeToDrop = currentEquippedItemType switch
        {
            SelectedItemType.Mineral => ItemType.None,
            _ => ItemType.None
        };

        switch (itemTypeToDrop)
        {
            case ItemType.None:
                Drop();
                break;
            default:
                Debug.LogWarning("Can't drop invalid type!");
                break;
        }
    }

    // TODO: Make into general items in the future 
    private void Drop()
    {
        if(inventory[currentlySelectedSlot] == null || inventory[currentlySelectedSlot].quantity <= 0) return;

        //CreateOreObject();
        RemoveItemFromInventory(currentlySelectedSlot, inventory[currentlySelectedSlot].quantity);
    }

    /*
    private GameObject CreateOreObject()
    {
        var oreGameObject = new GameObject("Dropped Mineral")
        {
            tag = "Mineral"
        };

        var mineral = (ItemClass)inventory[currentlySelectedSlot].item;

        var oreSpriteRenderer = oreGameObject.AddComponent<SpriteRenderer>();
        oreSpriteRenderer.sprite = mineral.itemIcon;
        oreSpriteRenderer.sortingOrder = 4;

        float scale = UnityEngine.Random.Range(mineral.minimumSize, mineral.maximumSize);
        oreGameObject.transform.localScale = new Vector3(scale, scale, 1);

        oreGameObject.transform.position = transform.position;
        oreGameObject.transform.SetParent(transform);
        var itemObject = oreGameObject.AddComponent<ItemObject>();
        itemObject.item = mineral;
        itemObject.quantity = inventory[currentlySelectedSlot].quantity;

        oreGameObject.transform.parent = null;

        var rb = oreGameObject.GetComponent<Rigidbody2D>();
        var oreCollider = oreGameObject.GetComponent<BoxCollider2D>();

        if (!rb)
        {
            rb = oreGameObject.AddComponent<Rigidbody2D>();
        }

        if (!oreCollider)
        {
            oreCollider = oreGameObject.AddComponent<BoxCollider2D>();
        }

        oreCollider.isTrigger = true;

        rb.gravityScale = 0f;
        rb.linearDamping = 3f;
        rb.angularDamping = 5f;

        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;

        Vector2 randomDirection = UnityEngine.Random.insideUnitCircle.normalized;
        float forceMagnitude = UnityEngine.Random.Range(3f, 7f);
        rb.AddForce(randomDirection * forceMagnitude, ForceMode2D.Impulse);

        float randomTorque = UnityEngine.Random.Range(-5f, 5f);
        rb.AddTorque(randomTorque, ForceMode2D.Impulse);

        return oreGameObject;
    }
*/
}