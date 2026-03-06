// PlayerProgress.cs
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "PlayerProgress", menuName = "Systems/Player Progress")]
public class PlayerProgress : ScriptableObject
{
    [Tooltip("The list of books/levels the player has completed.")]
    public List<LevelID> unlockedBooks = new List<LevelID>();

    public UnityAction OnCollectionUpdated;

    public void UnlockBook(LevelID id)
    {
        if (id != LevelID.None && !unlockedBooks.Contains(id))
        {
            unlockedBooks.Add(id);
            Debug.Log($"<color=green>Progress:</color> Unlocked Book for {id}");
            OnCollectionUpdated?.Invoke();
        }
    }

    public bool IsUnlocked(LevelID id)
    {
        return unlockedBooks.Contains(id);
    }

    // Call this to safely reset testing data without destroying your master list
    public void ResetProgress()
    {
        unlockedBooks.Clear();
        Debug.Log("<color=red>Progress:</color> All data cleared for testing.");
        OnCollectionUpdated?.Invoke();
    }
}