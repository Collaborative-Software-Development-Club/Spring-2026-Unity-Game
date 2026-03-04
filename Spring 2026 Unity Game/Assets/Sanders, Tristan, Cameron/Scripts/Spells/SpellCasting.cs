using UnityEngine;
using UnityEngine.InputSystem;

public class SpellCasting : MonoBehaviour
{
    [SerializeField] GameObject[] spells;
    [SerializeField] SpellSelection spellSelection;

    private EssenceInput essenceInput;
    private EssenceInput.SpellCastingActions spellActions;

    private void Awake()
    {
        essenceInput = new EssenceInput();
    }
    private void OnEnable()
    {
        spellActions = essenceInput.SpellCasting;
        spellActions.Enable();
        spellActions.Cast.performed += Cast;
    }

    private void OnDisable()
    {
        spellActions.Cast.performed -= Cast;
    }

    private void Cast(InputAction.CallbackContext ctx)
    {
        int curIndex = spellSelection.GetCurrentSpellIndex();
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        Instantiate(spells[curIndex], mousePos, Quaternion.identity);
    }
}
