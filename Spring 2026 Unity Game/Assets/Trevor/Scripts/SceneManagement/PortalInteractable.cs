using UnityEngine;

public class PortalInteractable : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject portalVisual;
    [SerializeField] private GameObject glowVisual;
    [SerializeField] private CanvasGroup interactionPrompt;
    [SerializeField] private InputReader inputReader;

    [Header("Audio")]
    [SerializeField] private AudioClip portalActivateSFX;
    [SerializeField] private AudioClip portalNearbySFX;

    private bool _isPlayerNearby = false;

    private void Awake()
    {
        if (portalVisual != null) portalVisual.SetActive(false);
        if (glowVisual != null) glowVisual.SetActive(false);

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
        if (_isPlayerNearby && !portalVisual.activeSelf)
        {
            ActivatePortal();
        }
    }

    private void ActivatePortal()
    {
        if (portalVisual != null) portalVisual.SetActive(true);

        if (interactionPrompt != null)
        {
            interactionPrompt.alpha = 0;
        }

        MAIN_SFXManager.Instance.PlaySFX(portalActivateSFX);
        Debug.Log("Portal Activated!");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _isPlayerNearby = true;

            if (glowVisual != null) glowVisual.SetActive(true);

            if (interactionPrompt != null && !portalVisual.activeSelf)
            {
                interactionPrompt.alpha = 1;
                // Play sound when prompt appears
                MAIN_SFXManager.Instance.PlaySFX(portalNearbySFX);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _isPlayerNearby = false;

            if (glowVisual != null) glowVisual.SetActive(false);

            if (interactionPrompt != null)
            {
                interactionPrompt.alpha = 0;
            }
        }
    }
}