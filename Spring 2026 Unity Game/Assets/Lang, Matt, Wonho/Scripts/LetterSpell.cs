using UnityEngine;

using TMPro;
using UnityEngine.UI;
using System.Collections;

public class LetterSpell : SpellBehavior
{
    private string letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    private Coroutine currentRoutine;

    public TMP_Text textDisplay; // Use TextMeshPro for 3D
    public GameObject box;
    void Awake()
    {
        box = GameObject.FindWithTag("IncantationSpell");

        if (box == null)
        {
            Debug.LogError("Tag not found!");
            return;
        }

        textDisplay = box.GetComponent<TMP_Text>();

        if (textDisplay == null)
        {
            Debug.LogError("TextMeshPro component not found!");
        }
        
    }

    public void ChangeMyText(char letter)
    {
        //textDisplay.enabled = true;
        if (textDisplay != null)
        {
            textDisplay.text = "" + letter;
        }
        else
        {
            Debug.Log("Darn");
        }
    }
    public override void CastSpell(Transform playerTransform)
    {
        char letter = RandomLetter();
        Debug.Log(letter);
        ShowText(""+letter);
        // Logic to spawn a fireball projectile, play sounds, etc.
    }
    public void ShowText(string message, float duration = 2f)
    {
        Color c = textDisplay.color;
        c.a = 1;
        textDisplay.color = c;
        if (currentRoutine != null)
        {
            StopCoroutine(currentRoutine);
        }

        currentRoutine = StartCoroutine(ShowTextRoutine(message, duration)); 
    }

    private IEnumerator ShowTextRoutine(string message, float duration)
    {
        textDisplay.text = message;

        yield return new WaitForSeconds(duration - 3);
        for (int i = 0; i < 24; i++)
        {
            Color c = textDisplay.color;
            c.a = c.a - 1f/24f;
            textDisplay.color = c;
            yield return new WaitForSeconds(0.125f);
        }
        

        textDisplay.text = "";
    }
    char RandomLetter()
    {
        int letter = Random.Range(0, 26);
        return letters[letter];
    }
}