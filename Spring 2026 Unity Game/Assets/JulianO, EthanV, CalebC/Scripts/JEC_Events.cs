using UnityEngine;
using UnityEngine.Events;

public static class JEC_Events
{
    public static UnityEvent OnInteractPedestal = new UnityEvent();

    public static UnityEvent<string> OnKeyPressSuccess = new UnityEvent<string>();

    public static UnityEvent<string> OnKeyPressFailure = new UnityEvent<string>();

    public static UnityEvent<string> OnKeyRemoved = new UnityEvent<string>();
}
