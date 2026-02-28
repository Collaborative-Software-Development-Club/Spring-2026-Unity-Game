using System;
using UnityEngine;

public class RentManager : MonoBehaviour
{
    private int currentRentStage = 0;
    private int nextRentDueTurn = 0;

    [SerializeField] private float rentGrowthMultiplier = 1.5f;
    [SerializeField] private int baseRent = 100;
    [SerializeField] private int baseDuration = 100;
    [SerializeField] private int minimumRentDuration = 5;
    
    private void Start()
    {
        nextRentDueTurn = CalculateRentDuration(currentRentStage);
        GameManager.Instance.onTurnChange += CheckIfRentIsDue;
    }

    /// <summary>
    /// Resets all the rent variables
    /// </summary>
    public void ResetRent()
    {
        currentRentStage = 0;
        nextRentDueTurn = CalculateRentDuration(currentRentStage);
    }
    
    /// <summary>
    /// Charges a certain amount of money if the rent is due
    /// </summary>
    /// <param name="currentTurn">The current turn to check</param>
    private void ChargeRent(int currentTurn)
    {
        int rentAmount = CalculateRentAmount(currentRentStage);
        GameManager.Instance.EconomyManager.RemoveMoney(rentAmount);

        if (GameManager.Instance.EconomyManager.GetMoney() >= 0)
        {
            currentRentStage++;
            nextRentDueTurn = currentTurn + CalculateRentDuration(currentRentStage);
        }
        else
            // Fail game state goes here later
            Debug.Log("Game over!");
    }

    /// <summary>
    /// Checks if the rent is due every turn
    /// </summary>
    /// <param name="currentTurn">The current turn the game is on</param>
    private void CheckIfRentIsDue(int currentTurn)
    {
        if (currentTurn >= nextRentDueTurn)
            ChargeRent(currentTurn);
    }

    /// <summary>
    /// Gets the total amount of money due at the end of the stage. 
    /// </summary>
    /// <param name="stage">Current stage of rent</param>
    /// <returns></returns>
    private int CalculateRentAmount(int stage)
    {
        return baseRent * Mathf.RoundToInt(Mathf.Pow(rentGrowthMultiplier, stage));
    }

    /// <summary>
    /// Calculates the total amount of turns before the current stage ends
    /// </summary>
    /// <param name="stage">Current stage of rent</param>
    /// <returns></returns>
    private int CalculateRentDuration(int stage)
    {
        return Mathf.RoundToInt(baseDuration / (stage + 7)) + minimumRentDuration;
    }

    private void OnDestroy()
    {
        if(GameManager.Instance != null)
            GameManager.Instance.onTurnChange -= CheckIfRentIsDue;
    }
}
