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
    public RectTransform brainrotAttributesRect;
    
    [Header("Lootbox Section")]
    public GameObject lootboxSection;
    public RectTransform lootboxChancesRect;
    public GameObject lootboxDropPrefab;

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
        valueText.text = StringUtils.AbbreviateNumber(item.GetValue());
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
        if (!brainrot)
            return;
        
        brainrotSection.SetActive(true);

        brainrotCategoryText.text = StringUtils.PlaceSeparators(brainrot.GetCategory().ToString());

        foreach (var attributeQuantity in brainrot.GetAttributes())
        {
            GameManager.Instance.GUIManager.CreateSlot(attributeQuantity).transform.parent = brainrotAttributesRect;
        }
    }
    private void ShowLootboxSection(Lootbox lootbox)
    {
        if (!lootbox)
            return;
        
        lootboxSection.SetActive(true);

        foreach (var drop in lootbox.GetDropSummary())
        {
            GameObject temp = Instantiate(lootboxDropPrefab, lootboxChancesRect);
            TextMeshProUGUI tempText = temp.GetComponent<TextMeshProUGUI>();
            
            tempText.text = drop;
        }
    }

    public void Hide()
    {
        background.SetActive(false);
    }
}