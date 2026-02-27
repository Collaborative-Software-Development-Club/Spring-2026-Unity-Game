using UnityEngine;
using TMPro;

public class MainGUI : MonoBehaviour
{
    public TMPro.TextMeshProUGUI MoneyText;
    public TMPro.TextMeshProUGUI ContractText;
    public TMPro.TextMeshProUGUI StateText;
    public TMPro.TextMeshProUGUI TurnText;
    public TMPro.TextMeshProUGUI PowerText;

    public GUIManager MyGUIManager;

    public string Money = "Monie: ";
    public string Contract = "Current contract: ";
    public string State = "GameState: ";
    public string Turn = "Turn: ";
    public string Power = "Power: ";


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ForceNext() { // cheat to the next game state
        GameState current = MyGUIManager.gM.CurrentGameState;
        MyGUIManager.gM.NextState();
        print("Forced GUI Game State change from " + current + " to " + MyGUIManager.gM.CurrentGameState);
    }
}
