using UnityEngine;

public class CollectionResetter : MonoBehaviour
{
    [Header("Data Reference")]
    [SerializeField] private SpellBookCollection collectionData;

    [Header("Visual Reference")]
    [Tooltip("Drag the Bookshelf object here to update the shelf instantly when pressing C.")]
    [SerializeField] private BookCollectionManager shelfManager;

    private void Update()
    {
        // Check for the 'C' key press using the Legacy Input System
        if (Input.GetKeyDown(KeyCode.C))
        {
            ResetCollection();
        }
    }

    private void ResetCollection()
    {
        if (collectionData != null)
        {
            collectionData.unlockedBookIDs.Clear();
            Debug.Log("<color=red><b>[DEBUG]:</b></color> Spell Book Collection Cleared!");

            // If we have a reference to the shelf, tell it to refresh its visuals
            if (shelfManager != null)
            {
                shelfManager.UpdateShelf();
            }
        }
        else
        {
            Debug.LogWarning("CollectionResetter: No SpellBookCollection assigned!");
        }
    }
}