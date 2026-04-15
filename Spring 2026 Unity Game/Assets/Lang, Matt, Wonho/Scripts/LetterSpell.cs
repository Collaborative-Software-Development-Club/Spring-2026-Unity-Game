using UnityEngine;

using TMPro;
using UnityEngine.UI;

public class LetterSpell : SpellBehavior
{
    private string letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

    public TextMeshPro textDisplay; // Use TextMeshPro for 3D

    void Awake()
    {
        // Force the script to find the text component on THIS object
        //textDisplay = GetComponent<TextMeshProUGUI>();

        // This will tell you EXACTLY if it failed to find it
        if (textDisplay == null)
        {
            Debug.LogError($"TMP Component missing on {gameObject.name}!", this);
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
        ChangeMyText(letter);
        // Logic to spawn a fireball projectile, play sounds, etc.
    }
    char RandomLetter()
    {
        int letter = Random.Range(0, 26);
        return letters[letter];
    }
}