using TMPro.EditorUtilities;
using UnityEngine;

public class JEC_KeyboardPedestal : JEC_InteractableBase
{
    public GameObject KeyboardUI;
    public GameObject Player;

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
        Debug.Log("Interacted with Keyboard");
    }

    public void ExitPedestal()
    {
        KeyboardUI.SetActive(false);
        Player.GetComponent<PlayerController>().enabled = true;
    }

    public void EnterPedestal()
    {
        KeyboardUI.SetActive(true);
        Player.GetComponent<PlayerController>().enabled = false;
        Player.GetComponent<Rigidbody>().linearVelocity = Vector3.zero;

    }
}
