using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.InputSystem;

public class JEC_KeyDisplay : MonoBehaviour
{
    public JEC_KeyManager KeyManager;

    void Start()
    {
        
    }

    void Update()
    {
        // Whenever we press F, KeyManager updates our scriptableObjects and KeyDisplay updates the UI 
        if(Input.GetKeyDown("f"))
        {
            KeyManager.IncrementKeyVal("a");
            Debug.Log("Added a to Keys");

            UpdateDisplayedKey("a");
        }
    }

    public void UpdateDisplayedKey(string character)
    {
        JEC_Key key = KeyManager.FindKey(character);
        GameObject keyUI = GetUIKey(character);

        if (key == null || keyUI == null)
        {
            Debug.LogError("JEC_ERROR: Failed to find key or keyUI of character: " + character);
        }
        else
        {
            GameObject amount = JEC_Helper.FindGameObjectInChildWithTag(keyUI, "JEC_Amount");
            TextMeshProUGUI amountText = amount.GetComponent<TextMeshProUGUI>();

            amountText.text = key.amount.ToString();
        }
    }

    public GameObject GetUIKey(string character)
    {
        GameObject keyUI = GameObject.Find("Keyboard/" +  character);

        if (keyUI == null)
        {
            Debug.LogError("JEC_ERROR: Failed to find keyUI of character: " + character);
        }

        return keyUI;
    }


}
