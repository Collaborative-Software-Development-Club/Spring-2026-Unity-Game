using UnityEngine;

public class GridUtility
{
    public static Vector2Int WorldSpaceToGrid(Vector2 worldPos, int gridWidth, int gridHeight)
    {
        Vector2 viewportPos = Camera.main.WorldToViewportPoint(worldPos);
        int gridX = Mathf.RoundToInt(viewportPos.x * gridWidth);
        int gridY = Mathf.RoundToInt(viewportPos.y * gridHeight);
        return new Vector2Int(gridX, gridY);
    }

    //public static Vector2 GridToWorldSpace(int gridWidth, int gridHeight)
}
