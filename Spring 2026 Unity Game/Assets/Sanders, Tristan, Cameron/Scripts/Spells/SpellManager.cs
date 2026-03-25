using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using System;

public class SpellManager : MonoBehaviour
{
    GameObject[] spells = Array.Empty<GameObject>();
    int[] spellCharges = Array.Empty<int>();
    private int currentSpellIndex = 0;

    [Header("Input Reader")]
    [SerializeField] private InputReader _inputReader;
    private InputAction _numSelectAction;

    //Events
    public event UnityAction<int, GameObject> OnSpellSelected;
    public event UnityAction<int, int> OnChargeChanged;
    public event UnityAction OnLoadoutChanged;

    private void Awake()
    {
        SetLoadout(Array.Empty<GameObject>(), Array.Empty<int>(), 0);
    }

    private void OnEnable()
    {
        _numSelectAction = _inputReader.Actions.Player.NumSelect;
        _numSelectAction.performed += HandleNumSelect;
    }

    private void OnDisable()
    {
        if (_numSelectAction != null)
        {
            _numSelectAction.performed -= HandleNumSelect;
        }
    }

    private void HandleNumSelect(InputAction.CallbackContext context)
    {
        string displayName = context.control?.displayName;
        if (!string.IsNullOrEmpty(displayName) && int.TryParse(displayName, out int number))
        {
            TrySelectSpell(number - 1);
            return;
        }

        string controlName = context.control?.name;
        if (!string.IsNullOrEmpty(controlName) && int.TryParse(controlName, out number))
        {
            TrySelectSpell(number - 1);
        }
    }

    void TrySelectSpell(int requestedIndex)
    {
        if (spells == null || requestedIndex < 0 || requestedIndex >= spells.Length)
        {
            return;
        }

        if (spells[requestedIndex] == null)
        {
            return;
        }

        currentSpellIndex = requestedIndex;
        OnSpellSelected?.Invoke(currentSpellIndex, spells[currentSpellIndex]);
    }

    /// <summary>
    /// Returns the charges of the spell at the spellManager's current index
    /// </summary>
    /// <returns></returns>
    public int GetCurCharge()
    {
        if (spellCharges == null || currentSpellIndex < 0 || currentSpellIndex >= spellCharges.Length)
        {
            return 0;
        }

        return spellCharges[currentSpellIndex];
    }

    /// <summary>
    /// Adds number of charges to spell at the spellManager's current index
    /// </summary>
    /// <param name="change"></param>
    public void AddCurCharge(int change)
    {
        if (spellCharges == null || currentSpellIndex < 0 || currentSpellIndex >= spellCharges.Length)
        {
            return;
        }

        spellCharges[currentSpellIndex] += change;
        spellCharges[currentSpellIndex] = Mathf.Max(0, spellCharges[currentSpellIndex]);
        OnChargeChanged?.Invoke(currentSpellIndex, spellCharges[currentSpellIndex]);
    }

    public int[] GetCharges()
    {
        return spellCharges;
    }

    public void SetLoadout(GameObject[] newSpells, int[] newCharges, int defaultSelectedIndex = 0)
    {
        if (newSpells == null || newCharges == null)
        {
            spells = Array.Empty<GameObject>();
            spellCharges = Array.Empty<int>();
            currentSpellIndex = 0;
            OnLoadoutChanged?.Invoke();
            return;
        }

        int count = Mathf.Min(newSpells.Length, newCharges.Length);
        if (count <= 0)
        {
            spells = Array.Empty<GameObject>();
            spellCharges = Array.Empty<int>();
            currentSpellIndex = 0;
            OnLoadoutChanged?.Invoke();
            return;
        }

        spells = new GameObject[count];
        spellCharges = new int[count];

        Array.Copy(newSpells, spells, count);
        Array.Copy(newCharges, spellCharges, count);

        for (int i = 0; i < spellCharges.Length; i++)
        {
            spellCharges[i] = Mathf.Max(0, spellCharges[i]);
        }

        currentSpellIndex = Mathf.Clamp(defaultSelectedIndex, 0, count - 1);

        OnLoadoutChanged?.Invoke();
        for (int i = 0; i < spellCharges.Length; i++)
        {
            OnChargeChanged?.Invoke(i, spellCharges[i]);
        }

        if (spells[currentSpellIndex] != null)
        {
            OnSpellSelected?.Invoke(currentSpellIndex, spells[currentSpellIndex]);
        }
    }
}
