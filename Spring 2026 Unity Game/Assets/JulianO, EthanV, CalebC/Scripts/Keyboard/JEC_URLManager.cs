using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class JEC_URLManager : MonoBehaviour
{
    public JEC_KeyManager KeyManager;
    public string StartingURL;

    private TMP_InputField inputText;
    private string CurrentText;
    private int CaretPosition;

    private void Start()
    {
        inputText = GetComponent<TMP_InputField>();
        inputText.text = StartingURL;

        CurrentText = inputText.text;

        CaretPosition = inputText.text.Length;
        inputText.caretPosition = CaretPosition;


        JEC_Events.OnInteractKeyboardPedestal.AddListener(StartURLTyping);
        JEC_Events.OnExitKeyboardPedestal.AddListener(ExitURLTyping);
    }

    private void OnEnable()
    {
        inputText = GetComponent<TMP_InputField>();

        CaretPosition = inputText.text.Length;

        inputText.caretPosition = CaretPosition;


        if (inputText == null)
        {
            Debug.LogError("JEC_ERROR: Failed to retrieve text component.");
        }

        KeyManager.ResetKeysTyped();
    }

    private void Update()
    {
        CaretPosition = inputText.caretPosition;

        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (JEC_PageManager.Instance != null && JEC_PageManager.Instance.IsValidURL(inputText.text))
            {
                JEC_Events.OnEnterURL.Invoke(inputText.text);
                JEC_Events.OnExitKeyboardPedestal.Invoke();
            }
            else
            {
                inputText.text = "";
                inputText.placeholder.GetComponent<TMP_Text>().text = "Invalid URL!";
                JEC_Events.OnExitKeyboardPedestal.Invoke();
            }
        }
    }

    private void StartURLTyping()
    {
        inputText.readOnly = false;

        inputText.placeholder.GetComponent<TMP_Text>().text = "Type a URL";
        CurrentText = inputText.text;

        StartCoroutine(SelectInputField());
    }

    IEnumerator SelectInputField()
    {

        inputText.ActivateInputField();

        yield return new WaitForEndOfFrame();

        inputText.OnPointerClick(new PointerEventData(EventSystem.current));
    }


    public void TextFilter(string text)
    {
        inputText = GetComponent<TMP_InputField>();

        if (CurrentText != null) { 
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
            if (KeyManager.KeysUsed["" + c] != 0) {
                KeyManager.KeysUsed["" + c]--;
            }
            else
            {
                KeyManager.IncrementKeyVal("" + c);
            }

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

   
    public void ExitURLTyping()
    {
        inputText.readOnly = true;
    }

}
