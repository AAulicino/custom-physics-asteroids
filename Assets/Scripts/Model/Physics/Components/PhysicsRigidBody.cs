using System;
using UnityEngine;

public class PhysicsRigidBody : IRigidBody
{
    public event Action OnOutOfBounds;

    public ICollider Collider { get; }

    public Vector2 Position { get; set; }
    public Vector2 Velocity { get; set; }
    public Vector2 Acceleration { get; set; }

    public float AngularVelocity { get; set; }
    public float Rotation { get; set; }

    public float MaxSpeed => settings.MaxSpeed;
    public float Drag => settings.Drag;
    public bool WrapOnScreenEdge => settings.WrapOnScreenEdge;

    protected readonly IEntitySettings settings;
    protected readonly IStageBounds stageInfo;

    public PhysicsRigidBody (
        IEntitySettings settings,
        IStageBounds stageInfo,
        ICollider collider
    )
    {
        this.settings = settings;
        this.stageInfo = stageInfo;
        Collider = collider;
    }

    public virtual void Step (float deltaTime)
    {
        Velocity = Vector2.ClampMagnitude(Velocity + Acceleration, MaxSpeed);
        Position += Velocity * deltaTime;
        Rotation += AngularVelocity * deltaTime;

        Velocity *= Drag;

        if (WrapOnScreenEdge)
        {
            if (stageInfo.Rect.xMax < Position.x)
                Position = new Vector2(stageInfo.Rect.xMin, Position.y);
            else if (stageInfo.Rect.xMin > Position.x)
                Position = new Vector2(stageInfo.Rect.xMax, Position.y);
            else if (stageInfo.Rect.yMax < Position.y)
                Position = new Vector2(Position.x, stageInfo.Rect.yMin);
            else if (stageInfo.Rect.yMin > Position.y)
                Position = new Vector2(Position.x, stageInfo.Rect.yMax);
        }
        else if (!stageInfo.Rect.Contains(Position))
        {
            OnOutOfBounds?.Invoke();
        }
    }

    public void SyncComponents ()
    {
        Collider.SetPosition(Position);
    }
}
