using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetSceneDamp : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetLevel();
        }
    }


    void ResetLevel()
    {
        PlayerInv.water = 0;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}
