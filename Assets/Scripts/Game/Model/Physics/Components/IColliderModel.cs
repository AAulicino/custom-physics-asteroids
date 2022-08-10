using UnityEngine;

public interface IColliderModel
{
    CollisionLayer Layer { get; set; }
    Vector2 Position { get; set; }
    int Scale { get; set; }
    Rect Bounds { get; }

    void SetSize (Vector2 size);
}
