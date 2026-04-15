using UnityEngine;

public class DampSpell : SpellBehavior
{
    public GameObject summonPrefab;

    public override void CastSpell(Transform playerTransform)
    {
        Instantiate(summonPrefab);
        Debug.Log("Something damp was summoned");
    }
       
}
