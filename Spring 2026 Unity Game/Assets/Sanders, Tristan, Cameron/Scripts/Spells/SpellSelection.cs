using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;

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
        SelectSpell(currentIndex,null);
        spellManager.OnSpellSelected += SelectSpell;
        spellManager.OnChargeChanged += UpdateSpellCount;
    }

    void InitializeSpellCounts()
    {
        int[] spellCharges = spellManager.GetCharges();
        for (int i =0; i < spellCharges.Length; i++)
        {
            UpdateSpellCount(i, spellCharges[i]);
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

        spellSlotImageHolders[currentIndex].color = defaultColor;
        spellSlotImageHolders[index].color = selectedColor;
        currentIndex = index;
    }

    private void UpdateSpellCount(int idx, int newCount)
    {
        spellTexts[idx].text = $"x{newCount}";
    }
}
