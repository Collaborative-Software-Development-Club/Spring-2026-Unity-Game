using UnityEngine;

public class WaterSeed : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject interactionPrompt;
    [SerializeField] private InputReader inputReader;
    
    [Header("Plant Data")]
    [SerializeField] private GameObject[] plantPrefabs;
    [SerializeField] private Transform plantSpawnPoint;
    
    private bool watered = false;
    private bool seedSelected = false;
    private bool _isPlayerNearby = false;
    private int seedIndex = -1;
    [SerializeField] public int waterCost = 1;

    private void Awake()
    {
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
        if (!_isPlayerNearby || watered) return;
        if (PlayerInv.water >= waterCost)
        {
            GrowPlant();
        }
        else
        {
            Debug.Log("Not Enough Water");
        }
    }

    public void SelectSeed(int selectedSeedIndex)
    {
        seedIndex = selectedSeedIndex;
        seedSelected = true;

        Debug.Log("Seed planted, needs water.");
    }

    private void GrowPlant()
    {
        Debug.Log($"GrowPlant | seedIndex={seedIndex}, plantPrefabs.Length={plantPrefabs.Length}");

        if (plantSpawnPoint == null || seedIndex < 0 || !seedSelected) return;

        PlayerInv.water -= waterCost;
        Debug.Log("Plant watered! Water left: " + PlayerInv.water);
        Instantiate(plantPrefabs[seedIndex],
                    plantSpawnPoint.position,
                    plantSpawnPoint.rotation);
        watered = true;
        Debug.Log("Spawned plant: " + seedIndex);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _isPlayerNearby = true;

            if (interactionPrompt != null && !watered)
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
