using NUnit.Framework;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GrandpaSpell : SpellBehavior
{
    [SerializeField] private GameObject grandpa;
    [SerializeField] private Vector3 summonLocation;
    [SerializeField] private Quaternion summonRotation;
    [SerializeField] private int grandpaLimit = 8;

    private int grandpaCount = 0;
    private int destroyedCount = 0;
    private List<GameObject> Grandpas = new List<GameObject>();
    public override void CastSpell(Transform playerTransform)
    {
        SummonGrandpa(playerTransform);
    }

    public void SummonGrandpa(Transform location)
    {
        if (grandpaCount >= grandpaLimit)
        {
            Destroy(Grandpas[destroyedCount]);
            destroyedCount++;
            grandpaCount--;
        }

        Grandpas.Add(Instantiate(grandpa, location.position, summonRotation));
        grandpaCount++;
    }
}