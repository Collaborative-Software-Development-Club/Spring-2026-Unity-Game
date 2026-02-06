using UnityEngine;

public class PortalInteractable : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject portalVisual;
    [SerializeField] private GameObject interactionPrompt; // Drag your UI Canvas/Panel here
    [SerializeField] private InputReader inputReader;

    private bool _isPlayerNearby = false;

    private void Awake()
    {
        // Ensure everything is hidden at the start
        if (portalVisual != null) portalVisual.SetActive(false);
        if (interactionPrompt != null) interactionPrompt.SetActive(false);
    }

    private void OnEnable()
    {
        if (inputReader != null) inputReader.InteractEvent += HandleInteraction;
    }

    private void OnDisable()
    {
        if (inputReader != null) inputReader.InteractEvent -= HandleInteraction;
    }

    private void HandleInteraction()
    {
        if (_isPlayerNearby && !portalVisual.activeSelf)
        {
            ActivatePortal();
        }
    }

    private void ActivatePortal()
    {
        portalVisual.SetActive(true);

        // Hide the prompt once the portal is open
        if (interactionPrompt != null) interactionPrompt.SetActive(false);

        Debug.Log("Portal Activated!");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _isPlayerNearby = true;

            // Only show prompt if the portal isn't already open
            if (interactionPrompt != null && !portalVisual.activeSelf)
            {
                interactionPrompt.SetActive(true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _isPlayerNearby = false;
            if (interactionPrompt != null) interactionPrompt.SetActive(false);
        }
    }
}