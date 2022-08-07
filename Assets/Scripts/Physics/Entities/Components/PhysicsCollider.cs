using UnityEngine;

public class PhysicsCollider : ICollider
{
    public Rect Bounds { get; private set; }
    public int Layer { get; set; }

    Rect originalBounds;

    public PhysicsCollider (Rect bounds, int layer)
    {
        Layer = layer;
        originalBounds = bounds;
    }

    public void SetPosition (Vector2 position)
    {
        Bounds = new Rect(
            position - originalBounds.center,
            originalBounds.size
        );
    }

    public bool CollidesWith (ICollider other)
    {
        return (other.Layer & Layer) != 0 && Bounds.Overlaps(other.Bounds);
    }

    public void SetSize (Vector2 size)
    {
        originalBounds.size = size;
        SetPosition(Bounds.center);
    }
}
