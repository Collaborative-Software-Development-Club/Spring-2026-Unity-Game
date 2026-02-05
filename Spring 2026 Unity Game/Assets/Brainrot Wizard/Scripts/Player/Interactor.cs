using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class Interacter : MonoBehaviour
{
    private static Interacter instance;
    public static Interacter MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindFirstObjectByType<Interacter>();
            }
            return instance;
        }
    }
    [SerializeField] private Transform _interactionPoint; // Interaction Point
    [SerializeField] private float _interactionPointRadius = 0.5f; // The radius in which you can interact
    [SerializeField] private LayerMask _interactableMask;
    [HideInInspector] public bool inInteractionRadius = false;

    private readonly Collider2D[] _colliders = new Collider2D[3];
    private Collider2D _interactionCollider; // Number of colliders found
    private bool _interacting = false; // Makes interacting into a bool
    private bool _isInteracting = false; // Prevents being away from a object and still doing the input

    private void Update()
    {
        _interactionCollider = Physics2D.OverlapCircle(_interactionPoint.position, _interactionPointRadius, _interactableMask);
        
        if(_interactionCollider)
        {
            var interactable = _interactionCollider.GetComponent<IInteractable>(); // Gets the interface
            inInteractionRadius = true;

            if (interactable == null || _interacting != true) return; // New input system get key down || Implement Other Devices Later
            interactable.Interact(this);
            _interacting = false;
        }
        else 
            inInteractionRadius = false;
    }
    public void OnInteract(InputAction.CallbackContext ctxt)
    {
        if (_isInteracting == true)
        {
            _interacting = false;
            _isInteracting = false;
        }
        else if (_isInteracting == false)
        {
            _interacting = true;
            _isInteracting = true;
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_interactionPoint.position, _interactionPointRadius);
    }
}