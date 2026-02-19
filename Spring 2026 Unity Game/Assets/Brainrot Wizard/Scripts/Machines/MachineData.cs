using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Windows;

public enum machineType
{ 
    None,
    Add,
    Remove,
    Duplicate,
    Swap,
    Fission,
    Fusion
}



[CreateAssetMenu(fileName = "New Machine", menuName = "Game/BrainrotMixer/Machine/Base Machine")]
public class MachineData : ScriptableObject
{
    public new string name = "Unnamed";
    public machineType processType = machineType.None;
    public Sprite texture;
    public int inputCount = 0;
    public int outputCount = 0;
}