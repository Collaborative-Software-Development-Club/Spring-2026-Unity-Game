using System;
using UnityEngine;
using UnityEngine.Assertions;

/// <summary>
/// Handles the economy with operations like adding and removing money
/// </summary>
public class EconomyManager : MonoBehaviour
{
    private double money = 0;
    public Action<double> onMoneyChanged;
    
    /// <summary>
    /// Adds the amount of money passed in. 
    /// </summary>
    /// <param name="amount">Required to be 0 or bigger</param>
    public void AddMoney(int amount)
    { 
        Assert.IsTrue(amount > 0);
        ChangeMoney(amount);
    }
    
    /// <summary>
    /// Removes the amount of money passed in. 
    /// </summary>
    /// <param name="amount">Required to be 0 or bigger</param>
    public void RemoveMoney(int amount)
    {
        Assert.IsTrue(amount > 0);
        ChangeMoney(-amount);
    }

    /// <summary>
    /// Takes in a amount of money and modifies the players money with the amount.
    /// </summary>
    /// <param name="amount">Amount of money to change</param>
    private void ChangeMoney(int amount)
    {
        money += amount;
        onMoneyChanged?.Invoke(amount);
    }

    /// <summary>
    /// Gets the amount of money the player has.
    /// </summary>
    /// <returns>Money</returns>
    public double GetMoney()
    {
        return money;
    }
}
