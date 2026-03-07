using UnityEngine;

public class InteractableShelfBook : MonoBehaviour
{
    [Header("Data References")]
    [SerializeField] private SpellData mySpellData;
    [SerializeField] private PlayerProgress progressData;
    [SerializeField] private InputReader inputReader; // Your custom input asset

    [Header("Visual References")]
    [SerializeField] private SpriteRenderer bookVisuals;

    private bool isPlayerNearby = false;
    private PlayerSpellManager nearbyPlayer;

    private void OnEnable()
    {
        PlayerSpellManager.OnSpellEquipped += HandleGlobalEquipChange;
        PlayerSpellManager.OnSpellUnequipped += HandleGlobalUnequip;
        progressData.OnCollectionUpdated += CheckVisibility;

        // Subscribe to the InputReader event
        if (inputReader != null) inputReader.InteractEvent += OnInteractPressed;
    }

    private void OnDisable()
    {
        PlayerSpellManager.OnSpellEquipped -= HandleGlobalEquipChange;
        PlayerSpellManager.OnSpellUnequipped -= HandleGlobalUnequip;
        progressData.OnCollectionUpdated -= CheckVisibility;

        // Clean up the subscription
        if (inputReader != null) inputReader.InteractEvent -= OnInteractPressed;
    }

    private void Start() => CheckVisibility();

    // Replaces the old Update() method
    private void OnInteractPressed()
    {
        // Safety check: Ensure the player is nearby, valid, and the book is unlocked
        if (isPlayerNearby && nearbyPlayer != null && progressData.IsUnlocked(mySpellData.spellID))
        {
            Interact();
        }
    }

    private void Interact()
    {
        // If the player already has THIS book equipped, put it back
        if (nearbyPlayer.currentSpellData == mySpellData)
        {
            nearbyPlayer.UnequipSpell();
        }
        else
        {
            // Otherwise, grab this book (which auto-unequips the old one)
            nearbyPlayer.EquipSpell(mySpellData);
        }
    }

    private void CheckVisibility()
    {
        bool isUnlocked = progressData.IsUnlocked(mySpellData.spellID);
        gameObject.SetActive(isUnlocked);

        if (isUnlocked && nearbyPlayer != null)
        {
            bookVisuals.enabled = (nearbyPlayer.currentSpellData != mySpellData);
        }
    }

    private void HandleGlobalEquipChange(SpellData equippedSpell)
    {
        bookVisuals.enabled = (equippedSpell != mySpellData);
    }

    private void HandleGlobalUnequip()
    {
        bookVisuals.enabled = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Fix for the NullReferenceException: Try to get the component from the object OR its parent
            PlayerSpellManager manager = other.GetComponentInParent<PlayerSpellManager>();

            if (manager != null)
            {
                nearbyPlayer = manager;
                isPlayerNearby = true;
            }
            else
            {
                Debug.LogWarning("Player tag detected, but PlayerSpellManager is missing on this object or its parents!", other.gameObject);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (other.GetComponentInParent<PlayerSpellManager>() != null)
            {
                isPlayerNearby = false;
                nearbyPlayer = null;
            }
        }
    }
}