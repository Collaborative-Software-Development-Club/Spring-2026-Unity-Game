using UnityEngine;

public class GrandpaSpell : SpellBehavior
{
    [SerializeField] private GameObject grandpa;
    [SerializeField] private Vector3 summonLocation;
    [SerializeField] private Quaternion summonRotation;
    public override void CastSpell(Transform playerTransform)
    {
        SummonGrandpa(playerTransform);
    }

    public void SummonGrandpa(Transform location)
    {
        Instantiate(grandpa, location.position, summonRotation);
    }
}