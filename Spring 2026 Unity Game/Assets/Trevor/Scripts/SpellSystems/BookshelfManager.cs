
using UnityEngine;
using System.Collections.Generic;

public class BookshelfManager : MonoBehaviour
{
    [SerializeField] private PlayerProgress progressData;

    [System.Serializable]
    public struct BookVisual
    {
        public LevelID id;
        public GameObject bookModel;
    }

    [Tooltip("Map each Level ID to its specific 3D model on the shelf.")]
    public List<BookVisual> libraryShelf;

    private void OnEnable()
    {
        UpdateShelf();
        if (progressData != null)
            progressData.OnCollectionUpdated += UpdateShelf;
    }

    private void OnDisable()
    {
        if (progressData != null)
            progressData.OnCollectionUpdated -= UpdateShelf;
    }

    public void UpdateShelf()
    {
        if (progressData == null) return;

        foreach (var book in libraryShelf)
        {
            if (book.bookModel != null)
            {
                // Turn the model on if it's in the unlocked list, off if it isn't
                bool isUnlocked = progressData.IsUnlocked(book.id);
                book.bookModel.SetActive(isUnlocked);
            }
        }
    }
}