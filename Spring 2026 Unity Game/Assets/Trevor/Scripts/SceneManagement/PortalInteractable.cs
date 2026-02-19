using UnityEngine;

public class PortalInteractable : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject portalVisual;      // The actual portal effect
    [SerializeField] private GameObject glowVisual;        // The 'Glow' child object for the pillar
    [SerializeField] private CanvasGroup interactionPrompt; // The UI popup (add a Canvas Group component)
    [SerializeField] private InputReader inputReader;

    private bool _isPlayerNearby = false;

    private void Awake()
    {
        // Ensure the portal and glow are hidden at the start
        if (portalVisual != null) portalVisual.SetActive(false);
        if (glowVisual != null) glowVisual.SetActive(false);

        // UI Optimization: Keep the object active but hidden.
        // This forces Unity to load fonts/textures into memory now
        // rather than during the first interaction, preventing lag spikes.
        if (interactionPrompt != null)
        {
            interactionPrompt.alpha = 0;
            interactionPrompt.blocksRaycasts = false;
        }
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
        // Only allow activation if the player is nearby and portal isn't already active
        if (_isPlayerNearby && !portalVisual.activeSelf)
        {
            ActivatePortal();
        }
    }

    private void ActivatePortal()
    {
        if (portalVisual != null) portalVisual.SetActive(true);

        // Hide the UI prompt immediately upon activation
        if (interactionPrompt != null)
        {
            interactionPrompt.alpha = 0;
        }

        Debug.Log("Portal Activated!");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _isPlayerNearby = true;

            // Enable the glow effect when the player is in range
            if (glowVisual != null) glowVisual.SetActive(true);

            // Show prompt only if the portal isn't already open
            if (interactionPrompt != null && !portalVisual.activeSelf)
            {
                interactionPrompt.alpha = 1;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _isPlayerNearby = false;

            // Disable the glow when the player leaves
            if (glowVisual != null) glowVisual.SetActive(false);

            // Hide the UI prompt
            if (interactionPrompt != null)
            {
                interactionPrompt.alpha = 0;
            }
        }
    }
}