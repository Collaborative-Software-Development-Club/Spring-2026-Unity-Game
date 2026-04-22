using UnityEngine;
using UnityEngine.InputSystem;

//Vile coupling with SpellSelection
public class SpellCasting : MonoBehaviour
{
    
    [SerializeField] SpellManager spellManager;
    [SerializeField] private float rotationStep = 15f;
    [SerializeField][Tooltip("Parent object for all spawned spells")] Transform spellContainer;

    private EssenceInput.SpellCastingActions spellActions;
    private GameObject spellToPlace;
    private float currentRotation = 0f;


    private void Awake()
    {
        spellActions = new EssenceInput().SpellCasting;

        if (spellContainer == null)
        {
            Debug.LogWarning($"{name}: Spell Container is not assigned. Spawned spells will not be grouped under a shared parent.");
        }
    }

    private void OnEnable()
    {
        spellActions.Enable();
        spellActions.Cast.performed += Cast;
        spellActions.Rotate.performed += Rotate;
    }

    private void OnDisable()
    {
        spellActions.Cast.performed -= Cast;
        spellActions.Rotate.performed -= Rotate;
        spellManager.OnSpellSelected -= Display;
        spellActions.Disable();
    }

    private void Start()
    {
        spellManager.OnSpellSelected += Display;
    }

    private void Update()
    {
        if (!IsSpellSelected()) return;
       
        UpdateSpellPosition();
    }

    //Updates the spells to be where the mouse is and determines the color the spell should be based on visibility
    private void UpdateSpellPosition()
    {
        spellToPlace.transform.position = GetMousePosition();
        spellToPlace.GetComponent<EssenceMover>().DisplayPlaceability(CanSeeLocation());
        
    }

    private void Cast(InputAction.CallbackContext ctx)
    {
        if (!CanSeeLocation()) return;

        if (spellManager.GetCurCharge() <= 0) return;

        spellManager.AddCurCharge(-1);
        spellToPlace.GetComponent<Collider2D>().enabled = true;
        spellToPlace = null;
    }

    private void Rotate(InputAction.CallbackContext ctx)
    {
        if (!IsSpellSelected()) return;

        float scroll = ctx.ReadValue<float>();
        currentRotation += scroll * rotationStep;
        spellToPlace.transform.rotation = Quaternion.Euler(0f, 0f, currentRotation);
    }

    private void Display(int idx, GameObject spell)
    {
        if (spell == null)
        {
            return;
        }

        if (spellToPlace != null)
        {
            Destroy(spellToPlace);
        }

        Vector3 mousePos = GetMousePosition();

        spellToPlace = Instantiate(spell, mousePos, Quaternion.identity, spellContainer);
        spellToPlace.GetComponent<Collider2D>().enabled = false;
    }

    public void ClearPlacedSpells()
    {
        if (spellToPlace != null)
        {
            Destroy(spellToPlace);
            spellToPlace = null;
        }

        if (spellContainer == null)
        {
            return;
        }

        for (int i = spellContainer.childCount - 1; i >= 0; i--)
        {
            Destroy(spellContainer.GetChild(i).gameObject);
        }
    }

    private bool CanSeeLocation()
    {
        if (!IsSpellSelected()) return false;

        RaycastHit2D hit = Physics2D.Linecast(transform.position, spellToPlace.transform.position, LayerMask.GetMask("Interactable"));
        return hit.collider == null;
    }

    private bool IsSpellSelected()
    {
        return spellToPlace != null;
    }

    private Vector2 GetMousePosition()
    {
        Vector3 mouseScreen = Mouse.current.position.ReadValue();
        mouseScreen.z = -Camera.main.transform.position.z;
        return Camera.main.ScreenToWorldPoint(mouseScreen);
    }
}
