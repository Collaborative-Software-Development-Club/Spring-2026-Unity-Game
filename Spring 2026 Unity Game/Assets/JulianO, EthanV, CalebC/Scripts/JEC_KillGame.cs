using UnityEngine;
using UnityEngine.SceneManagement;

public class JEC_KillGame : MonoBehaviour
{
    [SerializeField] private string HubSceneName = "MainGameMenu";

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene(HubSceneName);
        }    
    }
}
