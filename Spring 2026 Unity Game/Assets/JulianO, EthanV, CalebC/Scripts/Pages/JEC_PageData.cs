using UnityEngine;

[CreateAssetMenu(fileName = "JEC_PageData", menuName = "Scriptable Objects/JEC_PageData")]
public class JEC_PageData : ScriptableObject
{
    public string url;
    public string pageName;
    public Sprite background;
    public GameObject contentPrefab;
}
