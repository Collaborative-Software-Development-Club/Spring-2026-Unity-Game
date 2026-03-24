using UnityEngine;
using TMPro;

public class EssenceTracker : MonoBehaviour
{
    [SerializeField] EssenceCollector essenceCollector;
    TextMeshProUGUI text;
    int requiredEssence;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        requiredEssence = essenceCollector.RequiredAmount;
        essenceCollector.OnEssenceCollected.AddListener(UpdateTracker);
        essenceCollector.OnRequiredMet.AddListener(DebugWin);
        text = GetComponent<TextMeshProUGUI>();
        text.text = $"Essence Collected: 0/{requiredEssence}";
    }

    void UpdateTracker(int newValue)
    {
        text.text = $"Essence Collected: {newValue}/{requiredEssence}";
    }

    void DebugWin()
    {
        text.text = "You win";
    }
}
