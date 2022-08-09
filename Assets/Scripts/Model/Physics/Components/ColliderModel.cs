using UnityEngine;

public abstract class ColliderModel : IColliderModel
{
    public Rect Bounds { get; private set; }
    public CollisionLayer Layer { get; set; }

    public virtual Vector2 Position
    {
        get => Bounds.center;
        set
        {
            Rect bounds = Bounds;
            bounds.center = value;
            Bounds = bounds;
        }
    }

    public int Scale
    {
        get => scale;
        set
        {
            scale = value;
            ApplyBoundsScale();
        }
    }

    int scale = 1;
    Rect unscaledBounds;

    public ColliderModel (CollisionLayer layer)
    {
        Layer = layer;
    }

    public virtual void SetSize (Vector2 size)
    {
        unscaledBounds = Bounds;
        unscaledBounds.size = size;

        ApplyBoundsScale();
    }

    void ApplyBoundsScale ()
    {
        Rect bounds = Bounds;
        bounds.size = unscaledBounds.size * scale;
        Bounds = bounds;
    }
}
