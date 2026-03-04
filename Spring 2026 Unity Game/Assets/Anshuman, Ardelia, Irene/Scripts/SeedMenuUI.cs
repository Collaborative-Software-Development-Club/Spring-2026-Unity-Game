using UnityEngine;

public class SeedMenuUI : MonoBehaviour
{
    public GameObject panel;
    private PlantSeed plantSeed;
    [SerializeField] private WaterSeed waterSeed;

    public void OpenMenu(PlantSeed seed)
    {
        plantSeed = seed;
        panel.SetActive(true);
    }

    public void CloseMenu()
    {
        panel.SetActive(false);
    }

    public void PlantSeed(int seedIndex)
    {
        Debug.Log("seed: " + seedIndex);
        if (waterSeed == null)
        {
            Debug.LogError("No WaterSeed assigned to SeedMenuUI");
            return;
        }
        waterSeed.SelectSeed(seedIndex);

        CloseMenu();
    }
}
