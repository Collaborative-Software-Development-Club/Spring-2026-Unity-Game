using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class SpellManager : MonoBehaviour
{
    [Header("Spell Casting")]
    [SerializeField] GameObject[] spells;
    [SerializeField][Tooltip("How many charges the player has of each spell")] int[] spellCharges;
    private int[] remainingCharges;
    private int currentSpellIndex = 0;

    [Header("Input Reader")]
    [SerializeField] private InputReader _inputReader;
    private InputAction _numSelectAction;

    //Events
    public event UnityAction<int, GameObject> OnSpellSelected;
    public event UnityAction<int, int> OnChargeChanged;

    private void Awake()
    {
        remainingCharges = new int[spellCharges.Length];
        spellCharges.CopyTo(remainingCharges, 0);
    }

    private void OnEnable()
    {
        _numSelectAction = _inputReader.Actions.Player.NumSelect;
        _numSelectAction.performed += HandleNumSelect;
    }

    private void OnDisable()
    {
        _numSelectAction.performed -= HandleNumSelect;
    }

    private void HandleNumSelect(InputAction.CallbackContext context)
    {
        string displayName = context.control?.displayName;
        if (!string.IsNullOrEmpty(displayName) && int.TryParse(displayName, out int number))
        {
            currentSpellIndex = number - 1;
            OnSpellSelected.Invoke(currentSpellIndex, spells[currentSpellIndex]);
            return;
        }

        string controlName = context.control?.name;
        if (!string.IsNullOrEmpty(controlName) && int.TryParse(controlName, out number))
        {
            currentSpellIndex = number - 1;
            OnSpellSelected.Invoke(currentSpellIndex, spells[currentSpellIndex]);
        }
    }

    /// <summary>
    /// Returns the charges of the spell at the spellManager's current index
    /// </summary>
    /// <returns></returns>
    public int GetCurCharge()
    {
        return spellCharges[currentSpellIndex];
    }

    /// <summary>
    /// Adds number of charges to spell at the spellManager's current index
    /// </summary>
    /// <param name="change"></param>
    public void AddCurCharge(int change)
    {
        spellCharges[currentSpellIndex] += change;
        OnChargeChanged.Invoke(currentSpellIndex, spellCharges[currentSpellIndex]);
    }

    public int[] GetCharges()
    {
        return remainingCharges;
    }
}
