using UnityEngine;
using UnityEngine.Events;

public class EssenceCollector : MonoBehaviour
{
    [field: SerializeField][Tooltip("How much essence is required to move on")] public int RequiredAmount { get; private set; }
    public int CurrentAmount => currentAmount;

    int currentAmount;
    bool isCompleted;

    public UnityEvent<int> OnEssenceCollected;
    public UnityEvent OnRequiredMet;
    public UnityEvent<EssenceCollector> OnGoalCompleted;

    private void Start()
    {
        ResetCollector(true);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isCompleted)
        {
            return;
        }

        if(collision.collider.CompareTag("Essence"))
        {
            Destroy(collision.gameObject);
            currentAmount++;
            OnEssenceCollected.Invoke(currentAmount);

            if(currentAmount >= RequiredAmount)
            {
                isCompleted = true;
                OnRequiredMet.Invoke();
                OnGoalCompleted?.Invoke(this);
            }
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
}
