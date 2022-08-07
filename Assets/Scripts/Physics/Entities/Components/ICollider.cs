using UnityEngine;

public interface ICollider
{
    Rect Bounds { get; }
    int Layer { get; set; }

    bool CollidesWith (ICollider other);
    void SetPosition (Vector2 position);
    void SetSize (Vector2 size);
}
