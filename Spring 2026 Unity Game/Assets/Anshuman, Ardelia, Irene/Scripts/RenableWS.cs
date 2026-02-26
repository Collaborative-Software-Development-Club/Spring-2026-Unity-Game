using UnityEngine;
using UnityEngine.InputSystem;

public class RenableWS : MonoBehaviour
{
    [SerializeField] private InputReader inputReader;
    
    void Start()
    {
        inputReader.Actions.Player.Move.RemoveBindingOverride(0);
        inputReader.Actions.Player.Move.RemoveBindingOverride(1);
        inputReader.Actions.Player.Move.RemoveAllBindingOverrides();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inputReader.Actions.Player.Move.RemoveAllBindingOverrides();
        }
    }
}