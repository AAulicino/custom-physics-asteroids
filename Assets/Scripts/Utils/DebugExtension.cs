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

    public static void DebugQuadTreeRect (QuadTreeRect rect, Color color, float duration = 0, bool depthTest = true)
    {
        Vector2 ru = new Vector2(rect.Right, rect.Top);
        Vector2 lu = new Vector2(rect.Left, rect.Top);

        Vector2 rd = new Vector2(rect.Right, rect.Bottom);
        Vector2 ld = new Vector2(rect.Left, rect.Bottom);

        Debug.DrawLine(ru, lu, color, duration, depthTest);
        Debug.DrawLine(ru, rd, color, duration, depthTest);
        Debug.DrawLine(lu, ld, color, duration, depthTest);
        Debug.DrawLine(rd, ld, color, duration, depthTest);
    }
}
