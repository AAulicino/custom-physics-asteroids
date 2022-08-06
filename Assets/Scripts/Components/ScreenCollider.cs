using UnityEngine;

public class ScreenCollider : ICollider
{
    public int Id => -1;
    public int Layer => -1;

    public Rect Bounds { get; private set; }

    public ScreenCollider (Rect bounds)
    {
        Bounds = bounds;
    }

    public void SetPosition (Vector2 position)
    {
    }

    public bool CollidesWith (ICollider other)
    {
        return (other.Layer & Layer) != 0 && Bounds.Overlaps(other.Bounds, true);
    }
}
