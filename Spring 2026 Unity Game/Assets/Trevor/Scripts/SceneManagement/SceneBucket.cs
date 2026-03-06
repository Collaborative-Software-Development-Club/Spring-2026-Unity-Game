// SceneBucket.cs
using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "SceneBucket", menuName = "Systems/Scene Bucket")]
public class SceneBucket : ScriptableObject
{
    [System.Serializable]
    public struct LevelData
    {
        public LevelID id;
        public string sceneName; // The actual string needed for SceneManager.LoadScene
    }

    [Tooltip("The master list of all possible levels. NEVER delete items from here at runtime.")]
    public List<LevelData> allLevels;

    [Tooltip("Reference to the player's progress so we know what to filter out.")]
    public PlayerProgress progressData;

    public string GetRandomAvailableScene()
    {
        // 1. Find all levels that have NOT been unlocked yet
        List<LevelData> availableLevels = allLevels.FindAll(level => !progressData.IsUnlocked(level.id));

        // 2. If everything is unlocked, return empty or handle game beat logic
        if (availableLevels.Count == 0)
        {
            Debug.LogWarning("No more available scenes! Player beat the game.");
            return "";
        }

        // 3. Pick a random scene from the remaining available pool
        int index = Random.Range(0, availableLevels.Count);
        return availableLevels[index].sceneName;
    }
}