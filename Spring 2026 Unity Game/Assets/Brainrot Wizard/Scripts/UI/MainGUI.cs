using System;
using UnityEngine;
using TMPro;

public class MainGUI : MonoBehaviour
{
    public TextMeshProUGUI MoneyText;
    public TextMeshProUGUI StateText;
    public TextMeshProUGUI TurnText;

    private const string MoneyDisplayText = "Money: ";
    private const string StateDisplayText = "GameState: ";
    private const string TurnDisplayText = "Turn: ";

    private void Start()
    {
        UpdateMoneyGUI(GameManager.Instance.EconomyManager.GetMoney());
        UpdateStateGUI(GameManager.Instance.CurrentGameState);
        UpdateTurnGUI(GameManager.Instance.CurrentTurnCount);
        
        GameManager.Instance.EconomyManager.onMoneyChanged += UpdateMoneyGUI;
        GameManager.Instance.onTurnChange += UpdateTurnGUI;
        GameManager.Instance.onGameStateChange += UpdateStateGUI;
    }


    /// <summary>
    /// Forces the game state to the next state.
    /// </summary>
    public void ForceNext() { // cheat to the next game state
        GameState oldGameState = GameManager.Instance.CurrentGameState;
        GameManager.Instance.NextState();
        print("Forced GUI Game State change from " + oldGameState + " to " + GameManager.Instance.CurrentGameState);
    }
    
    /// <summary>
    /// Updates the text for the money to the amount passed as a parameter.
    /// </summary>
    /// <param name="amount">The amount to update the text to.</param>
    public void UpdateMoneyGUI(double amount) {
        MoneyText.text = MoneyDisplayText + amount;
    }
    /// <summary>
    /// Updates the text for the game state to the state passed as a parameter.
    /// </summary>
    /// <param name="gameState">The state to update the text to.</param>
    public void UpdateStateGUI(GameState gameState) {
        StateText.text = StateDisplayText + gameState;
    }
    /// <summary>
    /// Updates the text for the turn to the turn passed as a parameter. 
    /// </summary>
    /// <param name="turn">The turn to update the text to.</param>
    public void UpdateTurnGUI(int turn) {
        TurnText.text = TurnDisplayText + turn;
    }

    /// <summary>
    /// Shows the all the main gui
    /// </summary>
    public void Show()
    {
        gameObject.SetActive(true);
    }
    /// <summary>
    /// Hides all the main gui 
    /// </summary>
    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
