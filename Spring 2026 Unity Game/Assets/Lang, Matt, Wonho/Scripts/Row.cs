using UnityEngine;

public class Row : MonoBehaviour
{
    public Tile[] tiles {  get; private set; }

    private void Awake()
    {
        tiles = GetComponentsInChildren<Tile>();
    }
}