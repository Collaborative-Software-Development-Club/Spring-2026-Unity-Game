using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInventory : MonoBehaviour
{
   private Inventory _inventory = new(9);
   [SerializeField] private PlayerInventoryUI playerInventoryUI;
   private int selectedSlot;

   public Item TestItem;
   
   private void Start()
   {
       AddItemToInventory(TestItem, 10);
   }


   public void OnSlotClicked(InputAction.CallbackContext context)
   { 
       if (!context.started) return;

       int slotIndex = int.Parse(context.control.name) - 1;
        
       if (slotIndex < 0 || slotIndex >= _inventory.Length + 1)
       {
           Debug.LogWarning("Invalid slot index.");
           return;
       }

       selectedSlot = slotIndex;
       playerInventoryUI.HighlightInventorySlot(slotIndex);
   }

   public void AddItemToInventory(Item item, int amount)
   {
       int slotIndex = _inventory.AddItemToInventory(item, amount);
       
       playerInventoryUI.UpdateQuantityTextForIndex(slotIndex, _inventory.GetItemAt(slotIndex).quantity);
       playerInventoryUI.UpdateIconForIndex(slotIndex, item.GetIcon());
   }

   public void RemoveItemFromInventory(Item item, int amount)
   {
       List<InventoryChange> removeResult = _inventory.RemoveItemFromInventory(item, amount);
        print(removeResult.Count);
       
       
       foreach (InventoryChange inventoryChange in removeResult)
       {
           InventorySlot slot = _inventory.GetItemAt(inventoryChange.Index);

           if (slot.item == null || slot.quantity <= 0)
           {
               playerInventoryUI.UpdateQuantityTextForIndex(inventoryChange.NewQuantity, 0);
               playerInventoryUI.UpdateIconForIndex(inventoryChange.Index, null);
           }
           else
           {
               playerInventoryUI.UpdateQuantityTextForIndex(inventoryChange.NewQuantity, slot.quantity);
               playerInventoryUI.UpdateIconForIndex(inventoryChange.Index, slot.item.GetIcon());
           }
       }
   }
   
   //Implement later
   //public void DragToOpenContainer();

   private bool _transferring;
   
   public void OnQuickTransfer(InputAction.CallbackContext context)
   {
       if (context.started && !_transferring)
       {
           _transferring = true;
           QuickTransferToContainer(selectedSlot, 1);
       }

       if (context.canceled)
       {
           _transferring = false;
       }
   }

   public void QuickTransferToContainer(int index, int quantity)
   {
       Machine machineRef = GameManager.Instance.GUIManager.MachineUIRef.CurrentMachine;
       if (machineRef == null) return;

       machineRef.AddItemToInput(_inventory.slots[index].item, quantity);
       RemoveItemFromInventory(_inventory.slots[index].item, quantity);
   }
}
