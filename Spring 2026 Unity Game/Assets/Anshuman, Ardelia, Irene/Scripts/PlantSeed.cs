using UnityEngine;

public class PlantSeed : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject seed;
    [SerializeField] private GameObject interactionPrompt; // Drag your UI Canvas/Panel here
    [SerializeField] private InputReader inputReader;

    private bool _isPlayerNearby = false;

    private void Awake()
    {
        // Ensure everything is hidden at the start
        if (seed != null) seed.SetActive(false);
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
        if (_isPlayerNearby && !seed.activeSelf)
        {
            PlantOption();
        }
    }

    private void PlantOption()
    {
        seed.SetActive(true);

        // Hide the prompt once the seed is planted
        if (interactionPrompt != null) interactionPrompt.SetActive(false);

        Debug.Log("Seed Planted!");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _isPlayerNearby = true;

            // Only show prompt if the seed isn't already planted
            if (interactionPrompt != null && !seed.activeSelf)
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
