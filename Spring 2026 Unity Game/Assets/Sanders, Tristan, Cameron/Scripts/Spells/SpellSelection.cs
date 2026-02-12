using UnityEngine;
using UnityEngine.UI;

public class SpellSelection : MonoBehaviour
{
    [Header("Spell Slots")]
    public Image[] spellSlots;

    [Header("Input Reader")]
    [SerializeField] private InputReader _inputReader;

    [Header("Input Action Names")]
    [SerializeField] private string numSelectActionName = "NumSelect";
    [SerializeField] private string actionMapName = "Player";

    [Header("Selection Settings")]
    public Color defaultColor = Color.white;
    public Color selectedColor = Color.yellow;

    private int currentSpellIndex = 0;
    private UnityEngine.InputSystem.InputAction _numSelectAction;
    private System.Action<UnityEngine.InputSystem.InputAction.CallbackContext> _numSelectHandler;

    void Start()
    {
        UpdateSelectionVisual();
    }

    private void OnEnable()
    {
        if (_inputReader == null)
        {
            Debug.LogWarning("SpellSelection: InputReader is not assigned.");
            return;
        }

        var actions = _inputReader.Actions;
        var actionMap = actions.asset.FindActionMap(actionMapName, false);
        if (actionMap != null && !actionMap.enabled)
            actionMap.Enable();

        _numSelectAction = actionMap != null
            ? actionMap.FindAction(numSelectActionName, false)
            : actions.FindAction(numSelectActionName, false);

        if (_numSelectAction == null)
        {
            Debug.LogWarning($"SpellSelection: Could not find action '{numSelectActionName}' in map '{actionMapName}'.");
            return;
        }

        _numSelectHandler = HandleNumSelect;
        _numSelectAction.performed += _numSelectHandler;
    }

    private void OnDisable()
    {
        if (_numSelectAction == null || _numSelectHandler == null)
            return;

        _numSelectAction.performed -= _numSelectHandler;
        _numSelectAction = null;
        _numSelectHandler = null;
    }

    private void HandleNumSelect(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        string displayName = context.control?.displayName;
        if (!string.IsNullOrEmpty(displayName) && int.TryParse(displayName, out int number))
        {
            SelectSpell(number - 1);
            return;
        }

        string controlName = context.control?.name;
        if (!string.IsNullOrEmpty(controlName) && int.TryParse(controlName, out number))
            SelectSpell(number - 1);
    }

    public void SelectSpell(int index)
    {
        if (index < 0 || index >= spellSlots.Length)
            return;

        currentSpellIndex = index;
        UpdateSelectionVisual();
    }

    void UpdateSelectionVisual()
    {
        for (int i = 0; i < spellSlots.Length; i++)
        {
            spellSlots[i].color = (i == currentSpellIndex) ? selectedColor : defaultColor;
        }
    }

    public int GetCurrentSpellIndex()
    {
        return currentSpellIndex;
    }
}
