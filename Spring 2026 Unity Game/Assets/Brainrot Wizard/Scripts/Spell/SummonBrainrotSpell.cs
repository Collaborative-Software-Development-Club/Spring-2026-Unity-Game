using Unity.VisualScripting;
using UnityEngine;

public class SummonBrainrotSpell : SpellBehavior
{
    public GameObject petBrainrot;

    SummonBrainrotSpell() {
    }

    public override void CastSpell(Transform location) {
        Instantiate(petBrainrot, location.position, Quaternion.identity);
    }
}