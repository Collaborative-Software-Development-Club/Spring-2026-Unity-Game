using TMPro.EditorUtilities;
using UnityEngine;

public class JEC_KeyboardPedestal : JEC_InteractableBase
{
    public GameObject KeyboardUI;
    public PlayerController Controller;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            JEC_Events.OnExitKeyboardPedestal.Invoke();
        }
    }

    private void Start()
    {
        JEC_Events.OnInteractKeyboardPedestal.AddListener(EnterPedestal);
        JEC_Events.OnExitKeyboardPedestal.AddListener(ExitPedestal);
    }

    protected override void Interact()
    {
        JEC_Events.OnInteractKeyboardPedestal.Invoke();
    }

    public void ExitPedestal()
    {
        KeyboardUI.SetActive(false);
        Controller.enabled = true;
    }

    public void EnterPedestal()
    {
        KeyboardUI.SetActive(true);
        Controller.enabled = false;
    }
}
