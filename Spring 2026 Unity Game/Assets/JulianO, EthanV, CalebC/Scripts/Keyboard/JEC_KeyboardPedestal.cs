using System.Collections;
//using TMPro.EditorUtilities;
using UnityEngine;
//using UnityEngine.EventSystems;

public class JEC_KeyboardPedestal : JEC_InteractableBase
{
    public GameObject KeyboardUI;
    public GameObject Player;

    private bool isActive;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
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
        if (!isActive)
        {
            StartCoroutine(BufferInteract());
        }
    }

    IEnumerator BufferInteract()
    {
        yield return new WaitForEndOfFrame();

        JEC_Events.OnInteractKeyboardPedestal.Invoke();
    }

    public void ExitPedestal()
    {
        isActive = false;
        KeyboardUI.SetActive(false);
        Player.GetComponent<PlayerController>().enabled = true;
    }

    public void EnterPedestal()
    {
        isActive = true;
        KeyboardUI.SetActive(true);
        Player.GetComponent<PlayerController>().enabled = false;
        Player.GetComponent<Rigidbody>().linearVelocity = Vector3.zero;

    }
}
