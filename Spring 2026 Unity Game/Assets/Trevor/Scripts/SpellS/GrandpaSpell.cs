using UnityEngine;

public class GrandpaSpell : SpellBehavior
{
    [SerializeField] private GameObject grandpa;
    [SerializeField] private Vector3 summonLocation;
    [SerializeField] private Quaternion summonRotation;
    public override void CastSpell(Transform playerTransform)
    {
        SummonGrandpa();
    }

    public void SummonGrandpa()
    {
        Instantiate(grandpa, summonLocation, summonRotation);
    }
}