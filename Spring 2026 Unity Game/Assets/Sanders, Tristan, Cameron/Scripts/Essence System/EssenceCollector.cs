using UnityEngine;
using UnityEngine.Events;

public class EssenceCollector : MonoBehaviour
{
    [field: SerializeField][Tooltip("How much essence is required to move on")] public int RequiredAmount { get; private set; }
    int currentAmount;
    public UnityEvent<int> OnEssenceCollected;
    public UnityEvent OnRequiredMet;
    private void Start()
    {
        currentAmount = 0;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.CompareTag("Essence"))
        {
            Destroy(collision.gameObject);
            currentAmount++;
            if(currentAmount < RequiredAmount)
            {
                OnEssenceCollected.Invoke(currentAmount);
            }
            else
            {
                OnRequiredMet.Invoke();
            }
        }
    }
}
