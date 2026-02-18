using UnityEngine;

//If there is an issue with this class it may be because canvas is xy space and map is xz space.
public class GridUtility
{
    public static Vector2Int WorldSpaceToGrid(Vector2 worldPos, int gridWidth, int gridHeight)
    {
        Vector2 viewportPos = Camera.main.WorldToViewportPoint(worldPos);
        int gridX = Mathf.RoundToInt(viewportPos.x * gridWidth);
        int gridY = Mathf.RoundToInt(viewportPos.y * gridHeight);
        return new Vector2Int(gridX, gridY);
    }


    public static Vector2 GridToWorldSpace(int x, int y, int gridWidth, int gridHeight)
    {
        Vector2 viewportPos = new Vector3((float)x / gridWidth, (float)y / gridHeight);
        return Camera.main.ViewportToWorldPoint(viewportPos);
    }

    public static bool IsInBounds(Vector2Int position, int gridWidth, int gridHeight)
    {
        return position.x >= 0 && position.y >= 0 && position.x < gridWidth && position.y < gridHeight;
    }
}
