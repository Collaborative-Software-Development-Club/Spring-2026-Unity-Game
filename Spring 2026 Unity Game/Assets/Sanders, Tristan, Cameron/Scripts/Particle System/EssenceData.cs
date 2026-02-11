using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public struct EssenceData
{

    public Vector2Int velocity;
    public EssenceType type ;

    public static readonly EssenceData Empty = new EssenceData {velocity = Vector2Int.zero, type = EssenceType.none};


    
}
