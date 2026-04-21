using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnToMainMenu : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ReturnToMenu();
        }
    }


    void ReturnToMenu()
    {
        PlayerInv.water = 0;
        SceneManager.LoadScene("MainGameMenu");
    }

}
