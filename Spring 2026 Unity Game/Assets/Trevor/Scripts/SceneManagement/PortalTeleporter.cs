using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalTeleporter : MonoBehaviour
{
    public SceneBucket gameplayScenes;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            string nextScene = gameplayScenes.GetRandomScene();
            if (!string.IsNullOrEmpty(nextScene))
            {
                SceneManager.LoadScene(nextScene);
            }
            else
            {
                Debug.LogError("Scene Bucket is empty! Add scenes to the ScriptableObject.");
            }
        }
    }
}