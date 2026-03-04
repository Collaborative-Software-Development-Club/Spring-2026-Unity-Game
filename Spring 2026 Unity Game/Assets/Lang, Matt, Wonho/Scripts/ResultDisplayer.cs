using UnityEngine;

public class ResultDisplayer : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject win;
    public GameObject lose;
    public GameObject clickBlock;
    void Start()
    {
        ContinueAfterLoss(); 
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void WinScreen()
    {
        clickBlock.SetActive(true);
        win.SetActive(true);
        lose.SetActive(false);

    }
    public void LoseScreen()
    {
        clickBlock.SetActive(true);
        win.SetActive(false);
        lose.SetActive(true);
    }
    public void ContinueAfterLoss()
    {
        clickBlock.SetActive(false);
        win.SetActive(false);
        lose.SetActive(false);
    }
}
