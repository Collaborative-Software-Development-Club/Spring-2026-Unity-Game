using UnityEngine;
using UnityEngine.UI;
using TMPro; // Assuming TextMeshPro

public class SpellUIManager : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject menuPanel; // The main parent panel

    [Header("UI Elements")]
    public Image spellIcon;
    public TextMeshProUGUI spellNameText;
    public TextMeshProUGUI loreText;
    public TextMeshProUGUI mechanicText;

    private void Start()
    {
        menuPanel.SetActive(false);
    }

    public void ToggleMenu(SpellData data)
    {
        bool isActive = !menuPanel.activeSelf;
        menuPanel.SetActive(isActive);

        if (isActive)
        {
            spellIcon.sprite = data.spellIcon;
            spellNameText.text = data.spellName;
            loreText.text = data.loreDescription;
            mechanicText.text = data.mechanicDescription;
        }
    }
}