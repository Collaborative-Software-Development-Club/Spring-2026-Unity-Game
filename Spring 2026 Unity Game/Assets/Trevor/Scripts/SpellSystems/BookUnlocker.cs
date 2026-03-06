using UnityEngine;
using UnityEngine.Events; // Required for UnityEvent

public class BookUnlocker : MonoBehaviour
{
    [Header("Data Settings")]
    [SerializeField] private PlayerProgress collectionData;
    [SerializeField] private LevelID collectionID;

    [Header("Behavior Settings")]
    [Tooltip("Check this if the player should physically walk into a trigger to unlock the book.")]
    [SerializeField] private bool unlockOnPlayerCollision = true;

    [Header("Events")]
    [Tooltip("Fires when the book unlocks. Great for playing particles, sounds, or calling a 'Return To Hub' script.")]
    public UnityEvent onBookUnlocked;

    private bool hasUnlocked = false; // Prevents double-triggering if called multiple times

    // Option 1: Physical Trigger (3D Objects)
    private void OnTriggerEnter(Collider other)
    {
        if (unlockOnPlayerCollision && !hasUnlocked && other.CompareTag("Player"))
        {
            Unlock();
        }
    }

    // Option 2: Direct Call (Puzzle Managers, UI, etc.)
    public void Unlock()
    {
        // Prevent unlocking twice in the same frame
        if (hasUnlocked) return;
        hasUnlocked = true;

        if (collectionData != null && collectionID != LevelID.None)
        {
            collectionData.UnlockBook(collectionID);
        }
        else
        {
            Debug.LogWarning("BookUnlocker: Missing PlayerProgress data or LevelID is set to None!", gameObject);
        }

        // Trigger any sounds, particles, or level transitions hooked up in the Inspector
        onBookUnlocked?.Invoke();

        // Optional: If you have visual parts (like a mesh), you might want to disable them 
        // instead of destroying the whole object immediately if a sound needs time to play.
        Destroy(gameObject);
    }
}