using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlotUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Image slotIcon;
    public GameObject background;
    public TextMeshProUGUI quantityText;
    public TextMeshProUGUI nameText;

    private Item item;
    
    public void SetQuantityText(int amount)
    {
        quantityText.text = amount <= 0 ? "" : StringUtils.AbbreviateNumber(amount);
    }

    public void UpdateName(string newName)
    {
        nameText.text = newName;
        
        nameText.gameObject.SetActive(true);
        slotIcon.gameObject.SetActive(false);
    }
    public void UpdateIcon(Sprite icon)
    {
        slotIcon.sprite = icon;
        
        nameText.gameObject.SetActive(false);
        slotIcon.gameObject.SetActive(true);
    }
    
    // This is needed for the tooltips
    public void SetItem(Item newItem)
    {
        if (newItem == null)
            slotIcon.gameObject.SetActive(false);
        else
            slotIcon.gameObject.SetActive(true);
        
        item = newItem;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (item == null) return;
        
        GameManager.Instance.GUIManager.TooltipUIRef.Show(item);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (item == null) return;
        
        GameManager.Instance.GUIManager.TooltipUIRef.Hide();
    }
}
