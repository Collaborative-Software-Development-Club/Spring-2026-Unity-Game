using UnityEngine;
using TMPro;

public class EssenceTracker : MonoBehaviour
{
    [SerializeField] EssenceCollector essenceCollector;
    TextMeshProUGUI text;
    int requiredEssence;

    void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
        SetEssenceGoal(essenceCollector);
    }

    void OnDestroy()
    {
        UnsubscribeFromCurrentGoal();
    }

    public void SetEssenceGoal(EssenceCollector newCollector)
    {
        UnsubscribeFromCurrentGoal();

        essenceCollector = newCollector;
        if (essenceCollector == null)
        {
            requiredEssence = 0;
            text.text = "Essence Collected: 0/0";
            return;
        }

        requiredEssence = essenceCollector.RequiredAmount;
        essenceCollector.OnEssenceCollected.AddListener(UpdateTracker);
        essenceCollector.OnRequiredMet.AddListener(DebugWin);
        UpdateTracker(0);
    }

    void UnsubscribeFromCurrentGoal()
    {
        if (essenceCollector == null)
        {
            return;
        }

        essenceCollector.OnEssenceCollected.RemoveListener(UpdateTracker);
        essenceCollector.OnRequiredMet.RemoveListener(DebugWin);
    }

    void UpdateTracker(int newValue)
    {
        text.text = $"Essence Collected: {newValue}/{requiredEssence}";
    }

    void DebugWin()
    {
        text.text = "Level complete! Loading next level...";
    }
}
