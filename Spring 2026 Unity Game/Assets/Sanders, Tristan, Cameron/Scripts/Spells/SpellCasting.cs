using UnityEngine;
using UnityEngine.InputSystem;

//Vile coupling with SpellSelection
public class SpellCasting : MonoBehaviour
{
    [SerializeField] GameObject[] spells;
    [SerializeField] int[] spellCharges;
    [SerializeField] SpellSelection spellSelection;
    [SerializeField] private float rotationStep = 15f;

    private EssenceInput essenceInput;
    private EssenceInput.SpellCastingActions spellActions;
    private GameObject spellToPlace;
    private float currentRotation = 0f;
    private int[] remainingCharges;

    private void Awake()
    {
        essenceInput = new EssenceInput();
        remainingCharges = new int[spellCharges.Length];
        spellCharges.CopyTo(remainingCharges, 0);
    }

    private void OnEnable()
    {
        spellActions = essenceInput.SpellCasting;
        spellActions.Enable();
        spellActions.Cast.performed += Cast;
        spellActions.Rotate.performed += Rotate;
    }

    private void OnDisable()
    {
        spellActions.Cast.performed -= Cast;
        spellActions.Rotate.performed -= Rotate;
        spellActions.Disable();
    }

    private void OnDestroy()
    {
        essenceInput.Dispose();
    }

    private void Start()
    {
        Display();
        InitializeSpellCounts();
    }

    private void Update()
    {
        if (spellToPlace == null) return;

        Vector3 mouseScreen = Mouse.current.position.ReadValue();
        mouseScreen.z = -Camera.main.transform.position.z;
        spellToPlace.transform.position = Camera.main.ScreenToWorldPoint(mouseScreen);
    }

    private void Cast(InputAction.CallbackContext ctx)
    {
        if (spellToPlace == null) return;

        int curIndex = spellSelection.GetCurrentSpellIndex();
        if (remainingCharges[curIndex] <= 0) return;

        remainingCharges[curIndex]--;
        spellToPlace.GetComponent<Collider2D>().enabled = true;
        spellToPlace = null;
        spellSelection.UpdateSpellCount(curIndex, remainingCharges[curIndex]);
        Display();
    }

    private void Rotate(InputAction.CallbackContext ctx)
    {
        if (spellToPlace == null) return;

        float scroll = ctx.ReadValue<float>();
        currentRotation -= scroll * rotationStep;
        spellToPlace.transform.rotation = Quaternion.Euler(0f, 0f, currentRotation);
    }

    private void Display()
    {
        int curIndex = spellSelection.GetCurrentSpellIndex();

        if (remainingCharges[curIndex] <= 0)
        {
            if (spellToPlace != null)
            {
                Destroy(spellToPlace);
                spellToPlace = null;
            }
            return;
        }

        currentRotation = 0f;
        Vector3 mouseScreen = Mouse.current.position.ReadValue();
        mouseScreen.z = -Camera.main.transform.position.z;
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(mouseScreen);

        spellToPlace = Instantiate(spells[curIndex], worldPos, Quaternion.identity);
        spellToPlace.GetComponent<Collider2D>().enabled = false;
    }

    public int GetRemainingCharges(int spellIndex) => remainingCharges[spellIndex];

    public void RefillCharges(int spellIndex) => remainingCharges[spellIndex] = spellCharges[spellIndex];

    private void InitializeSpellCounts()
    {
        for(int i = 0; i < remainingCharges.Length; i++)
        {
            spellSelection.UpdateSpellCount(i, remainingCharges[i]);
        }
    }
}
