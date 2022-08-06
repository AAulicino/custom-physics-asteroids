using UnityEngine;

public class PhysicsRigidBody : IRigidBody
{
    const float epsilon = 0.01f;

    public ICollider Collider { get; }

    public Vector2 Position { get; set; }
    public Vector2 Velocity { get; set; }
    public float AngularVelocity { get; set; }
    public float Rotation { get; set; }

    public PhysicsRigidBody (ICollider collider)
    {
        Collider = collider;
    }

    public void Step (float deltaTime)
    {
        Position += Velocity * deltaTime;
        Rotation += AngularVelocity * deltaTime;
    }

    public void SyncComponents ()
    {
        Collider.SetPosition(Position);
    }

    public void ResolveCollision (Collision collision)
    {
        if (Velocity.sqrMagnitude == 0)
            return;

        Vector2 newVelocity = Velocity;
        Vector2 newPosition = Position;

        Rect selfBounds = Collider.Bounds;
        Rect otherBounds = collision.Other.Bounds;

        float leftDist = Mathf.Abs(selfBounds.xMax - otherBounds.xMin);
        float rightDist = Mathf.Abs(selfBounds.xMin - otherBounds.xMax);
        float bottomDist = Mathf.Abs(selfBounds.yMax - otherBounds.yMin);
        float topDist = Mathf.Abs(selfBounds.yMin - otherBounds.yMax);

        float minDist = Mathf.Min(leftDist, rightDist, Mathf.Min(bottomDist, topDist));

        if (minDist == leftDist)
        {
            newVelocity.x = -Velocity.x;
            newPosition.x = otherBounds.xMin - selfBounds.width / 2;
        }
        else if (minDist == bottomDist)
        {
            newVelocity.y = -Velocity.y;
            newPosition.y = otherBounds.yMin - selfBounds.height / 2;
        }
        else if (minDist == rightDist)
        {
            newVelocity.x = -Velocity.x;
            newPosition.x = otherBounds.xMax + selfBounds.width / 2;
        }
        else
        {
            newVelocity.y = -Velocity.y;
            newPosition.y = otherBounds.yMax + selfBounds.height / 2;
        }

        Velocity = newVelocity;
        Position = newPosition;
    }
}
