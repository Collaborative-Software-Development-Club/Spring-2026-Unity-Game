using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class JEC_URLManager : MonoBehaviour
{
    public JEC_KeyManager KeyManager;

    private TMP_InputField inputText;
    private string CurrentText;



    private void OnEnable()
    {
        inputText = GetComponent<TMP_InputField>();

        if (inputText == null)
        {
            Debug.LogError("JEC_ERROR: Failed to retrieve text component.");
        }

        CurrentText = inputText.text;

        KeyManager.ResetKeysTyped();

        // TODO: make it so input field is automatically selected
        inputText.ActivateInputField();
    }

    public void TextFilter(string text)
    {
        if (CurrentText.Length < text.Length)
        {
            HandleKeyAdded(text);
        }
        else if (CurrentText.Length > text.Length) 
        { 
            HandleKeyRemoved(text); 
        }

        CurrentText = inputText.text;
    }

    public void HandleKeyAdded(string text)
    {
        string c = text[^1].ToString();

        JEC_Key key = KeyManager.FindKey(c);

        //Check if the user has a specific key
        if (key == null || key.amount == 0 || KeyManager.KeysUsed[c] >= key.amount)
        {
            inputText.text = text.Substring(0, text.Length - 1);

            // Invoke key press failure event
            JEC_Events.OnKeyPressFailure.Invoke(c);

        }
        else
        {
            inputText.text = text;

            KeyManager.KeysUsed[c]++;

            // Invoke key press success event
            JEC_Events.OnKeyPressSuccess.Invoke(c);

        }
    }

    public void HandleKeyRemoved(string text)
    {
        string c = CurrentText[^1].ToString();

        KeyManager.KeysUsed[c]--;

        // Invoke key removed event
        JEC_Events.OnKeyRemoved.Invoke(c);
    }

}
