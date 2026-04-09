using UnityEngine;

public class LetterSpell : SpellBehavior
{
    public override void CastSpell(Transform playerTransform)
    {
        Debug.Log("A");
        // Logic to spawn a fireball projectile, play sounds, etc.
    }
}