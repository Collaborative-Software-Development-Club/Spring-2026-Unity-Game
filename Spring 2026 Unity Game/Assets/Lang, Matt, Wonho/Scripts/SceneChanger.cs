using UnityEngine;
using UnityEngine.SceneManagement; // Required for changing scenes

public class SceneChanger : MonoBehaviour
{
    [SerializeField] private string sceneToLoad = "MainGameMenu";

    void Update()
    {
        // Check if the Escape key was pressed this frame
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            LoadTargetScene();
        }
    }

    public void LoadTargetScene()
    {
        SceneManager.LoadScene(sceneToLoad);
    }
}