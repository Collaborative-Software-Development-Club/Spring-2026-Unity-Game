using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "SpellBookCollection", menuName = "Systems/Spell Book Collection")]
public class SpellBookCollection : ScriptableObject
{
    public List<string> unlockedBookIDs = new List<string>();

    // This event fires whenever a new book is added
    public UnityAction OnCollectionUpdated;

    public void UnlockBook(string id)
    {
        if (!unlockedBookIDs.Contains(id))
        {
            unlockedBookIDs.Add(id);
            Debug.Log($"<color=green>Data System:</color> Unlocked {id}");
            OnCollectionUpdated?.Invoke();
        }
    }
}