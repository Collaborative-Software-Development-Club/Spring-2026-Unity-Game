using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SpellEquipPopup : MonoBehaviour
{
    [Header("UI References")]
    [Tooltip("The parent panel of the popup to turn on/off")]
    [SerializeField] private GameObject popupPanel;
    [Tooltip("The UI Image component where the book sprite will go")]
    [SerializeField] private Image bookIconUI;

    [SerializeField] private TextMeshProUGUI Q;
    [SerializeField] private TextMeshProUGUI R;

    private void OnEnable()
    {
        // Listen for both equipping and unequipping
        PlayerSpellManager.OnSpellEquipped += ShowPopup;
        PlayerSpellManager.OnSpellUnequipped += HidePopup;
    }

    private void OnDisable()
    {
        // Always unsubscribe to prevent memory leaks!
        PlayerSpellManager.OnSpellEquipped -= ShowPopup;
        PlayerSpellManager.OnSpellUnequipped -= HidePopup;
    }

    // Receive the sprite directly from the event
    private void ShowPopup(SpellData spellData, Sprite bookSprite)
    {
        if (spellData == null) return;

        // Dynamically assign the sprite
        if (bookSprite != null)
        {
            bookIconUI.sprite = bookSprite;
        }
        else
        {
            Debug.LogWarning("The equipped book did not pass a valid sprite.");
        }

        if (Q != null) Q.enabled = true;
        if (R != null) R.enabled = true;

        // Turn the panel ON and leave it on
        popupPanel.SetActive(true);
    }

    private void HidePopup()
    {
        // Turn the panel OFF when the spell is unequipped
        popupPanel.SetActive(false);
    }
}