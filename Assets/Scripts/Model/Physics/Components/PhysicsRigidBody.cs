using UnityEngine;

public class PhysicsRigidBody : IRigidBody
{
    public ICollider Collider { get; }

    public Vector2 Position { get; set; }
    public Vector2 Velocity { get; set; }
    public float AngularVelocity { get; set; }
    public float Rotation { get; set; }

    readonly IStageBounds stageInfo;

    public PhysicsRigidBody (IStageBounds stageInfo, ICollider collider)
    {
        this.stageInfo = stageInfo;
        Collider = collider;
    }

    public void Step (float deltaTime)
    {
        Position += Velocity * deltaTime;
        Rotation += AngularVelocity * deltaTime;

        if (stageInfo.Rect.xMax < Position.x)
            Position = new Vector2(stageInfo.Rect.xMin, Position.y);
        else if (stageInfo.Rect.xMin > Position.x)
            Position = new Vector2(stageInfo.Rect.xMax, Position.y);
        else if (stageInfo.Rect.yMax < Position.y)
            Position = new Vector2(Position.x, stageInfo.Rect.yMin);
        else if (stageInfo.Rect.yMin > Position.y)
            Position = new Vector2(Position.x, stageInfo.Rect.yMax);
    }

    public void SyncComponents ()
    {
        Collider.SetPosition(Position);
    }
}
