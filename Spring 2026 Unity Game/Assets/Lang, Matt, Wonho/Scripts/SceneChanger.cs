using UnityEngine;
using UnityEngine.SceneManagement; // Required for changing scenes

public class SceneChanger : MonoBehaviour
{
    public int currentLevel;
    [SerializeField] private string sceneToLoad;
    [SerializeField] private string main = "MainGameMenu";
    public void Start()
    {
        switch (currentLevel)
        {
            case 1:
                sceneToLoad = "Medium Level 1";
                break;
            case 2:
                sceneToLoad = "Medium Level 2";
                break;
            case 3:
                sceneToLoad = "Hard Level 1";
                break;
            case 4:
                sceneToLoad = "Hard Level 2";
                break;
            case 5:
                sceneToLoad = "MainGameMenu";
                break;
            default:
                Debug.Log("Level not Found");
                break;
        }
    }
    void Update()
    {
        // Check if the Escape key was pressed this frame
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene(main);
        }
    }

    public void LoadTargetScene()
    {
        SceneManager.LoadScene(sceneToLoad);
    }
}