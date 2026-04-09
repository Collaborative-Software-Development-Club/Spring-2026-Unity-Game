using UnityEngine;

public class EssenceSpell : SpellBehavior
{
    public override void CastSpell(Transform playerTransform)
    {
        Debug.Log("Casting Essence Explosion!");
        // Logic to spawn an explosion of essence, play sounds, etc.
    }
}