using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInventory : MonoBehaviour
{
   private Inventory _inventory = new(9);
   [SerializeField] private PlayerInventoryUI playerInventoryUI;
   private int selectedSlot;

   public Loottable testItem;

   private void Start()
   {
       AddItemToInventory(new Lootbox(testItem), 5);
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
       
       playerInventoryUI.UpdateItem(slotIndex, item);
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
               playerInventoryUI.UpdateItem(inventoryChange.Index, null);
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

   public void RemoveItemFromSlot(int index, int amount)
   {
       InventorySlot removeResult = _inventory.RemoveFromSlot(index,amount);
       
       if (removeResult.item == null || removeResult.quantity <= 0)
       {
           playerInventoryUI.UpdateItem(index, null);
           playerInventoryUI.UpdateQuantityTextForIndex(index, 0); 
           playerInventoryUI.UpdateIconForIndex(index, null);
       }
       else
       {
           playerInventoryUI.UpdateQuantityTextForIndex(index, _inventory.slots[index].quantity);
           playerInventoryUI.UpdateIconForIndex(index, removeResult.item.GetIcon());
       }
   }
   
   //Implement later
   //public void DragToOpenContainer();

   private bool _transferringTo;
   
   public void OnQuickTransferTo(InputAction.CallbackContext context)
   {
       if (context.started && !_transferringTo)
       {
           _transferringTo = true;
           QuickTransferToContainer(selectedSlot, 1);
       }

       if (context.canceled)
       {
           _transferringTo = false;
       }
   }

   private bool _transferringFrom;
   public void OnQuickTransferFrom(InputAction.CallbackContext context)
   {
       
       if (context.started && !_transferringFrom)
       {
           _transferringFrom = true;
           QuickTransferFromContainer();
       }

       if (context.canceled)
       {
           _transferringFrom = false;
       }
   }
   public void QuickTransferToContainer(int index, int quantity)
   {
       Machine machineRef = GameManager.Instance.GUIManager.MachineUIRef.CurrentMachine;
       if (machineRef == null) return;
       
       machineRef.AddItemToInput(_inventory.slots[index].item, quantity);
       RemoveItemFromSlot(index, quantity);
   }

   // Bandid solution do better later lol
   public void QuickTransferFromContainer()
   {
       Machine machineRef = GameManager.Instance.GUIManager.MachineUIRef.CurrentMachine;
       if (machineRef == null) return;

       for (int i = 0; i < machineRef.GetOutputInventory().slots.Length; i++)
       {
           InventorySlot slot = machineRef.GetOutputInventory().slots[i];

           if (slot.item == null || slot.quantity <= 0) return;
           
           AddItemToInventory(slot.item, slot.quantity);
           machineRef.RemoveItemFromOutput(i, slot.quantity);
       }
   }

   public void OnOpenLootbox(InputAction.CallbackContext context)
   {
       if (_inventory.GetItemAt(selectedSlot).item is not Lootbox lootbox || !context.canceled) return;
       
       GameManager.Instance.GUIManager.ShowLootboxUI(lootbox);
       RemoveItemFromSlot(selectedSlot, 1);
   }
}
