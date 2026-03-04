using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LootboxUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI lootboxTitleText; // Lootbox name
    [SerializeField] private TextMeshProUGUI dropTextPrefab;    // Prefab for drop entries
    [SerializeField] private Transform contentParent;          // Parent for instantiated drops
    [SerializeField] private Button openButton;                // Open/Roll button

    [Header("References")]
    [SerializeField] private PlayerInventory playerInventory;  // Player's inventory reference

    private Lootbox currentLootbox;
    private bool hasRolled = false;

    /// <summary>
    /// Open the UI for a specific lootbox
    /// </summary>
    public void OpenLootboxUI(Lootbox lootbox)
    {
        currentLootbox = lootbox;
        hasRolled = false;

        // Set lootbox title
        lootboxTitleText.text = currentLootbox.name;

        // Clear previous drop list
        foreach (Transform child in contentParent)
            Destroy(child.gameObject);

        // Show all possible drops as a summary
        foreach (string drop in currentLootbox.GetDropSummary())
        {
            TextMeshProUGUI newText = Instantiate(dropTextPrefab, contentParent);
            newText.text = drop;
        }

        // Setup button
        openButton.onClick.RemoveAllListeners();
        openButton.onClick.AddListener(OnOpenButtonClicked);
        openButton.interactable = true;
        openButton.GetComponentInChildren<TextMeshProUGUI>().text = "Open Lootbox";

        gameObject.SetActive(true);
    }

    /// <summary>
    /// Handles open/roll button click
    /// </summary>
    private void OnOpenButtonClicked()
    {
        if (currentLootbox == null) return;

        if (!hasRolled)
        {
            LoottableEntry entry = currentLootbox.Roll();
            if (entry != null)
            {
                // Convert ItemData → runtime Item
                Item runtimeItem = ItemFactory.CreateItem(entry.ItemData);
                if (runtimeItem != null)
                {
                    playerInventory.AddItemToInventory(runtimeItem, entry.ItemQuantity);
                }

                lootboxTitleText.text = $"You got: {entry.ItemData.name} x{entry.ItemQuantity}";
            }
            else
            {
                lootboxTitleText.text = "You got nothing!";
            }

            hasRolled = true;
            openButton.GetComponentInChildren<TextMeshProUGUI>().text = "Close Lootbox";
        }
        else
        {
            CloseLootboxUI();
        }
    }

    /// <summary>
    /// Close the lootbox UI and reset
    /// </summary>
    public void CloseLootboxUI()
    {
        gameObject.SetActive(false);

        // Clear content
        foreach (Transform child in contentParent)
            Destroy(child.gameObject);

        // Reset button
        openButton.onClick.RemoveAllListeners();
        openButton.GetComponentInChildren<TextMeshProUGUI>().text = "Open Lootbox";

        currentLootbox = null;
        hasRolled = false;
    }
}