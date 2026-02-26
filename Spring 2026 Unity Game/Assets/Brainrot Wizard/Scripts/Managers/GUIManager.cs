using UnityEngine;


public class GUIManager : MonoBehaviour
{
        
    private GameManager gM;
    public GameObject mainObject;
    public GameObject MainGUIPrefab;
    public MainGUI main;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mainObject = GameObject.Instantiate(MainGUIPrefab);
        main = mainObject.GetComponent<MainGUI>();

        gM = GameManager.Instance;
        print(gM.EconomyManager);
        gM.EconomyManager.onMoneyChanged += UpdateMoneyGUI;
        gM.onTurnChange += UpdateTurnGUI;
        gM.onGameStateChange += UpdateStateGUI;
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
    public void UpdateStateGUI(GameState data) {
        main.StateText.text = main.State + data;
    }
    public void UpdateTurnGUI(int data) {
        main.TurnText.text = main.Turn + data;
    }
}
