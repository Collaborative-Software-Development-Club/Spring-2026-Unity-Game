using UnityEngine;

/// <summary>
/// Handles the visual UI behavior for the player's inventory,
/// including slot highlighting and highlight toggling.
/// </summary>
public class PlayerInventoryUI : MonoBehaviour
{
    /// <summary>
    /// Array of inventory slot UI GameObjects.
    /// Each element represents a visual slot in the inventory.
    /// </summary>
    [SerializeField] private GameObject[] inventorySprites = new GameObject[9];

    /// <summary>
    /// The UI element used to visually indicate the currently selected slot.
    /// </summary>
    [SerializeField] private GameObject inventorySelectIcon;

    /// <summary>
    /// Returns true if the highlight icon is currently active.
    /// </summary>
    private bool IsHighlighting => inventorySelectIcon.activeSelf; 

    /// <summary>
    /// Stores the index of the currently highlighted inventory slot.
    /// Defaults to -1 when no slot is selected.
    /// </summary>
    private int highlightedSlotIndex = -1;

    /// <summary>
    /// Highlights the inventory slot at the given index.
    /// If the same slot is selected again, the highlight toggles on/off.
    /// </summary>
    /// <param name="index">The index of the inventory slot to highlight.</param>
    public void HighlightInventorySlot(int index)
    {
        if (highlightedSlotIndex == index)
        {
            if (IsHighlighting)
                HideHighlight();
            else
                ShowHighlight();
        }

        MoveHighlight(index);
        highlightedSlotIndex = index;
    }

    /// <summary>
    /// Moves the highlight icon to match the position of the
    /// inventory slot at the specified index.
    /// </summary>
    /// <param name="index">The inventory slot index to move the highlight to.</param>
    private void MoveHighlight(int index)
    {
        inventorySelectIcon.transform.position = new Vector3(
            inventorySprites[index].transform.position.x, 
            inventorySprites[index].transform.position.y, 
            inventorySprites[index].transform.position.z
        );
    }

    /// <summary>
    /// Activates the highlight icon, making it visible.
    /// </summary>
    private void ShowHighlight()
    {
        inventorySelectIcon.SetActive(true);
    }
    
    /// <summary>
    /// Deactivates the highlight icon, hiding it from view.
    /// </summary>
    private void HideHighlight()
    {
        inventorySelectIcon.SetActive(false);
    }
}