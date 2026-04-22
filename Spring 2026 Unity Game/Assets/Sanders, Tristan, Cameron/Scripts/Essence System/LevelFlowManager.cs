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
    [SerializeField][Range(0f, 2f)] float gameCompleteVolume = 1.35f;

    [Header("Background Music")]
    [SerializeField] AudioSource backgroundMusicSource;
    [SerializeField] AudioClip backgroundMusicClip;
    [SerializeField][Range(0f, 1f)] float backgroundMusicVolume = 0.65f;
    [SerializeField][Min(0f)] float backgroundMusicFadeDuration = 0.75f;
    [SerializeField][Min(0f)] float finalWinBackgroundMusicFadeDuration = 2.5f;
    [SerializeField] bool fadeMusicInOnStart = true;

    [Header("Player Transition VFX")]
    [SerializeField] UnityEngine.ParticleSystem playerPoofOutPrefab;
    [SerializeField] UnityEngine.ParticleSystem playerPoofInPrefab;
    [SerializeField][Min(0f)] float playerPoofOutHoldTime = 0.3f;
    [SerializeField][Min(0f)] float playerPoofInRevealDelay = 0.15f;

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
    Coroutine backgroundMusicFadeCoroutine;

    void Awake()
    {
        ResolveReferences();
        EnsureCompletionAudioSource();
        EnsureBackgroundMusicSource();
        SubscribeToGoals();
    }

    void Start()
    {
        StartBackgroundMusic();

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

        bool isFinalLevel = currentLevelIndex >= levels.Length - 1;
        isTransitioning = true;

        if (isFinalLevel && !loopLevels)
        {
            FadeOutBackgroundMusic(finalWinBackgroundMusicFadeDuration);
            PlayCompletionAudio(gameCompleteClip, gameCompleteVolume);
            StartCoroutine(ReturnToHubAfterWinAudio());
            return;
        }

        PlayCompletionAudio(levelCompleteClip);
        StartCoroutine(AdvanceAfterDelay());
    }

    IEnumerator AdvanceAfterDelay()
    {
        yield return new WaitForSeconds(levelTransitionDelay);

        yield return PlayPlayerPoofOutAndHide();

        ClearContainers();

        int nextLevel = (currentLevelIndex + 1) % levels.Length;
        ActivateLevel(nextLevel, false);
        yield return PlayPlayerPoofInAndShow();
        isTransitioning = false;
    }

    IEnumerator ReturnToHubAfterWinAudio()
    {
        float sequenceStartTime = Time.time;
        yield return PlayPlayerPoofOutAndHide();

        ClearContainers();

        float elapsedSinceSequenceStart = Time.time - sequenceStartTime;
        float clipDuration = gameCompleteClip != null ? gameCompleteClip.length : 0f;
        float remainingClipDelay = Mathf.Max(0f, clipDuration - elapsedSinceSequenceStart);
        float remainingMusicFadeDelay = Mathf.Max(0f, finalWinBackgroundMusicFadeDuration - elapsedSinceSequenceStart);
        float delay = Mathf.Max(finalReturnDelay, remainingClipDelay, remainingMusicFadeDelay);
        if (delay > 0f)
        {
            yield return new WaitForSeconds(delay);
        }

        CompletePuzzleAndReturnToHub();
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
        EnsurePlayerVisible();
        FadeInBackgroundMusic();
    }

    public float BackgroundMusicFadeDurationSeconds => backgroundMusicClip != null ? backgroundMusicFadeDuration : 0f;

    public void FadeOutBackgroundMusic()
    {
        FadeOutBackgroundMusic(backgroundMusicFadeDuration);
    }

    public void FadeOutBackgroundMusic(float fadeDurationSeconds)
    {
        if (backgroundMusicClip == null)
        {
            return;
        }

        EnsureBackgroundMusicSource();
        if (backgroundMusicSource == null || !backgroundMusicSource.isPlaying)
        {
            return;
        }

        FadeBackgroundMusicTo(0f, true, fadeDurationSeconds);
    }

    public void FadeInBackgroundMusic()
    {
        if (backgroundMusicClip == null)
        {
            return;
        }

        EnsureBackgroundMusicSource();
        if (backgroundMusicSource == null)
        {
            return;
        }

        backgroundMusicSource.clip = backgroundMusicClip;
        if (!backgroundMusicSource.isPlaying)
        {
            backgroundMusicSource.volume = 0f;
            backgroundMusicSource.Play();
        }

        FadeBackgroundMusicTo(backgroundMusicVolume, false, backgroundMusicFadeDuration);
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

    IEnumerator PlayPlayerPoofOutAndHide()
    {
        if (player == null)
        {
            yield break;
        }

        float poofDuration = SpawnPlayerPoof(playerPoofOutPrefab, player.position);
        player.gameObject.SetActive(false);

        float holdDelay = Mathf.Max(playerPoofOutHoldTime, poofDuration);
        if (holdDelay > 0f)
        {
            yield return new WaitForSeconds(holdDelay);
        }
    }

    IEnumerator PlayPlayerPoofInAndShow()
    {
        if (player == null)
        {
            yield break;
        }

        float poofDuration = SpawnPlayerPoof(playerPoofInPrefab, player.position);
        float revealDelay = poofDuration > 0f ? Mathf.Min(playerPoofInRevealDelay, poofDuration) : 0f;
        if (revealDelay > 0f)
        {
            yield return new WaitForSeconds(revealDelay);
        }

        EnsurePlayerVisible();
    }

    void EnsurePlayerVisible()
    {
        if (player == null)
        {
            return;
        }

        if (!player.gameObject.activeSelf)
        {
            player.gameObject.SetActive(true);
        }
    }

    float SpawnPlayerPoof(UnityEngine.ParticleSystem poofPrefab, Vector3 position)
    {
        if (poofPrefab == null)
        {
            return 0f;
        }

        UnityEngine.ParticleSystem spawnedPoof = Instantiate(poofPrefab, position, Quaternion.identity);
        spawnedPoof.Play();

        float poofDuration = GetParticleDurationSeconds(spawnedPoof);
        Destroy(spawnedPoof.gameObject, poofDuration + 0.1f);
        return poofDuration;
    }

    float GetParticleDurationSeconds(UnityEngine.ParticleSystem particleSystem)
    {
        if (particleSystem == null)
        {
            return 0f;
        }

        UnityEngine.ParticleSystem.MainModule main = particleSystem.main;
        return main.duration + main.startLifetime.constantMax;
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

    void EnsureBackgroundMusicSource()
    {
        if (backgroundMusicSource == null && backgroundMusicClip != null)
        {
            backgroundMusicSource = gameObject.AddComponent<AudioSource>();
        }

        if (backgroundMusicSource == null)
        {
            return;
        }

        backgroundMusicSource.playOnAwake = false;
        backgroundMusicSource.loop = true;
        backgroundMusicSource.spatialBlend = 0f;
        backgroundMusicSource.clip = backgroundMusicClip;
    }

    void StartBackgroundMusic()
    {
        if (backgroundMusicClip == null)
        {
            return;
        }

        EnsureBackgroundMusicSource();
        if (backgroundMusicSource == null)
        {
            return;
        }

        backgroundMusicSource.clip = backgroundMusicClip;

        if (fadeMusicInOnStart)
        {
            if (!backgroundMusicSource.isPlaying)
            {
                backgroundMusicSource.Play();
            }

            backgroundMusicSource.volume = 0f;
            FadeBackgroundMusicTo(backgroundMusicVolume, false, backgroundMusicFadeDuration);
            return;
        }

        backgroundMusicSource.volume = backgroundMusicVolume;
        if (!backgroundMusicSource.isPlaying)
        {
            backgroundMusicSource.Play();
        }
    }

    void FadeBackgroundMusicTo(float targetVolume, bool stopWhenSilent, float fadeDurationSeconds)
    {
        if (backgroundMusicSource == null)
        {
            return;
        }

        if (backgroundMusicFadeCoroutine != null)
        {
            StopCoroutine(backgroundMusicFadeCoroutine);
        }

        backgroundMusicFadeCoroutine = StartCoroutine(FadeBackgroundMusicRoutine(targetVolume, stopWhenSilent, fadeDurationSeconds));
    }

    IEnumerator FadeBackgroundMusicRoutine(float targetVolume, bool stopWhenSilent, float fadeDurationSeconds)
    {
        float startVolume = backgroundMusicSource.volume;

        if (fadeDurationSeconds <= 0f)
        {
            backgroundMusicSource.volume = targetVolume;
            if (stopWhenSilent && targetVolume <= 0f)
            {
                backgroundMusicSource.Stop();
            }

            backgroundMusicFadeCoroutine = null;
            yield break;
        }

        float elapsed = 0f;
        while (elapsed < fadeDurationSeconds)
        {
            elapsed += Time.deltaTime;
            backgroundMusicSource.volume = Mathf.Lerp(startVolume, targetVolume, elapsed / fadeDurationSeconds);
            yield return null;
        }

        backgroundMusicSource.volume = targetVolume;
        if (stopWhenSilent && targetVolume <= 0f)
        {
            backgroundMusicSource.Stop();
        }

        backgroundMusicFadeCoroutine = null;
    }

    void PlayCompletionAudio(AudioClip clip)
    {
        PlayCompletionAudio(clip, completionVolume);
    }

    void PlayCompletionAudio(AudioClip clip, float volume)
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

        completionAudioSource.PlayOneShot(clip, volume);
    }
}
