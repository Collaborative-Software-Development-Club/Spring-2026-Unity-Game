using TMPro;
using UnityEngine;

public class JEC_KeyCollectable : MonoBehaviour
{
    [SerializeField] private JEC_Key keyData;

    private string character;

    private void Start()
    {
        if (keyData != null)
        {
            character = keyData.character;
        }
        else
        {
            GameObject canvas = transform.GetChild(0).gameObject;
            TextMeshProUGUI charTextUI = canvas.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
            character = charTextUI.text;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        JEC_Events.OnPickupKey.Invoke(character);

        Destroy(gameObject);
    }
}
