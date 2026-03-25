using System.Collections;
using UnityEngine;

public class LevelFlowManager : MonoBehaviour
{
    [System.Serializable]
    public class LevelDefinition
    {
        public string levelName;
        [Tooltip("Environment root to enable when this level is active")]
        public GameObject environmentRoot;
        [Tooltip("Player spawn used when entering this level")]
        public Transform playerSpawnPoint;
        [Tooltip("Essence goal for this level")]
        public EssenceCollector essenceGoal;
        [Tooltip("Spells available in this level")]
        public GameObject[] spells;
        [Tooltip("Spell charges for this level (same order as spells)")]
        public int[] spellCharges;
    }

    [Header("Core References")]
    [SerializeField] Transform player;
    [SerializeField] EssenceTracker essenceTracker;
    [SerializeField] SpellManager spellManager;
    [SerializeField] SpellCasting spellCasting;

    [Header("Containers To Clear")]
    [SerializeField] Transform essenceContainer;

    [Header("Flow")]
    [SerializeField] float levelTransitionDelay = 3f;
    [SerializeField] LevelDefinition[] levels;

    int currentLevelIndex;
    bool isTransitioning;

    void Awake()
    {
        ResolveReferences();
        SubscribeToGoals();
    }

    void Start()
    {
        if (levels == null || levels.Length == 0)
        {
            Debug.LogWarning($"{name}: No levels configured in LevelFlowManager.");
            return;
        }

        currentLevelIndex = Mathf.Clamp(currentLevelIndex, 0, levels.Length - 1);
        ActivateLevel(currentLevelIndex, true);
    }

    void OnDestroy()
    {
        UnsubscribeFromGoals();
    }

    void ResolveReferences()
    {
        if (player == null)
        {
            GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
            if (playerObject != null)
            {
                player = playerObject.transform;
            }
        }

        if (essenceTracker == null)
        {
            essenceTracker = FindFirstObjectByType<EssenceTracker>();
        }

        if (spellManager == null)
        {
            spellManager = FindFirstObjectByType<SpellManager>();
        }

        if (spellCasting == null)
        {
            spellCasting = FindFirstObjectByType<SpellCasting>();
        }

    }

    void SubscribeToGoals()
    {
        if (levels == null)
        {
            return;
        }

        for (int i = 0; i < levels.Length; i++)
        {
            if (levels[i].essenceGoal != null)
            {
                levels[i].essenceGoal.OnGoalCompleted.AddListener(HandleGoalCompleted);
            }
        }
    }

    void UnsubscribeFromGoals()
    {
        if (levels == null)
        {
            return;
        }

        for (int i = 0; i < levels.Length; i++)
        {
            if (levels[i].essenceGoal != null)
            {
                levels[i].essenceGoal.OnGoalCompleted.RemoveListener(HandleGoalCompleted);
            }
        }
    }

    void HandleGoalCompleted(EssenceCollector completedGoal)
    {
        if (isTransitioning || levels == null || levels.Length == 0)
        {
            return;
        }

        LevelDefinition activeLevel = levels[currentLevelIndex];
        if (activeLevel.essenceGoal != completedGoal)
        {
            return;
        }

        StartCoroutine(AdvanceAfterDelay());
    }

    IEnumerator AdvanceAfterDelay()
    {
        isTransitioning = true;
        yield return new WaitForSeconds(levelTransitionDelay);

        ClearContainers();

        int nextLevel = (currentLevelIndex + 1) % levels.Length;
        ActivateLevel(nextLevel, false);
        isTransitioning = false;
    }

    void ActivateLevel(int levelIndex, bool isInitialLevel)
    {
        currentLevelIndex = levelIndex;

        for (int i = 0; i < levels.Length; i++)
        {
            if (levels[i].environmentRoot != null)
            {
                levels[i].environmentRoot.SetActive(i == currentLevelIndex);
            }
        }

        LevelDefinition level = levels[currentLevelIndex];

        if (level.essenceGoal != null)
        {
            level.essenceGoal.ResetCollector(true);
            if (essenceTracker != null)
            {
                essenceTracker.SetEssenceGoal(level.essenceGoal);
            }
        }
        else if (essenceTracker != null)
        {
            essenceTracker.SetEssenceGoal(null);
        }

        ApplyLevelLoadout(level);

        if (!isInitialLevel)
        {
            TeleportPlayer(level.playerSpawnPoint);
        }
    }

    void ApplyLevelLoadout(LevelDefinition level)
    {
        if (spellManager == null)
        {
            return;
        }

        if (level.spells == null || level.spellCharges == null)
        {
            spellManager.SetLoadout(System.Array.Empty<GameObject>(), System.Array.Empty<int>());
            return;
        }

        spellManager.SetLoadout(level.spells, level.spellCharges, 0);
    }

    void TeleportPlayer(Transform spawnPoint)
    {
        if (player == null || spawnPoint == null)
        {
            return;
        }

        Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
        }

        player.position = spawnPoint.position;
        player.rotation = spawnPoint.rotation;
    }

    void ClearContainers()
    {
        if (spellCasting != null)
        {
            spellCasting.ClearPlacedSpells();
        }

        ClearChildren(essenceContainer);
    }

    void ClearChildren(Transform container)
    {
        if (container == null)
        {
            return;
        }

        for (int i = container.childCount - 1; i >= 0; i--)
        {
            Destroy(container.GetChild(i).gameObject);
        }
    }
}
