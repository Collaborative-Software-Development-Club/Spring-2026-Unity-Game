using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInventory : MonoBehaviour
{
   private Inventory _inventory = new(9);
   [SerializeField] private PlayerInventoryUI playerInventoryUI;

   public void OnSlotClicked(InputAction.CallbackContext context)
   { 
       if (!context.started) return;

       int slotIndex = int.Parse(context.control.name) - 1;
        
       if (slotIndex < 0 || slotIndex >= _inventory.Length + 1)
       {
           Debug.LogWarning("Invalid slot index.");
           return;
       }

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
      List<int> removeResult = _inventory.RemoveItemFromInventory(item, amount);

      foreach (int i in removeResult)
      {
          playerInventoryUI.UpdateQuantityTextForIndex(i, _inventory.GetItemAt(i).quantity);
          playerInventoryUI.UpdateIconForIndex(i, item.GetIcon());
      }
   }
}
