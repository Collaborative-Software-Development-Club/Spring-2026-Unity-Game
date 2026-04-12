
using System.Collections.Generic;
using UnityEngine;

public enum Difficulty {
    Easy,
    Medium,
    Hard
} 

[CreateAssetMenu(fileName = "New Contract", menuName = "Game/BrainrotMixer/Contracts/Contract")]
public class ContractData : ItemData
{
    public static readonly string[] ContractNames = new string[]
    {
        "Junho",
        "Sam",
        "Tristan",
        "Abdel",
        "Adam",
        "Alec",
        "Alex",
        "Ali",
        "Alvin",
        "Amir",
        "Antonio",
        "Brady",
        "Collin",
        "Connor",
        "Drew",
        "Felix",
        "Frank",
        "Hank",
        "Hunter",
        "Ian",
        "Jerry",
        "Jocelin",
        "Johnny",
        "Jonathon",
        "Joy",
        "Juaquin",
        "Karam",
        "Krishna",
        "Logan",
        "Manraj",
        "Marley",
        "Matt",
        "Mohisha",
        "Jacob",
        "Noah",
        "Parker",
        "Rachel",
        "Raymond",
        "Robbie",
        "Sarah",
        "Shawn",
        "Spencer",
        "Tony",
        "Wyatt",
        "Trevor"
    };
    
    public static readonly Dictionary<Difficulty, float> DifficultyMultiplier =
        new()
        {
            { Difficulty.Easy, 1f },
            { Difficulty.Medium, 1.5f },
            { Difficulty.Hard, 2f }
        };

    public int TurnDuration = 0;
    public Difficulty difficulty = Difficulty.Easy;
}
