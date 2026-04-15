using UnityEngine;

public class LetterSpell : SpellBehavior
{
    private string letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

    public override void CastSpell(Transform playerTransform)
    {
        char letter = RandomLetter();
        Debug.Log(letter);
        // Logic to spawn a fireball projectile, play sounds, etc.
    }
    char RandomLetter()
    {
        int letter = Random.Range(0, 26);
        return letters[letter];
    }
}