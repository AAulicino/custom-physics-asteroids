using UnityEngine;

public static class Vector2Extensions
{
    public static Vector2 ClampMagnitude (this Vector2 v, float min, float max)
    {
        float sqrMagnitude = v.sqrMagnitude;

        if (sqrMagnitude > max * max)
            return v.normalized * max;
        else if (sqrMagnitude < min * min)
            return v.normalized * min;
        return v;
    }
}
