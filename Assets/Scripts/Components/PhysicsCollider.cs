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

    public Vector2 ClosestPointOnBoundsToPoint (Vector2 point)
    {
        Vector2 max = Bounds.max;
        Vector2 min = Bounds.min;

        float minDist = Mathf.Abs(point.x - min.x);

        Vector2 boundsPoint = new Vector2(min.x, point.y);

        if (Mathf.Abs(max.x - point.x) < minDist)
        {
            minDist = Mathf.Abs(max.x - point.x);
            boundsPoint = new Vector2(max.x, point.y);
        }

        if (Mathf.Abs(max.y - point.y) < minDist)
        {
            minDist = Mathf.Abs(max.y - point.y);
            boundsPoint = new Vector2(point.x, max.y);
        }

        if (Mathf.Abs(min.y - point.y) < minDist)
            boundsPoint = new Vector2(point.x, min.y);

        return boundsPoint;
    }
}
