using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlotUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler 
{
    public Image slotIcon;
    public GameObject background;
    public GameObject brainDisplayPrefab;
    public GameObject brainDisplay; // instantiated from brainDisplayPrefab on icon update
    public TextMeshProUGUI quantityText;
    public TextMeshProUGUI nameText;
    private bool _showTooltip = false;

    private InventorySlot _linkedSlot;
    private BrainrotDisplayUI brainDisplayUI; // the script from brainDisplay
    
    public void SetQuantityText(int amount)
    {
        if (quantityText != null)
            quantityText.text = amount <= 1 ? "" : StringUtils.AbbreviateNumber(amount);
    }

    public void UpdateName(string newName)
    {
        nameText.text = newName;
        nameText.gameObject.SetActive(true);
        slotIcon.gameObject.SetActive(false);
    }

    public void UpdateIcon(Sprite icon)
    {
        if (brainDisplay is not null) {
            Destroy(brainDisplay);
        }
        print (_linkedSlot.Type());
        if (_linkedSlot != null && _linkedSlot.Type() == ItemType.Brainrot) {
            brainDisplay = GameObject.Instantiate(brainDisplayPrefab);
            brainDisplay.transform.SetParent(transform);
            brainDisplay.transform.position = transform.position;
            brainDisplayUI = brainDisplay.GetComponent<BrainrotDisplayUI>();
            brainDisplayUI.Begin((Brainrot)(_linkedSlot.item));
        }

        slotIcon.sprite = icon;
        nameText.gameObject.SetActive(false);
        slotIcon.gameObject.SetActive(true);
    }

    public void InitializeInventorySlot(InventorySlot inventorySlot)
    {
        _linkedSlot = inventorySlot;
        RefreshSlotVisuals();
    }
    
    public void SetItem(Item newItem, int quantity = 1)
    {
        if (_linkedSlot == null) return;

        _linkedSlot.item = newItem;
        _linkedSlot.quantity = quantity;

        RefreshSlotVisuals();
    }

    public void RefreshSlotVisuals()
    {
        if (_linkedSlot?.item == null)
        {
            slotIcon.gameObject.SetActive(false);
            SetQuantityText(0);
            return;
        }
        
        slotIcon.gameObject.SetActive(true);
        UpdateIcon(_linkedSlot.item.GetIcon());
        SetQuantityText(_linkedSlot.quantity);
    }

    public Item GetItem()
    {
        return _linkedSlot?.item;
    }

    public int GetQuantity()
    {
        return _linkedSlot != null ? _linkedSlot.quantity : 0;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (GetItem() == null && !_showTooltip) return;
        GameManager.Instance.GUIManager.TooltipUIRef.Show(GetItem());
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (GetItem() == null) return;
        GameManager.Instance.GUIManager.TooltipUIRef.Hide();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            GameManager.Instance.GUIManager.SwapMouseContent(this);
        }
        else
        {
            GetItem()?.Consume();
        }
    }

    public void ShowTooltip(bool show)
    {
        _showTooltip = show;
    }
}