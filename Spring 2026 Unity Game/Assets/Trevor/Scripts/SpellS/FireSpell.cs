using UnityEngine;

public class FireSpell : SpellBehavior
{
    public override void CastSpell(Transform playerTransform)
    {
        Debug.Log("Casting Fireball!");
        // Logic to spawn a fireball projectile, play sounds, etc.
    }
}