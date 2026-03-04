using UnityEngine;
using TMPro;

public class WaterUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI waterAmount; 

    void Update()
    {
        waterAmount.text = "Water: " + PlayerInv.water.ToString();
    }
}
