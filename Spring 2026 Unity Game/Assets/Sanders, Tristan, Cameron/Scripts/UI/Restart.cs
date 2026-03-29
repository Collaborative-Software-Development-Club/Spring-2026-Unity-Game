using UnityEngine;
using UnityEngine.SceneManagement; // Required for scene management

public class Restart : MonoBehaviour
{
    [Header("Scene Names")]
    [SerializeField] private string _mainMenuSceneName = "MainMenu";

    [Header("Input")]
    [SerializeField] private bool _enableEscapeToMenu = true;

    private void Update()
    {
        if (!_enableEscapeToMenu)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GoToMainMenu();
        }
    }

    // Call this method to restart the current scene
    public void RestartGame()
    {
        // Reloads the currently active scene by its name
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // Call this from a UI button to go back to the main menu scene.
    public void GoToMainMenu()
    {
        if (string.IsNullOrWhiteSpace(_mainMenuSceneName))
        {
            Debug.LogWarning("Main menu scene name is empty.");
            return;
        }

        SceneManager.LoadScene(_mainMenuSceneName);
    }
}