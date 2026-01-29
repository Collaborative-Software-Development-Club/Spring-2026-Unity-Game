using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "InputReader", menuName = "Input/Input Reader")]
public class InputReader : ScriptableObject, PlayerInputActions.IPlayerActions
{
    public event UnityAction<Vector2> MoveEvent = delegate { };
    public event UnityAction<bool> SprintEvent = delegate { };

    private PlayerInputActions _actions;

    private void OnEnable()
    {
        if (_actions == null)
        {
            _actions = new PlayerInputActions();
            _actions.Player.SetCallbacks(this);
        }
        _actions.Player.Enable();
    }

    private void OnDisable()
    {
        if (_actions != null)
        {
            _actions.Player.Disable();
        }
    }

    public void OnMove(InputAction.CallbackContext context) => MoveEvent.Invoke(context.ReadValue<Vector2>());

    public void OnSprint(InputAction.CallbackContext context)
    {
        if (context.started) SprintEvent.Invoke(true);
        else if (context.canceled) SprintEvent.Invoke(false);
    }
}