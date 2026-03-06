using UnityEngine;

public class CollectSpell : MonoBehaviour
{
    [SerializeField] private SpellBookCollection books;
    [SerializeField] private GameObject book;
    
    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            books.UnlockBook("DampSpell");
            Destroy(book);
        }
    }
}
