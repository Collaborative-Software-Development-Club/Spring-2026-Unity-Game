using UnityEngine;
using UnityEngine.Events;

public class PlayerSpellManager : MonoBehaviour
{
    [Header("Current State")]
    public SpellData currentSpellData;
    private SpellBehavior currentSpellInstance;

    [Header("References")]
    [Tooltip("Where projectiles should spawn from")]
    [SerializeField] private Transform castPoint;
    [SerializeField] private SpellUIManager uiManager;

    // The event now passes the SpellData AND the dynamic Sprite!
    public static UnityAction<SpellData, Sprite> OnSpellEquipped;
    public static UnityAction OnSpellUnequipped;

    private void Update()
    {
        // Replace these with your InputReader events later!
        if (Input.GetKeyDown(KeyCode.Q)) TryCastSpell();
        if (Input.GetKeyDown(KeyCode.R)) ToggleSpellMenu();
    }

    // Now requires the sprite of the physical book being equipped
    public void EquipSpell(SpellData newSpell, Sprite physicalBookSprite)
    {
        // If we already have a spell, destroy its instance
        if (currentSpellInstance != null)
        {
            Destroy(currentSpellInstance.gameObject);
        }

        currentSpellData = newSpell;

        // Instantiate the team's custom prefab and keep it as a child of the player
        if (newSpell.spellPrefab != null)
        {
            currentSpellInstance = Instantiate(newSpell.spellPrefab, transform);
        }

        Debug.Log($"Equipped {newSpell.spellName}");

        // Pass both the data and the dynamically grabbed sprite to the UI
        OnSpellEquipped?.Invoke(currentSpellData, physicalBookSprite);
    }

    public void UnequipSpell()
    {
        if (currentSpellInstance != null) Destroy(currentSpellInstance.gameObject);
        currentSpellData = null;

        Debug.Log("Spell Unequipped and returned to shelf.");
        OnSpellUnequipped?.Invoke();
    }

    private void TryCastSpell()
    {
        if (currentSpellInstance != null)
        {
            currentSpellInstance.CastSpell(castPoint);
        }
        else
        {
            Debug.Log("No spell equipped!");
        }
    }

    private void ToggleSpellMenu()
    {
        if (currentSpellData != null && uiManager != null)
        {
            uiManager.ToggleMenu(currentSpellData);
        }
    }
}