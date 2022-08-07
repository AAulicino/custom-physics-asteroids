using UnityEngine;

public interface IColliderModel
{
    Rect Bounds { get; }
    CollisionLayer Layer { get; set; }
    int Scale { get; set; }

    bool CollidesWith (IColliderModel other);
    void SetPosition (Vector2 position);
    void SetSize (Vector2 size);
}
