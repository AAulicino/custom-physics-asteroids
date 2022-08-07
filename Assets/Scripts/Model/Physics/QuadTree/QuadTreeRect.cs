using UnityEngine;

public struct QuadTreeRect
{
    public readonly float X;
    public readonly float Y;
    public readonly float Width;
    public readonly float Height;

    public float Top => Y + Height;
    public float Bottom => Y;
    public float Left => X;
    public float Right => X + Width;

    public float MidX => X + HalfWidth;
    public float MidY => Y + HalfHeight;

    public Vector2 Center => new(MidX, MidY);

    public QuadTreeRect TopRightQuarter => new(MidX, MidY, HalfWidth, HalfHeight);
    public QuadTreeRect BottomRightQuarter => new(MidX, Y, HalfWidth, HalfHeight);
    public QuadTreeRect TopLeftQuarter => new(X, MidY, HalfWidth, HalfHeight);
    public QuadTreeRect BottomLeftQuarter => new(X, Y, HalfWidth, HalfHeight);

    float HalfWidth => Width * 0.5f;
    float HalfHeight => Height * 0.5f;

    public QuadTreeRect (float x, float y, float width, float height)
    {
        X = x;
        Y = y;

        Width = width;
        Height = height;
    }

    public bool Contains (Vector2 pos)
    {
        return pos.x >= X && pos.x <= X + Width
            && pos.y >= Y && pos.y <= Y + Height;
    }

    public Vector2 Clamp (Vector2 pos)
    {
        return new Vector2(
            Mathf.Clamp(pos.x, Left, Right),
            Mathf.Clamp(pos.y, Bottom, Top)
        );
    }
}
