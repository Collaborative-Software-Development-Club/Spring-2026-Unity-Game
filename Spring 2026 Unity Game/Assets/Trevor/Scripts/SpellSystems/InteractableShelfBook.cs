using UnityEngine;

public class InteractableShelfBook : MonoBehaviour
{
    [Header("Data References")]
    [SerializeField] private SpellData mySpellData;
    [SerializeField] private PlayerProgress progressData;
    [SerializeField] private InputReader inputReader;

    [Header("Visual References")]
    [SerializeField] private SpriteRenderer bookVisuals;

    [Header("Audio")]
    [SerializeField] private AudioClip bookEquipSFX;
    [SerializeField] private AudioClip bookUnequipSFX;

    private bool isPlayerNearby = false;
    private PlayerSpellManager nearbyPlayer;

    private void OnEnable()
    {
        PlayerSpellManager.OnSpellEquipped += HandleGlobalEquipChange;
        PlayerSpellManager.OnSpellUnequipped += HandleGlobalUnequip;
        progressData.OnCollectionUpdated += CheckVisibility;

        if (inputReader != null) inputReader.InteractEvent += OnInteractPressed;
    }

    private void OnDisable()
    {
        PlayerSpellManager.OnSpellEquipped -= HandleGlobalEquipChange;
        PlayerSpellManager.OnSpellUnequipped -= HandleGlobalUnequip;
        progressData.OnCollectionUpdated -= CheckVisibility;

        if (inputReader != null) inputReader.InteractEvent -= OnInteractPressed;
    }

    private void Start() => CheckVisibility();

    private void OnInteractPressed()
    {
        if (isPlayerNearby && nearbyPlayer != null && progressData.IsUnlocked(mySpellData.spellID))
        {
            Interact();
        }
    }

    private void Interact()
    {
        if (nearbyPlayer.currentSpellData == mySpellData)
        {
            nearbyPlayer.UnequipSpell();
            MAIN_SFXManager.Instance.PlaySFX(bookUnequipSFX);
        }
        else
        {
            nearbyPlayer.EquipSpell(mySpellData, bookVisuals.sprite);
            MAIN_SFXManager.Instance.PlaySFX(bookEquipSFX);
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

    private void HandleGlobalEquipChange(SpellData equippedSpell, Sprite equippedSprite)
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
            PlayerSpellManager manager = other.GetComponentInParent<PlayerSpellManager>();

            if (manager != null)
            {
                nearbyPlayer = manager;
                isPlayerNearby = true;
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