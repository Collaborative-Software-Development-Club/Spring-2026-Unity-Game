using UnityEngine;

public class CoordConverter
{
    public static Vector3 ConvertXYToXZ(Vector2 vector)
    {
        return new Vector3(vector.x, 0, vector.y);
    }
}
