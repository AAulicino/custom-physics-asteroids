using System;
using UnityEngine;

public class PhysicsRigidBody : IRigidBody
{
    public event Action OnOutOfBounds;

    public ICollider Collider { get; }

    public Vector2 Position { get; set; }
    public Vector2 Velocity { get; set; }
    public float AngularVelocity { get; set; }
    public float Rotation { get; set; }

    readonly IStageBounds stageInfo;
    readonly bool wrapOnScreenEdge;

    public PhysicsRigidBody (IStageBounds stageInfo, ICollider collider, bool wrapOnScreenEdge)
    {
        this.stageInfo = stageInfo;
        Collider = collider;
        this.wrapOnScreenEdge = wrapOnScreenEdge;
    }

    public void Step (float deltaTime)
    {
        Position += Velocity * deltaTime;
        Rotation += AngularVelocity * deltaTime;

        if (wrapOnScreenEdge)
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
