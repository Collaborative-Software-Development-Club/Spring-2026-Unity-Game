using UnityEngine;

public enum machineType
{ 
    add,
    remove,
    copy,
    swap
}

[CreateAssetMenu(fileName = "New Machine", menuName = "Game/BrainrotMixer/Machine/Base Machine")]
public class MachineData : ScriptableObject
{
    public new string name = "Unnamed";
    public machineType processType;
    public Sprite texture;

}
