using UnityEngine;
using UnityEngine.InputSystem;

public class RenableWS : MonoBehaviour
{
    [SerializeField] private InputReader inputReader;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inputReader.Actions.Player.Move.RemoveAllBindingOverrides();
        }
    }
}