using UnityEngine;

public class PhysicsCollider : ICollider
{
    public int Id { get; }
    public Rect Bounds { get; private set; }
    public int Layer { get; }

    Rect originalBounds;

    public PhysicsCollider (int id, Rect bounds, int layer)
    {
        Id = id;
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
}
