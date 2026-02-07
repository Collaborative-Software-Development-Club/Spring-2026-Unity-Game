using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneReturner : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private string librarySceneName = "MageLibrary"; // Exact name of your main scene

    // Call this via a trigger or a UI button
    public void ReturnToLibrary()
    {
        if (!string.IsNullOrEmpty(librarySceneName))
        {
            Debug.Log($"<color=cyan>Scene System:</color> Returning to {librarySceneName}...");
            SceneManager.LoadScene(librarySceneName);
        }
        else
        {
            Debug.LogError("Library Scene Name is not set in the inspector!");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ReturnToLibrary();
        }
    }
}