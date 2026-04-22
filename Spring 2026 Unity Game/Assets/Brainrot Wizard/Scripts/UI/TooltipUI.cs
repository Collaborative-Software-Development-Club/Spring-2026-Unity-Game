using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TooltipUI : MonoBehaviour
{
    [Header("Common")] 
    public GameObject background;
    public Image iconImage;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI valueText;
    public TextMeshProUGUI descriptionText;

    [Header("Brainrot Section")]
    public GameObject brainrotSection;
    public TextMeshProUGUI brainrotCategoryText;
    public TextMeshProUGUI brainrotAttributesText;
    
    [Header("Lootbox Section")]
    public GameObject lootboxSection;
    public GameObject lootboxDropPrefab;
    public TextMeshProUGUI lootboxChancesText;

    public void Show(Item item)
    {
        ResetSections();

        if (item.GetIcon())
        {
            iconImage.sprite = item.GetIcon();
            
            var fitter = iconImage.GetComponent<AspectRatioFitter>();
            fitter.aspectRatio = (float)item.GetIcon().texture.width
                                 / item.GetIcon().texture.height;
            
            iconImage.gameObject.SetActive(true);
        }
        else
        {
            iconImage.gameObject.SetActive(false);
        }

        nameText.text = item.GetName();
        valueText.text = StringUtils.AbbreviateNumber(item.GetValue()) + " Coins";
        descriptionText.text = item.GetDescription();

        switch (item.GetItemType())
        {
            case ItemType.Brainrot:
                ShowBrainrotSection(item as Brainrot); 
                break;
            case ItemType.Lootbox:
                ShowLootboxSection(item as Lootbox);
                break;
        }

        background.SetActive(true);
    }
    private void ResetSections()
    {
        brainrotSection.SetActive(false);
        lootboxSection.SetActive(false);
    }

    private void ShowBrainrotSection(Brainrot brainrot)
    {
        if (brainrot == null)
            return;
        
        brainrotSection.SetActive(true);

        brainrotCategoryText.text = StringUtils.PlaceSeparators(brainrot.GetCategory().ToString());

        foreach (var attributeQuantity in brainrot.GetAttributes())
        {
            brainrotAttributesText.text = StringUtils.PlaceSeparators(attributeQuantity.attribute.ToString()) + ": " + attributeQuantity.quantity + "\n";
        }
    }
    private void ShowLootboxSection(Lootbox lootbox)
    {
        if (lootbox == null)
            return;
        
        lootboxSection.SetActive(true);

        foreach (var drop in lootbox.GetDropSummary())
        {
            lootboxChancesText.text += drop + "\n";
        }
    }

    public void Hide()
    {
        background.SetActive(false);
    }
}