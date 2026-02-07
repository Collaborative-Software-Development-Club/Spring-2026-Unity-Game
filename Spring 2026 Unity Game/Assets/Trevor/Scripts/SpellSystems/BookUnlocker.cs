using UnityEngine;

public class BookUnlocker : MonoBehaviour
{
    [SerializeField] private SpellBookCollection collectionData;
    [SerializeField] private string bookID; // Give each book a unique name

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            collectionData.UnlockBook(bookID);
            // Optional: Play a sound or particle effect
            Destroy(gameObject);
        }
    }
}