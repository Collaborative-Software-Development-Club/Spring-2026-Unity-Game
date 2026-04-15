using UnityEngine;
using UnityEngine.Events;

public class EssenceCollector : MonoBehaviour
{
    [field: SerializeField][Tooltip("How much essence is required to move on")] public int RequiredAmount { get; private set; }

    [Header("Audio")]
    [SerializeField] AudioSource collectionAudioSource;
    [SerializeField] AudioClip collectionClip;
    [SerializeField][Range(0f, 1f)] float collectionVolume = 1f;

    [Header("Debug")]
    [SerializeField][Tooltip("Enable dev-only input to manually add essence during play mode")]
    bool enableDebugEssenceInput;
    [SerializeField] KeyCode debugAddEssenceKey = KeyCode.Equals;
    [SerializeField][Min(1)] int debugAddAmount = 1;

    public int CurrentAmount => currentAmount;

    int currentAmount;
    bool isCompleted;

    public UnityEvent<int> OnEssenceCollected;
    public UnityEvent OnRequiredMet;
    public UnityEvent<EssenceCollector> OnGoalCompleted;

    private void Awake()
    {
        EnsureCollectionAudioSource();
    }

    private void Start()
    {
        ResetCollector(true);
    }

    private void Update()
    {
        if (!enableDebugEssenceInput || isCompleted)
        {
            return;
        }

        if (Input.GetKeyDown(debugAddEssenceKey))
        {
            CollectEssence(debugAddAmount);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isCompleted)
        {
            return;
        }

        if (collision.collider.CompareTag("Essence"))
        {
            PlayCollectionAudio();
            Destroy(collision.gameObject);
            CollectEssence(1);
        }
    }

    [ContextMenu("Debug/Add 1 Essence")]
    void DebugAddOneEssence()
    {
        if (!Application.isPlaying)
        {
            return;
        }

        CollectEssence(1);
    }

    public void DebugAddEssence(int amount)
    {
        if (!Application.isPlaying)
        {
            return;
        }

        CollectEssence(amount);
    }

    void CollectEssence(int amount)
    {
        if (isCompleted)
        {
            return;
        }

        int clampedAmount = Mathf.Max(1, amount);
        currentAmount += clampedAmount;
        OnEssenceCollected?.Invoke(currentAmount);

        if(currentAmount >= RequiredAmount)
        {
            isCompleted = true;
            OnRequiredMet?.Invoke();
            OnGoalCompleted?.Invoke(this);
        }
    }

    public void ResetCollector(bool notifyUI)
    {
        currentAmount = 0;
        isCompleted = false;

        if (notifyUI)
        {
            OnEssenceCollected?.Invoke(currentAmount);
        }
    }

    void EnsureCollectionAudioSource()
    {
        if (collectionAudioSource == null && collectionClip != null)
        {
            collectionAudioSource = gameObject.AddComponent<AudioSource>();
        }

        if (collectionAudioSource == null)
        {
            return;
        }

        collectionAudioSource.playOnAwake = false;
        collectionAudioSource.loop = false;
        collectionAudioSource.spatialBlend = 0f;
    }

    void PlayCollectionAudio()
    {
        if (collectionClip == null)
        {
            return;
        }

        EnsureCollectionAudioSource();
        if (collectionAudioSource == null)
        {
            return;
        }

        collectionAudioSource.PlayOneShot(collectionClip, collectionVolume);
    }
}
