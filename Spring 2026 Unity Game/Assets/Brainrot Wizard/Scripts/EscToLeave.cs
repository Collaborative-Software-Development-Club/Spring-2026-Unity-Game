using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class EscToLeave : MonoBehaviour
{
    [SerializeField] private string librarySceneName = "MainGameMenu";
    public void OnLeaveClicked(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        
        SceneManager.LoadScene(librarySceneName);
        
        Destroy(gameObject);
        Destroy(GameManager.Instance.gameObject);
    }
}
