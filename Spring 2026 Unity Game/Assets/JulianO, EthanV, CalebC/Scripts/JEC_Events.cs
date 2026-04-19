using UnityEngine;
using UnityEngine.Events;

public static class JEC_Events
{
    public static UnityEvent OnInteractKeyboardPedestal = new UnityEvent();
    public static UnityEvent OnInteractMessengerPedestal = new UnityEvent();

    public static UnityEvent<string> OnEnterURL = new UnityEvent<string>();

    public static UnityEvent OnExitKeyboardPedestal = new UnityEvent();
    public static UnityEvent OnExitMessengerPedestal = new UnityEvent();

    public static UnityEvent<string> OnPickupKey = new UnityEvent<string>();

    public static UnityEvent<string> OnKeyPressSuccess = new UnityEvent<string>();

    public static UnityEvent<string> OnKeyPressFailure = new UnityEvent<string>();

    public static UnityEvent<string> OnKeyRemoved = new UnityEvent<string>();

    public static UnityEvent OnStartNPC = new UnityEvent();

    public static UnityEvent ProgressDialogueNPC = new UnityEvent();

    public static UnityEvent LeaveNPC = new UnityEvent();

    public static UnityEvent<JEC_PageData> OnPageChanged = new UnityEvent<JEC_PageData>();
    public static UnityEvent<JEC_MessengerMessage> OnMessengerMessageReceived = new UnityEvent<JEC_MessengerMessage>();
    public static UnityEvent<bool> OnMessengerUnreadChanged = new UnityEvent<bool>();

    public static UnityEvent<GameObject> OnPopupClosed = new UnityEvent<GameObject>();

}
