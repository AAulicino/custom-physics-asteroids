using UnityEngine;

public class CircleColliderModel : ColliderModel, ICircleColliderModel
{
    public float Radius { get; private set; }

    public float SqrRadius { get; private set; }

    public override Vector2 Position
    {
        get => base.Position;
        set => base.Position = value;
    }

    public CircleColliderModel (CollisionLayer layer) : base(layer)
    {
    }

    public override void SetSize (Vector2 size)
    {
        base.SetSize(size);
        Radius = Mathf.Min(Bounds.width, Bounds.height) / 2;
        SqrRadius = Radius * Radius;
    }
}
