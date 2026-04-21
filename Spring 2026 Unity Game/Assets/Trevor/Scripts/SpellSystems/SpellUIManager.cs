using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SpellUIManager : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject menuPanel;

    [Header("UI Elements")]
    public Image spellIcon;
    public TextMeshProUGUI spellNameText;
    public TextMeshProUGUI loreText;
    public TextMeshProUGUI mechanicText;

    [Header("Audio")]
    [SerializeField] private AudioClip menuOpenSFX;
    [SerializeField] private AudioClip menuCloseSFX;

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
            MAIN_SFXManager.Instance.PlaySFX(menuOpenSFX);
            spellIcon.sprite = data.spellIcon;
            spellNameText.text = data.spellName;
            loreText.text = data.loreDescription;
            mechanicText.text = data.mechanicDescription;
        }
        else
        {
            MAIN_SFXManager.Instance.PlaySFX(menuCloseSFX);
        }
    }
}