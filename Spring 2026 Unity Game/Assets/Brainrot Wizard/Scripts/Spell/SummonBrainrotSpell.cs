using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SummonBrainrotSpell : SpellBehavior
{
    public GameObject petBrainrot;
    public int maxPets = 1;
    private List<GameObject> activePets = new List<GameObject>();

    SummonBrainrotSpell() {
    }

    public override void CastSpell(Transform location) {
        if (activePets.Count >= maxPets) {
            Destroy(activePets[0]);
            activePets.RemoveAt(0);
        }
        
        GameObject newPet = Instantiate(petBrainrot, location.position + Vector3.left * 1.5f, Quaternion.Euler(90f, 0f, 0f));
        activePets.Add(newPet);
    }
}