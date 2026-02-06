using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewSceneBucket", menuName = "Systems/Scene Bucket")]
public class SceneBucket : ScriptableObject
{
    [Tooltip("Add the exact names of your gameplay scenes here.")]
    public List<string> availableScenes;

    public string GetRandomScene()
    {
        if (availableScenes == null || availableScenes.Count == 0) return "";
        int index = Random.Range(0, availableScenes.Count);
        return availableScenes[index];
    }
}