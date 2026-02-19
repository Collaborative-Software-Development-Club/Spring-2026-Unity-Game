using UnityEngine;
using UnityEngine.Events;

public static class JEC_Events
{
    public static UnityEvent OnInteractKeyboardPedestal = new UnityEvent();

    public static UnityEvent OnExitKeyboardPedestal = new UnityEvent();

    public static UnityEvent<string> OnPickupKey = new UnityEvent<string>();

    public static UnityEvent<string> OnKeyPressSuccess = new UnityEvent<string>();

    public static UnityEvent<string> OnKeyPressFailure = new UnityEvent<string>();

    public static UnityEvent<string> OnKeyRemoved = new UnityEvent<string>();

    public static UnityEvent OnStartNPC = new UnityEvent();

    public static UnityEvent ProgressDialogueNPC = new UnityEvent();

    public static UnityEvent OnStopNPC = new UnityEvent();

}
