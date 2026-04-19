using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class TutorialUI : MonoBehaviour
{
    public Loottable starterLootbox;
    
    [Header("Arrow")]
    public GameObject arrowPrefab;
    private GameObject arrowInstance;

    private Transform currentArrowTarget;
    public Transform player;

    private readonly float detectionDistance = 3f;

    // Movement completion
    private bool wDone, aDone, sDone, dDone;

    // Tutorial states
    
    /*
    private bool arrivedAtShop;
    private bool openedShop;
    private bool boughtLootbox;
    private bool closedShop;
    */

    private bool openedLootboxUI;
    private bool openedLootbox;
    //private bool closedLootboxUI;

    private bool arrivedAtMachine;
    private bool usedMachine;

    private bool acceptedContract;
    private bool turnedInContract;

    // Input
    private InputAction wAction, aAction, sAction, dAction, interactAction;

    // Targets
    //private GameObject shop;
    private GameObject _machine;
    private GameObject _contractBoard;
    
    // Random landlord flavor lines
    private string[] landlordExtras =
    {
        "Please generate income faster.",
        "Your previous tenant record is concerning.",
        "I believe in you. Barely.",
        "This building runs on rent.",
        "Failure is... financially inconvenient."
    };

    private void OnEnable()
    {
        wAction = new InputAction(type: InputActionType.Button, binding: "<Keyboard>/w");
        wAction.performed += ctx => wDone = true;
        wAction.Enable();

        aAction = new InputAction(type: InputActionType.Button, binding: "<Keyboard>/a");
        aAction.performed += ctx => aDone = true;
        aAction.Enable();

        sAction = new InputAction(type: InputActionType.Button, binding: "<Keyboard>/s");
        sAction.performed += ctx => sDone = true;
        sAction.Enable();

        dAction = new InputAction(type: InputActionType.Button, binding: "<Keyboard>/d");
        dAction.performed += ctx => dDone = true;
        dAction.Enable();

        //interactAction = new InputAction(type: InputActionType.Button, binding: "<Keyboard>/e");
        //interactAction.performed += ctx => openedShop = true;
        //interactAction.Enable();
    }

    private void OnDisable()
    {
        wAction.Dispose();
        aAction.Dispose();
        sAction.Dispose();
        dAction.Dispose();
        //interactAction.Dispose();
    }

    private void Start()
    {
        SetupTutorial();
    }

    public void SetupTutorial()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;

        //shop = GameObject.Find("Shop");
        _machine = GameObject.Find("Adder");
        _contractBoard = GameObject.Find("Contract Terminal");

        GameManager.Instance.ContractManager.onContractTaken += () => acceptedContract = true;
        GameManager.Instance.ContractManager.onContractTurnedIn += c => turnedInContract = true;

        GameManager.Instance.GUIManager.lootboxUIRef.onLootboxOpened += l => openedLootbox = true;
        GameManager.Instance.GUIManager.lootboxUIRef.onLootboxUIOpened += () => openedLootboxUI = true;
        //GameManager.Instance.GUIManager.lootboxUIRef.onLootboxUIClosed += () => closedLootboxUI = true;

        _machine.GetComponent<MachineInteraction>().onMachineUsed += OnMachineUsed;
            
        StartCoroutine(
            NotificationUI.Instance.RequestConfirmation(
                "Skip tutorial?",
                "This action cannot be undone!",
                result =>
                {
                    if (result)
                    {
                        GameManager.Instance.playerInventory.AddItemToInventory(new Lootbox(starterLootbox), 1);
                        StartGame();
                    }
                    else
                    {
                        StartCoroutine(RunTutorial());
                    }
                }
            )
        );
    }

    private void Update()
    {
        UpdateArrow();

        //if (!arrivedAtShop && shop != null)
        //{
        //    if (Vector3.Distance(player.position, shop.transform.position) <= detectionDistance)
        //        arrivedAtShop = true;
        //}

        if (!arrivedAtMachine && _machine != null)
        {
            if (Vector3.Distance(player.position, _machine.transform.position) <= detectionDistance)
                arrivedAtMachine = true;
        }
    }

    private IEnumerator RunTutorial()
    {
        yield return LandlordMessage(
            "Welcome homeless brainrot maker.\n\nUse W/A/S/D to move.\nStanding still will not generate rent."
        );

        yield return new WaitUntil(() => wDone && aDone && sDone && dDone);

        yield return LandlordMessage(
            "You currently own ZERO Brainrots.\n\nThis is financially unacceptable.\nI'll give you a loot-box to get started.\n\n" + RandomLandlordLine()
        );
        
        GameManager.Instance.playerInventory.AddItemToInventory(new Lootbox(starterLootbox), 1);

        /*
        SpawnArrowAt(shop.transform);
        yield return new WaitUntil(() => arrivedAtShop);
        RemoveArrow();

        yield return LandlordMessage(
            "Press E to interact with the shop.\nTry not to break anything."
        );
        
        yield return new WaitUntil(() => openedShop);

        yield return LandlordMessage(
            "Buy a lootbox.\n\nInside could be something profitable.\nOr not.\n\nI still expect rent."
        );

        yield return new WaitUntil(() => boughtLootbox);

        yield return LandlordMessage(
            "Close the shop UI.\n\nWe cannot afford impulse purchases."
        );

        yield return new WaitUntil(() => closedShop);
        */

        yield return LandlordMessage(
            "Right click the lootbox in your inventory.\n\nLet's see how lucky you are."
        );

        yield return new WaitUntil(() => openedLootboxUI);

        yield return LandlordMessage(
            "Press OPEN.\n\nConsulting the random number gods..."
        );

        yield return new WaitUntil(() => openedLootbox);

        yield return LandlordMessage(
            "Take your new Brainrot to the Adder machine.\n\nMake it more profitable.\n\n" + RandomLandlordLine()
        );

        SpawnArrowAt(_machine.transform);
        yield return new WaitUntil(() => arrivedAtMachine);
        RemoveArrow();

        yield return LandlordMessage(
            "Insert the Brainrot into the machine.\n\nTry to improve it."
        );

        yield return new WaitUntil(() => usedMachine);

        yield return LandlordMessage(
            "Contracts are how tenants generate income. These only occur in the Morning!"
        );

        SpawnArrowAt(_contractBoard.transform);
        yield return new WaitUntil(() => acceptedContract);
        RemoveArrow();

        yield return LandlordMessage(
            "Turn in a Brainrot that matches the contract.\n\nTry not to embarrass yourself."
        );

        yield return new WaitUntil(() => turnedInContract);

        yield return LandlordMessage(
            "FINAL NOTICE\n\nRent is due soon.\n\nFailure to pay will result in eviction."
        );

        StartGame();
    }

    private IEnumerator LandlordMessage(string message)
    {
        NotificationUI.Instance.ShowNotification(new NotificationUI.NotificationData()
        {
            type = NotificationUI.NotificationType.CloseButton,
            title = "BUILDING MANAGEMENT NOTICE",
            subtitle = message
        });

        yield return new WaitUntil(() => !NotificationUI.Instance.IsShowing);
    }

    private string RandomLandlordLine()
    {
        return landlordExtras[Random.Range(0, landlordExtras.Length)];
    }

    private void SpawnArrowAt(Transform target)
    {
        currentArrowTarget = target;

        if (arrowInstance == null)
            arrowInstance = Instantiate(arrowPrefab);

        arrowInstance.SetActive(true);
    }

    private void RemoveArrow()
    {
        if (arrowInstance != null)
            arrowInstance.SetActive(false);
    }

    private void UpdateArrow()
    {
        if (arrowInstance == null || !arrowInstance.activeSelf || currentArrowTarget == null)
            return;

        arrowInstance.transform.position = player.position + Vector3.up * 2f;

        Vector3 dir = currentArrowTarget.position - arrowInstance.transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        arrowInstance.transform.rotation = Quaternion.Euler(0, 0, angle - 90f);
    }

    // Hooks called from other systems

    /*
    public void OnBoughtLootbox()
    {
        boughtLootbox = true;
    }

    public void OnClosedShop()
    {
        closedShop = true;
    }
    */

    public void OnMachineUsed()
    {
        usedMachine = true;
    }

    private void StartGame()
    {
        NotificationUI.Instance.HideUI();
        GameManager.Instance.NextState();
        
        GameManager.Instance.playerInventory.AddItemToInventory(new Lootbox(starterLootbox), 4);
    }
}