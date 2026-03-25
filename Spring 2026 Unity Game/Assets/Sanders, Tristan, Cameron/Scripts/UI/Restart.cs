using UnityEngine;
using UnityEngine.SceneManagement; // Required for scene management

public class Restart : MonoBehaviour
{
    // Call this method to restart the current scene
    public void RestartGame()
    {
        // Reloads the currently active scene by its name
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}