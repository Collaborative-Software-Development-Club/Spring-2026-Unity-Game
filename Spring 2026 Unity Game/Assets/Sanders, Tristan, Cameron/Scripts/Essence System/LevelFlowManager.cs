using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    [Header("Audio")]
    [SerializeField] AudioSource completionAudioSource;
    [SerializeField] AudioClip levelCompleteClip;
    [SerializeField] AudioClip gameCompleteClip;
    [SerializeField][Range(0f, 1f)] float completionVolume = 1f;

    [Header("Completion / Return To Hub")]
    [SerializeField] bool loopLevels = false;
    [SerializeField] float finalReturnDelay = 0.25f;
    [SerializeField] PlayerProgress progressData;
    [SerializeField] LevelID unlockedBook = LevelID.None;
    [SerializeField] string hubSceneName = "MageLibrary";
    [SerializeField] BookUnlocker bookUnlocker;
    [SerializeField] SceneReturner sceneReturner;

    int currentLevelIndex;
    bool isTransitioning;

    void Awake()
    {
        ResolveReferences();
        EnsureCompletionAudioSource();
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

        PlayCompletionAudio(levelCompleteClip);
        StartCoroutine(AdvanceAfterDelay());
    }

    IEnumerator AdvanceAfterDelay()
    {
        isTransitioning = true;
        yield return new WaitForSeconds(levelTransitionDelay);

        ClearContainers();

        bool isFinalLevel = currentLevelIndex >= levels.Length - 1;
        if (isFinalLevel && !loopLevels)
        {
            PlayCompletionAudio(gameCompleteClip);

            if (finalReturnDelay > 0f)
            {
                yield return new WaitForSeconds(finalReturnDelay);
            }

            CompletePuzzleAndReturnToHub();
            yield break;
        }

        int nextLevel = (currentLevelIndex + 1) % levels.Length;
        ActivateLevel(nextLevel, false);
        isTransitioning = false;
    }

    public void RestartCurrentLevel()
    {
        if (levels == null || levels.Length == 0)
        {
            return;
        }

        StopAllCoroutines();
        isTransitioning = false;
        ClearContainers();
        ActivateLevel(currentLevelIndex, false);
    }

    void CompletePuzzleAndReturnToHub()
    {
        if (progressData != null && unlockedBook != LevelID.None)
        {
            progressData.UnlockBook(unlockedBook);
        }
        else if (bookUnlocker != null)
        {
            bookUnlocker.Unlock();
        }
        else
        {
            Debug.LogWarning($"{name}: No book unlock target configured for final level completion.");
        }

        isTransitioning = false;

        if (sceneReturner != null)
        {
            sceneReturner.ReturnToLibrary();
            return;
        }

        if (!string.IsNullOrWhiteSpace(hubSceneName))
        {
            SceneManager.LoadScene(hubSceneName);
        }
        else
        {
            Debug.LogError($"{name}: Hub scene name is empty, cannot return after final level.");
        }
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

    void EnsureCompletionAudioSource()
    {
        if (completionAudioSource == null && (levelCompleteClip != null || gameCompleteClip != null))
        {
            completionAudioSource = gameObject.AddComponent<AudioSource>();
        }

        if (completionAudioSource == null)
        {
            return;
        }

        completionAudioSource.playOnAwake = false;
        completionAudioSource.loop = false;
        completionAudioSource.spatialBlend = 0f;
    }

    void PlayCompletionAudio(AudioClip clip)
    {
        if (clip == null)
        {
            return;
        }

        EnsureCompletionAudioSource();
        if (completionAudioSource == null)
        {
            return;
        }

        completionAudioSource.PlayOneShot(clip, completionVolume);
    }
}
