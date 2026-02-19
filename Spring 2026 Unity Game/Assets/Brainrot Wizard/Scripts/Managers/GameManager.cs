using System;
using UnityEngine;

public enum GameState 
{
    None,
    
    /// <summary>
    /// The player is taking on contracts, earning money, and selling stock to past clients.
    /// </summary>
    ContractWork,

    /// <summary>
    /// The player purchases brain-rot loot boxes.
    /// </summary>
    Shopping,
    
    /// <summary>
    /// The player is actively combining brain‑rots to create new ones.
    /// </summary>
    Crafting
}

[RequireComponent(typeof(EconomyManager))]
public class GameManager : MonoBehaviour
{
    /// <summary>
    /// Gets the current state that the game is in.
    /// </summary>
    public GameState CurrentGameState { get; private set; } = GameState.None;
    
    /// <summary>
    /// Triggers when game state is changed, and reports back the new state.
    /// </summary>
    public Action<GameState> onGameStateChange;

    /// <summary>
    /// Reports when turns are changed
    /// </summary>
    public Action onTurnChange; 
    
    public int CurrentTurnCount { get; private set; } = 0;

    public EconomyManager EconomyManager {get; private set;}

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject); 
            return;
        }

        _instance = this;
        DontDestroyOnLoad(gameObject); 
    }

    private void Start()
    {
        if (EconomyManager == null)
            EconomyManager = GetComponent<EconomyManager>();
    }

    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogError("GameManager not found");
            }
            return _instance;
        }
    }

    /// <summary>
    /// Switches to the next game state. 
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when a invalid state is switched to.</exception>
    public void NextState()
    {
        switch (CurrentGameState)
        {
            case GameState.None:
                break;
            case GameState.ContractWork:
                ShoppingState();
                break;
            case GameState.Shopping:
                CraftingState();
                break;
            case GameState.Crafting:
                ContractWorkState();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        
        onGameStateChange.Invoke(CurrentGameState);
    }
    private void CraftingState()
    {
        CurrentGameState = GameState.Crafting;
    }
    private void ShoppingState()
    {
        CurrentGameState = GameState.Shopping;
    }
    private void ContractWorkState()
    {
        CurrentGameState = GameState.ContractWork;
        
        onTurnChange?.Invoke();
        CurrentTurnCount++;
    }
}
