using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SpellSelection : MonoBehaviour
{
    [SerializeField] SpellManager spellManager;

    [Header("Spell Slots")]
    [SerializeField] TextMeshProUGUI[] spellTexts;
    [SerializeField] Image[] spellSlotImageHolders;

    [Header("Selection Settings")]
    public Color defaultColor = Color.white;
    public Color selectedColor = Color.yellow;

    int currentIndex = 0;


    void Start()
    {
        //InitializeImages();
        InitializeSpellCounts();
        spellManager.OnSpellSelected += SelectSpell;
        spellManager.OnChargeChanged += UpdateSpellCount;
        spellManager.OnLoadoutChanged += InitializeSpellCounts;
        SelectSpell(currentIndex,null);
    }

    void OnDestroy()
    {
        if (spellManager == null)
        {
            return;
        }

        spellManager.OnSpellSelected -= SelectSpell;
        spellManager.OnChargeChanged -= UpdateSpellCount;
        spellManager.OnLoadoutChanged -= InitializeSpellCounts;
    }

    void InitializeSpellCounts()
    {
        for (int i = 0; i < spellTexts.Length; i++)
        {
            spellTexts[i].text = "x0";
        }

        int[] spellCharges = spellManager.GetCharges();
        int count = Mathf.Min(spellCharges.Length, spellTexts.Length);

        for (int i = 0; i < count; i++)
        {
            UpdateSpellCount(i, spellCharges[i]);
        }

        if (spellSlotImageHolders.Length > 0)
        {
            currentIndex = Mathf.Clamp(currentIndex, 0, spellSlotImageHolders.Length - 1);
            SelectSpell(currentIndex, null);
        }
    }

    /* I'll prolly work this later so that it gives the slots images based on the spell the represent
    void InitializeImages()
    {
        for(int i = 0; i < spellSlots.Length; i++)
        {
            Sprite spellSprite = spellSlots[i].GetComponent<SpriteRenderer>().sprite;
            spellSlotImageHolders[i].sprite = spellSprite;
        }
    }
    */

    private void SelectSpell(int index, GameObject gameObject)
    {
        if (index < 0 || index >= spellSlotImageHolders.Length)
            return;

        if (currentIndex >= 0 && currentIndex < spellSlotImageHolders.Length)
        {
            spellSlotImageHolders[currentIndex].color = defaultColor;
        }

        spellSlotImageHolders[index].color = selectedColor;
        currentIndex = index;
    }

    private void UpdateSpellCount(int idx, int newCount)
    {
        if (idx < 0 || idx >= spellTexts.Length)
        {
            return;
        }

        spellTexts[idx].text = $"x{newCount}";
    }
}
