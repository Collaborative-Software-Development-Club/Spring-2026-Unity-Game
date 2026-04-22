using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class ReturnToMainMenu : MonoBehaviour
{
    [SerializeField] private InputReader inputReader;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ReturnToMenu();
            inputReader.Actions.Player.Move.RemoveAllBindingOverrides();
        }
    }


    void ReturnToMenu()
    {
        PlayerInv.water = 0;
        SceneManager.LoadScene("MainGameMenu");
    }

}
