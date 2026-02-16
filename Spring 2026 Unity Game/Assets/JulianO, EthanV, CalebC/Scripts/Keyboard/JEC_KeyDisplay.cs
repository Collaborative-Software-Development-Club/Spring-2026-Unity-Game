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
        JEC_Events.OnKeyPressSuccess.AddListener(UpdateDisplayedKey);
        JEC_Events.OnKeyRemoved.AddListener(UpdateDisplayedKey);
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

        // Whenever we press F, KeyManager updates our scriptableObjects and KeyDisplay updates the UI 
        if (Input.GetKeyDown("m"))
        {
            KeyManager.IncrementKeyVal("b");
            Debug.Log("Added b to Keys");

            UpdateDisplayedKey("b");
        }

    }

    public void UpdateDisplayedKey(string c)
    {
        JEC_Key key = KeyManager.FindKey(c);
        GameObject keyUI = GetUIKey(c);

        if (key == null || keyUI == null)
        {
            Debug.LogError("JEC_ERROR: Failed to find key or keyUI of character: " + c);
        }
        else
        {
            GameObject amount = JEC_Helper.FindGameObjectInChildWithTag(keyUI, "JEC_Amount");
            TextMeshProUGUI amountText = amount.GetComponent<TextMeshProUGUI>();

            amountText.text = (key.amount - KeyManager.KeysUsed[key.character]).ToString();
            Debug.Log("Number of " + c + " keys used: " + KeyManager.KeysUsed[key.character].ToString());
        }
    }

    public GameObject GetUIKey(string c)
    {
        GameObject keyUI = GameObject.Find("Keyboard/" +  c);

        if (keyUI == null)
        {
            Debug.LogError("JEC_ERROR: Failed to find keyUI of character: " + c);
        }

        return keyUI;
    }


}
