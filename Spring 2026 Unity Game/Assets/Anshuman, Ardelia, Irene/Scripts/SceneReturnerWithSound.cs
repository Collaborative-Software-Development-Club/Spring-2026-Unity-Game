using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneReturnerWithSound : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private string librarySceneName = "MageLibrary"; // Exact name of your main scene

    [SerializeField] private AudioSource audioSource;

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
    IEnumerator DelayAction(float delay) {
        audioSource.Play();
        yield return new WaitForSeconds(delay);
        ReturnToLibrary();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) {
            StartCoroutine(DelayAction(2.0f));
        }
    }
}