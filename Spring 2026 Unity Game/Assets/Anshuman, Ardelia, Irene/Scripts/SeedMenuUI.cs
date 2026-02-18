using UnityEngine;

public class SeedMenuUI : MonoBehaviour
{
    public GameObject panel;
    private PatchOfDirt currentPatch;

    public void OpenMenu(PatchOfDirt patch)
    {
        currentPatch = patch;
        panel.SetActive(true);
    }

    public void CloseMenu()
    {
        panel.SetActive(false);
    }

    public void PlantSeed(int seedIndex)
    {
        if (currentPatch != null)
        {
            currentPatch.PlantSeed(seedIndex);
        }

        CloseMenu();
    }
}
