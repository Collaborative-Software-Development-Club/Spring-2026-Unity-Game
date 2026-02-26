using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class JEC_URLManager : MonoBehaviour
{
    public JEC_KeyManager KeyManager;

    private TMP_InputField inputText;
    private string CurrentText;
    private int CaretPosition;


    private void OnEnable()
    {
        inputText = GetComponent<TMP_InputField>();

        inputText.text = "";
        CaretPosition = 0;

        inputText.caretPosition = CaretPosition;


        if (inputText == null)
        {
            Debug.LogError("JEC_ERROR: Failed to retrieve text component.");
        }

        CurrentText = inputText.text;
        KeyManager.ResetKeysTyped();

        StartCoroutine(SelectInputField());
    }

    private void FixedUpdate()
    {
        CaretPosition = inputText.caretPosition;
    }

    IEnumerator SelectInputField()
    {

        inputText.ActivateInputField();

        yield return new WaitForEndOfFrame();

        inputText.OnPointerClick(new PointerEventData(EventSystem.current));
    }

    public void TextFilter(string text)
    {
        if (CurrentText.Length < text.Length)
        {
            HandleKeyAdded(text);
        }
        else if (CurrentText.Length > text.Length) 
        { 
            HandleKeysRemoved(text); 
        }

        CurrentText = inputText.text;
    }

    public void HandleKeyAdded(string text)
    {

        string chars = StringDifference(CurrentText, text);

        foreach (char c in chars)
        {
            JEC_Key key = KeyManager.FindKey("" + c);


            //Check if the user has a specific key
            if (key == null || key.amount == 0 || KeyManager.KeysUsed["" + c] >= key.amount)
            {
                inputText.text = CurrentText;
                inputText.caretPosition = CaretPosition;

                // Invoke key press failure event
                JEC_Events.OnKeyPressFailure.Invoke("" + c);
            }
            else
            {
                inputText.text = text;

                CaretPosition = inputText.caretPosition;
                KeyManager.KeysUsed["" + c]++;

                // Invoke key press success event
                JEC_Events.OnKeyPressSuccess.Invoke("" + c);

            }
        }

    }

    public void HandleKeysRemoved(string text)
    {

        //if keys are being removed, text is a substring of CurrentText
        string chars = StringDifference(text, CurrentText);
        
        foreach (char c in chars)
        {
            KeyManager.KeysUsed["" + c]--;

            // Invoke key removed event
            JEC_Events.OnKeyRemoved.Invoke("" + c);
        }
    }


    // returns a string with every character not in the previous string
    // (assuming s1 is a substring of s2)
    private string StringDifference(string s1, string s2)
    {
        string result = (string)s2.Clone();

        for (int i = 0; i < s1.Length; i++)
        {
            if (result.IndexOf(s1[i]) != -1)
            {
                result = result.Remove(result.IndexOf(s1[i]), 1);
            }
        }

        return result;
    }

}
