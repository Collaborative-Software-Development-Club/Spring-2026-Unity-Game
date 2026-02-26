using UnityEngine;


public class GUIManager : MonoBehaviour
{
        
    private GameManager gM;
    public Canvas canvas;

    [SerializeField]
    public MoneyGUI moneyGUI;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gM = GameManager.Instance;
        gM.EconomyManager.onMoneyChanged += UpdateMoneyGUI;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void UpdateMoneyGUI(double data) {

    }
}
