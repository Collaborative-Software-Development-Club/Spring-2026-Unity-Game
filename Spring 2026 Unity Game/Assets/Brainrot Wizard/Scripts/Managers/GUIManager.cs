using UnityEngine;


public class GUIManager : MonoBehaviour
{
        
    private GameManager gM;
    public Canvas canvas;

    public GameObject mainObject;
    public GameObject MainGUIPrefab;
    public MainGUI main;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gM = GameManager.Instance;
        gM.EconomyManager.onMoneyChanged += UpdateMoneyGUI;
        gM.onTurnChange += UpdateTurnGUI;

        mainObject = GameObject.Instantiate(MainGUIPrefab);
        main = mainObject.GetComponent<MainGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateMoneyGUI(double data) {
        main.MoneyText.text = main.Money + data;
    }
    public void UpdateContractGUI(double data) {
        main.ContractText.text = main.Contract + data;
    }
    public void UpdateStateGUI(double data) {
        main.StateText.text = main.State + data;
    }
    public void UpdateTurnGUI(int data) {
        main.TurnText.text = main.Turn + data;
    }
}
