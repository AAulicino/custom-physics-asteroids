using UnityEngine;

public class SquareColliderModel : ColliderModel
{
    public float OuterRadius { get; set; }
    public float InnerRadius { get; set; }

    public SquareColliderModel (CollisionLayer layer) : base(layer)
    {
    }

    public override void SetSize (Vector2 size)
    {
        base.SetSize(size);
        OuterRadius = Mathf.Max(Bounds.width, Bounds.height) / 2f;
        InnerRadius = Mathf.Min(Bounds.width, Bounds.height) / 2f;
    }
}
