using UnityEngine;

public interface ICollider
{
    int Id { get; }
    Rect Bounds { get; }
    int Layer { get; }

    bool CollidesWith (ICollider other);
    void SetPosition (Vector2 position);
    Vector2 ClosestPointOnBoundsToPoint (Vector2 point);
}
