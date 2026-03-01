using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlotUI : MonoBehaviour
{
    public Image slotIcon;
    public GameObject background;
    public TextMeshProUGUI quantityText;
    
    public void SetQuantityText(int amount)
    {
       quantityText.text = amount.ToString(); 
    }

    public void UpdateIcon(Sprite icon)
    {
        slotIcon.sprite = icon;
    }
}
