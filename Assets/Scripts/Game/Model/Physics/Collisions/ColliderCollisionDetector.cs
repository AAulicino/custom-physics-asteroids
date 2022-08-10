using UnityEngine;

public class ColliderCollisionDetector : IColliderCollisionDetector
{
    public bool IsColliding (ICircleColliderModel a, ICircleColliderModel b)
    {
        return Vector2.SqrMagnitude(a.Position - b.Position) < a.SqrRadius + b.SqrRadius;
    }

    public bool IsColliding (ISquareColliderModel a, ISquareColliderModel b)
    {
        return a.Bounds.Overlaps(b.Bounds);
    }

    public bool IsColliding (ISquareColliderModel square, ICircleColliderModel circle)
    {
        static float Square (float a) => a * a;

        float sqDistanceBetweenCenters = (square.Position - circle.Position).sqrMagnitude;

        if (sqDistanceBetweenCenters > Square(square.OuterRadius + circle.Radius))
            return false;

        if (sqDistanceBetweenCenters < Square(square.InnerRadius + circle.Radius))
            return true;

        Vector2 dir = (circle.Position - square.Position).normalized;
        Vector2 outerPoint = circle.Position + circle.Radius * dir;
        return square.Bounds.Contains(outerPoint);
    }

    public bool LayerCollidesWith (CollisionLayer a, CollisionLayer b)
    {
        // TODO solve layer using bitmask
        switch (a)
        {
            case CollisionLayer.Player:
                switch (b)
                {
                    case CollisionLayer.Asteroid:
                        return true;
                    case CollisionLayer.Player:
                    case CollisionLayer.Projectile:
                        return false;
                }
                break;

            case CollisionLayer.Asteroid:
                switch (b)
                {
                    case CollisionLayer.Player:
                    case CollisionLayer.Projectile:
                        return true;
                    case CollisionLayer.Asteroid:
                        return false;
                }
                break;

            case CollisionLayer.Projectile:
                switch (b)
                {
                    case CollisionLayer.Player:
                    case CollisionLayer.Projectile:
                        return false;
                    case CollisionLayer.Asteroid:
                        return true;
                }
                break;
        }

        return false;
    }
}
