using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // Required for the Image component

public class PortalTeleporter : MonoBehaviour
{
    [Header("Data References")]
    public SceneBucket gameplayScenes;

    [Header("Transition Settings")]
    [SerializeField] private Image fadeOverlay; // Drag your black UI Image here
    [SerializeField] private float fadeDuration = 1f;
    [SerializeField] private AudioClip teleportSFX;

    private bool isTeleporting = false;

    private void Start()
    {
        // Ensure the screen is clear when the scene starts
        if (fadeOverlay != null)
        {
            Color startColor = fadeOverlay.color;
            startColor.a = 0f;
            fadeOverlay.color = startColor;
            fadeOverlay.gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isTeleporting)
        {
            string nextScene = gameplayScenes.GetRandomAvailableScene();

            if (!string.IsNullOrEmpty(nextScene))
            {
                // Play the sound effect using the manager we built earlier
                if (MAIN_SFXManager.Instance != null && teleportSFX != null)
                {
                    MAIN_SFXManager.Instance.PlaySFX(teleportSFX);
                }

                // Start the fade transition
                StartCoroutine(FadeAndLoadScene(nextScene));
            }
            else
            {
                Debug.LogError("Scene Bucket is empty! Add scenes to the ScriptableObject.");
            }
        }
    }

    private IEnumerator FadeAndLoadScene(string sceneName)
    {
        isTeleporting = true;

        if (fadeOverlay != null)
        {
            fadeOverlay.gameObject.SetActive(true);
            float elapsedTime = 0f;
            Color color = fadeOverlay.color;

            // Gradually increase the alpha of the black image to 1 (fully opaque)
            while (elapsedTime < fadeDuration)
            {
                elapsedTime += Time.deltaTime;
                color.a = Mathf.Clamp01(elapsedTime / fadeDuration);
                fadeOverlay.color = color;
                yield return null; // Wait until the next frame before continuing the loop
            }
        }
        else
        {
            // If you forgot to assign the image, just wait for the duration so the sound can play
            yield return new WaitForSeconds(fadeDuration);
        }

        // Load the next scene after the screen is completely black
        SceneManager.LoadScene(sceneName);
    }
}