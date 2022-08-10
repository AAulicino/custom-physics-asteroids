using UnityEngine;

public static class DebugExtension
{
    public static void DrawRect (Rect rect, Color color)
    {
        Vector2 ru = new Vector2(rect.xMax, rect.yMax);
        Vector2 lu = new Vector2(rect.xMin, rect.yMax);

        Vector2 rd = new Vector2(rect.xMax, rect.yMin);
        Vector2 ld = new Vector2(rect.xMin, rect.yMin);

        Color oldColor = Gizmos.color;
        Gizmos.color = color;

        Gizmos.DrawLine(ru, lu);
        Gizmos.DrawLine(ru, rd);
        Gizmos.DrawLine(lu, ld);
        Gizmos.DrawLine(rd, ld);

        Gizmos.color = oldColor;
    }

    public static void DebugRect (Rect rect, Color color, float duration = 0, bool depthTest = true)
    {
        Vector2 ru = new Vector2(rect.xMax, rect.yMax);
        Vector2 lu = new Vector2(rect.xMin, rect.yMax);

        Vector2 rd = new Vector2(rect.xMax, rect.yMin);
        Vector2 ld = new Vector2(rect.xMin, rect.yMin);

        Debug.DrawLine(ru, lu, color, duration, depthTest);
        Debug.DrawLine(ru, rd, color, duration, depthTest);
        Debug.DrawLine(lu, ld, color, duration, depthTest);
        Debug.DrawLine(rd, ld, color, duration, depthTest);
    }

    public static void DebugQuadTreeRect (
        QuadTreeRect rect,
        Color color,
        float duration = 0,
        bool depthTest = true
    )
    {
        DebugRect(
            new Rect(rect.X, rect.Y, rect.Width, rect.Height),
            color,
            duration,
            depthTest
        );
    }

    public static void DrawCircle (Vector3 position, Vector3 up, Color color, float radius = 1.0f)
    {
        up = ((up == Vector3.zero) ? Vector3.up : up).normalized * radius;
        Vector3 _forward = Vector3.Slerp(up, -up, 0.5f);
        Vector3 _right = Vector3.Cross(up, _forward).normalized * radius;

        Matrix4x4 matrix = new Matrix4x4();

        matrix[0] = _right.x;
        matrix[1] = _right.y;
        matrix[2] = _right.z;

        matrix[4] = up.x;
        matrix[5] = up.y;
        matrix[6] = up.z;

        matrix[8] = _forward.x;
        matrix[9] = _forward.y;
        matrix[10] = _forward.z;

        Vector3 _lastPoint = position + matrix.MultiplyPoint3x4(
            new Vector3(Mathf.Cos(0), 0, Mathf.Sin(0))
        );
        Vector3 _nextPoint = Vector3.zero;

        Color oldColor = Gizmos.color;
        Gizmos.color = (color == default) ? Color.white : color;

        for (var i = 0; i < 91; i++)
        {
            _nextPoint.x = Mathf.Cos((i * 4) * Mathf.Deg2Rad);
            _nextPoint.z = Mathf.Sin((i * 4) * Mathf.Deg2Rad);
            _nextPoint.y = 0;

            _nextPoint = position + matrix.MultiplyPoint3x4(_nextPoint);

            Gizmos.DrawLine(_lastPoint, _nextPoint);
            _lastPoint = _nextPoint;
        }

        Gizmos.color = oldColor;
    }
}
