using System;
using Unity.VisualScripting;
using UnityEngine;

public abstract class JEC_InteractableBase : MonoBehaviour
{
    [SerializeField] private InputReader inputReader;
    [SerializeField] private GameObject interactionPrompt;
    //Hides prompt after one interaction
    [SerializeField] private bool hidePromptOnInteract = false;
    private void OnEnable()
    {
        if (inputReader != null)
        {
            inputReader.InteractEvent += DisableInteract;
        }
    }

    private void OnDisable()
    {
        if (inputReader != null)
        {
            inputReader.InteractEvent -= DisableInteract;
        }
    }

    protected abstract void Interact();

    private void DisableInteract()
    {
        if (interactionPrompt != null && hidePromptOnInteract) interactionPrompt.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (interactionPrompt != null)
            {
                interactionPrompt.SetActive(true);
                inputReader.InteractEvent += Interact;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (interactionPrompt != null)
            {
                interactionPrompt.SetActive(false);
                inputReader.InteractEvent -= Interact;
            }
        }
    }
}
