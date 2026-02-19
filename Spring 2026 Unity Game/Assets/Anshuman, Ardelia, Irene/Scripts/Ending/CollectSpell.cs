using UnityEngine;

public class CollectSpell : MonoBehaviour
{
    [SerializeField] private InputReader inputReader;
    [SerializeField] private GameObject popupPrompt;
    [SerializeField] private SpellBookCollection books;
    [SerializeField] private GameObject book;
    
    private bool isPlayerNearby = false;

    private void Awake() {
        popupPrompt.SetActive(false);
    }

    private void OnEnable() {
        inputReader.InteractEvent += collectInteraction;
    }

    private void OnDisable() {
        inputReader.InteractEvent -= collectInteraction;
    }
    private void collectInteraction() {
        books.UnlockBook("DampSpell");
        Destroy(book);
    }
    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            isPlayerNearby = true;
            popupPrompt.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.CompareTag("Player")) {
            isPlayerNearby = false;
            popupPrompt.SetActive(false);
        }
    }
}
