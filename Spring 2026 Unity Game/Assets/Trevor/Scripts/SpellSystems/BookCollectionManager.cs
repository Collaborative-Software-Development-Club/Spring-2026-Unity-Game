using UnityEngine;
using System.Collections.Generic;

public class BookCollectionManager : MonoBehaviour
{
    [SerializeField] private SpellBookCollection collectionData;

    [System.Serializable]
    public struct BookMapping
    {
        public string bookID;
        public GameObject bookModel;
    }

    [Tooltip("Map each Book ID to its specific 3D model on the shelf.")]
    public List<BookMapping> libraryShelf;

    private void OnEnable()
    {
        // Refresh when the scene loads
        UpdateShelf();
        // Also refresh if the data changes while we are in the scene
        collectionData.OnCollectionUpdated += UpdateShelf;
    }

    private void OnDisable()
    {
        collectionData.OnCollectionUpdated -= UpdateShelf;
    }

    public void UpdateShelf()
    {
        foreach (var mapping in libraryShelf)
        {
            if (mapping.bookModel != null)
            {
                // If the list of IDs contains this mapping's ID, show the book
                bool isUnlocked = collectionData.unlockedBookIDs.Contains(mapping.bookID);
                mapping.bookModel.SetActive(isUnlocked);
            }
        }
    }
}