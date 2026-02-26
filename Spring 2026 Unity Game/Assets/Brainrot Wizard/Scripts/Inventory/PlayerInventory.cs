using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInventory : MonoBehaviour
{
   private Inventory inventory = new(9);
   [SerializeField] private PlayerInventoryUI playerInventoryUI;

   public void OnSlotClicked(InputAction.CallbackContext context)
   { 
       if (!context.started) return;

       var slotIndex = int.Parse(context.control.name) - 1;
        
       if (slotIndex < 0 || slotIndex >= inventory.Length + 1)
       {
           Debug.LogWarning("Invalid slot index.");
           return;
       }

       playerInventoryUI.HighlightInventorySlot(slotIndex);
   }
}
