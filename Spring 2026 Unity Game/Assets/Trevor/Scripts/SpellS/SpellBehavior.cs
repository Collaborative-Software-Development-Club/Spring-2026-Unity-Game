using UnityEngine;

public abstract class SpellBehavior : MonoBehaviour
{
    // The player script will call this when 'Q' is pressed.
    // We pass in the player's transform so the spell knows where to spawn/shoot from.
    public abstract void CastSpell(Transform playerTransform);
}