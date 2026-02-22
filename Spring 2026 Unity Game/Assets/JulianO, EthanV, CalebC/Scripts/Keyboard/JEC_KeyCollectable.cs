using TMPro;
using UnityEngine;

public class JEC_KeyCollectable : MonoBehaviour
{
    private string character;

    private void Start()
    {
        GameObject canvas = transform.GetChild(0).gameObject;
        TextMeshProUGUI charTextUI = canvas.transform.GetChild(0).GetComponent<TextMeshProUGUI>();

        character = charTextUI.text;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Collected " + character);

        JEC_Events.OnPickupKey.Invoke(character);

        Destroy(gameObject);
    }
}
