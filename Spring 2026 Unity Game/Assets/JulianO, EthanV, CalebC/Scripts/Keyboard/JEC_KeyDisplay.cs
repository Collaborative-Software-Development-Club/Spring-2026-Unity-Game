using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class JEC_KeyDisplay : MonoBehaviour
{
    public JEC_KeyManager KeyManager;
    public JEC_URLManager URLManager;

    void Start()
    {
        JEC_Events.OnKeyPressSuccess.AddListener(UpdateDisplayedKey);
        JEC_Events.OnKeyRemoved.AddListener(UpdateDisplayedKey);
        JEC_Events.OnInteractKeyboardPedestal.AddListener(UpdateAllDisplayedKeys);
    }

    private void OnEnable()
    {
        UpdateAllDisplayedKeys();
        
    }


    public void UpdateDisplayedKey(string c)
    {

        JEC_Key key = KeyManager.FindKey(c);

        c = key.character.Equals("forwardSlash") ? "/" : key.character;

        GameObject keyUI = GetUIKey(c);

        if (key == null || keyUI == null)
        {
            Debug.LogError("JEC_ERROR: Failed to find key or keyUI of character: " + c);
        }
        else
        {
            GameObject amount = JEC_Helper.FindGameObjectInChildWithTag(keyUI, "JEC_Amount");
            TextMeshProUGUI amountText = amount.GetComponent<TextMeshProUGUI>();
            Button keyIcon = keyUI.GetComponent<Button>();

            if (KeyManager.KeysUsed[c] == key.amount)
            {
                keyIcon.interactable = false;
            }
            else
            {
                keyIcon.interactable = true;
            }

            amountText.text = (key.amount - KeyManager.KeysUsed[c]).ToString();
            Debug.Log("Number of " + c + " keys used: " + KeyManager.KeysUsed[c].ToString());
        }
    }

    public void UpdateAllDisplayedKeys()
    {

        foreach (JEC_Key key in KeyManager.Keyboard)
        {
            string c = key.character.Equals("forwardSlash") ? "/" : key.character; 

            GameObject keyUI = GetUIKey(c);

            GameObject amount = JEC_Helper.FindGameObjectInChildWithTag(keyUI, "JEC_Amount");
            TextMeshProUGUI amountText = amount.GetComponent<TextMeshProUGUI>();
            Button keyIcon = keyUI.GetComponent<Button>();

            if (key.amount == 0 || KeyManager.KeysUsed[c] == key.amount)
            {
                keyIcon.interactable = false;
            }
            else
            {
                keyIcon.interactable = true;
            }

            amountText.text = (key.amount - KeyManager.KeysUsed[c]).ToString();
        }

        Debug.Log("Updated Key UI");
    }

    public GameObject GetUIKey(string c)
    {
        if (c.Equals("/"))
        {
            c = "forwardSlash";
        }

        GameObject keyUI = GameObject.Find("Keyboard/" +  c);

        if (keyUI == null)
        {
            Debug.LogError("JEC_ERROR: Failed to find keyUI of character: " + c);
        }

        return keyUI;
    }


}
