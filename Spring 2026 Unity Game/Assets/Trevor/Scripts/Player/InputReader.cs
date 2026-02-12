using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "InputReader", menuName = "Input/Input Reader")]
public class InputReader : ScriptableObject, PlayerInputActions.IPlayerActions
{
    public event UnityAction<Vector2> MoveEvent = delegate { };
    public event UnityAction<bool> SprintEvent = delegate { };
    // Add this line:
    public event UnityAction InteractEvent = delegate { };
    public event UnityAction NumSelectEvent = delegate { };

    private PlayerInputActions _actions;

    public PlayerInputActions Actions
    {
        get
        {
            EnsureActions();
            return _actions;
        }
    }

    private void OnEnable()
    {
        EnsureActions();
        _actions.Player.Enable();
    }

    private void OnDisable()
    {
        _actions?.Player.Disable();
    }

    private void EnsureActions()
    {
        if (_actions != null)
            return;

        _actions = new PlayerInputActions();
        _actions.Player.SetCallbacks(this);
    }

    public void OnMove(InputAction.CallbackContext context) => MoveEvent.Invoke(context.ReadValue<Vector2>());

    public void OnSprint(InputAction.CallbackContext context)
    {
        if (context.started) SprintEvent.Invoke(true);
        else if (context.canceled) SprintEvent.Invoke(false);
    }

    // Implementation for the Interact action
    public void OnInteract(InputAction.CallbackContext context)
    {
        // LOG 1: Check if the Input System is even firing the method
        Debug.Log($"Input System: OnInteract called with phase: {context.phase}");

        if (context.phase == InputActionPhase.Performed)
        {
            // LOG 2: Check if the event is about to be sent to listeners
            Debug.Log("Input Reader: Interaction Event Triggered!");
            InteractEvent.Invoke();
        }
    }

    public void OnNumSelect(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
            NumSelectEvent.Invoke();
    }
}