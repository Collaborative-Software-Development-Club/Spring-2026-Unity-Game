using UnityEngine;

public class PatchOfDirt : MonoBehaviour
{
    private bool playerInRange = false;
    private bool hasPlant = false;

    public Transform plantSpawnPoint;
    public GameObject[] plantPrefabs;

    public SeedMenuUI seedMenu;   // NEW

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }

    private void Update()
    {
        if (playerInRange && !hasPlant && Input.GetKeyDown(KeyCode.E))
        {
            seedMenu.OpenMenu(this);
        }
    }

    public void PlantSeed(int seedIndex)
    {
        if (plantSpawnPoint == null) return;

        Instantiate(plantPrefabs[seedIndex],
                    plantSpawnPoint.position,
                    plantSpawnPoint.rotation);

        hasPlant = true;
    }
}