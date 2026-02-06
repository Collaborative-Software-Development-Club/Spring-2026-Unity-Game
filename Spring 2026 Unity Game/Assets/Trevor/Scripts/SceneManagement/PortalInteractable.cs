using UnityEngine;

public class PortalInteractable : MonoBehaviour
{
    [Header("Settings")]
    public GameObject portalVisual;
    [SerializeField] private InputReader inputReader;

    private bool _isPlayerNearby = false;

    private void OnEnable()
    {
        if (inputReader != null)
        {
            inputReader.InteractEvent += HandleInteraction;
            Debug.Log($"{gameObject.name}: Subscribed to InputReader.");
        }
        else
        {
            Debug.LogError($"{gameObject.name}: InputReader is MISSING in the Inspector!");
        }
    }

    private void OnDisable()
    {
        if (inputReader != null) inputReader.InteractEvent -= HandleInteraction;
    }

    private void HandleInteraction()
    {
        // LOG 3: Did the script hear the event?
        Debug.Log($"{gameObject.name}: Received InteractEvent. Is player nearby? {_isPlayerNearby}");

        if (_isPlayerNearby)
        {
            ActivatePortal();
        }
    }

    private void ActivatePortal()
    {
        // LOG 4: Attempting to turn on the visual
        Debug.Log($"{gameObject.name}: Activating Portal...");

        if (portalVisual == null)
        {
            Debug.LogError($"{gameObject.name}: Portal Visual reference is NULL!");
            return;
        }

        portalVisual.SetActive(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        // LOG 5: Is the collision working?
        if (other.CompareTag("Player"))
        {
            _isPlayerNearby = true;
            Debug.Log("Player entered interaction zone.");
        }
        else
        {
            Debug.Log($"Something entered trigger, but it wasn't the 'Player' tag. It was: {other.gameObject.name}");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _isPlayerNearby = false;
            Debug.Log("Player left interaction zone.");
        }
    }
}