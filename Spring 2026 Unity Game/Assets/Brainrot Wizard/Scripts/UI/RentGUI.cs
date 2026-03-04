using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RentGUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI amountDueText;
    [SerializeField] private Button continueButton;
    [SerializeField] private GameObject panel;
    
    [SerializeField] private string sceneToLoad = "MainGameMenu";

    public void OpenUI(bool passed)
    {
       panel.SetActive(true);
       if (passed)
           continueButton.GetComponentInChildren<TextMeshProUGUI>().text = "Passed!";
       else
           continueButton.GetComponentInChildren<TextMeshProUGUI>().text = "Homeless!";
       
       SceneReturner sceneReturner = FindFirstObjectByType<SceneReturner>();
       continueButton.onClick.AddListener(() =>
       {
           SceneManager.LoadScene(sceneToLoad);
       });
;
    }

    public void CloseUI()
    {
        panel.SetActive(false);
    }
}
