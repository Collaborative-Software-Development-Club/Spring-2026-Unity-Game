using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

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
[RequireComponent(typeof(GUIManager))]
[RequireComponent(typeof(RentManager))]
[RequireComponent(typeof(ContractManager))]
public class GameManager : MonoBehaviour
{
    // TODO: Replace with shops in future
    [SerializeField] private Loottable[] DailyLootboxesData;
    private List<Lootbox> DailyLootboxes = new();
    public PlayerInventory playerInventory;
    
    /// <summary>
    /// Gets the current state that the game is in.
    /// </summary>
    public GameState CurrentGameState { get; private set; } = GameState.None;
    
    /// <summary>
    /// Triggers when game state is changed, and reports back the new state.
    /// </summary>
    public Action<GameState> onGameStateChange;

    /// <summary>
    /// Reports when turns are changed with the current turn
    /// </summary>
    public Action<int> onTurnChange; 
    
    public int CurrentTurnCount { get; private set; } = 0;

    public EconomyManager EconomyManager {get; private set;}
    public GUIManager GUIManager {get; private set;}
    public RentManager RentManager {get; private set;}
    public ContractManager ContractManager {get; private set;}

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject); 
            return;
        }

        _instance = this;
        DontDestroyOnLoad(gameObject); 

        if (EconomyManager == null)
            EconomyManager = GetComponent<EconomyManager>();
        if (GUIManager == null)
            GUIManager = GetComponent<GUIManager>();
        if (RentManager == null)
            RentManager = GetComponent<RentManager>();
        if (ContractManager == null)
            ContractManager = GetComponent<ContractManager>();

        foreach (Loottable lootbox in DailyLootboxesData)
        {
            DailyLootboxes.Add(new Lootbox(lootbox));
        }
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
                Debug.LogWarning("Game on none state, changing to contract!");
                ContractWorkState();
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
        playerInventory.AddItemToInventory(DailyLootboxes[Random.Range(0, DailyLootboxes.Count - 1)] ,1);
        CurrentGameState = GameState.ContractWork;
        
        CurrentTurnCount++;
        onTurnChange?.Invoke(CurrentTurnCount);
    }
}
