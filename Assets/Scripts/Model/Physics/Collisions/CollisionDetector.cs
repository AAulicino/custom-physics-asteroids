using System.Collections.Generic;
using UnityEngine;

public class CollisionDetector
{
    readonly IQuadTree<IEntityModel> quadTree;
    readonly IPhysicsSettings physicsSettings;
    readonly IDebugSettings gameSettings;

    public CollisionDetector (
        IQuadTree<IEntityModel> quadTree,
        IPhysicsSettings physicsSettings,
        IDebugSettings debugSettings
    )
    {
        this.gameSettings = debugSettings;
        this.quadTree = quadTree;
        this.physicsSettings = physicsSettings;
    }

    public void DetectCollisions (
        IReadOnlyList<IEntityModel> entities,
        List<Collision> collisionsBuffer
    )
    {
        quadTree.Clear();
        quadTree.InsertRange(entities);

        if (gameSettings.RenderCollisionQuadTree)
            RenderQuadTree();

        collisionsBuffer.Clear();

        for (int i = 0; i < entities.Count; i++)
        {
            IColliderModel collider = entities[i].Collider;

            foreach (IEntityModel other in quadTree.GetNearestObjects(entities[i]))
            {
                if (other == entities[i])
                    continue;

                if (DetectCollision(collider, other.Collider))
                    collisionsBuffer.Add(new Collision(entities[i], other.Collider));
            }
        }
    }

    bool DetectCollision (IColliderModel a, IColliderModel b)
    {
        if (!LayerCollidesWith(a.Layer, b.Layer))
            return false;

        ICircleColliderModel aCircle;
        SquareColliderModel aSquare;
        ICircleColliderModel bCircle;
        SquareColliderModel bSquare;

        aCircle = a as ICircleColliderModel;
        aSquare = a as SquareColliderModel;
        bCircle = b as ICircleColliderModel;
        bSquare = b as SquareColliderModel;

        if (aCircle != null && bCircle != null)
            return CircleCircleCollision(aCircle, bCircle);

        if (aSquare != null && bSquare != null)
            return SquareSquareCollision(aSquare, bSquare);

        return SquareCircleCollision(aSquare ?? bSquare, aCircle ?? bCircle);
    }

    bool CircleCircleCollision (ICircleColliderModel a, ICircleColliderModel b)
    {
        return Vector2.SqrMagnitude(a.Position - b.Position) < a.SqrRadius + b.SqrRadius;
    }

    bool SquareSquareCollision (SquareColliderModel a, SquareColliderModel b)
    {
        return a.Bounds.Overlaps(b.Bounds);
    }

    bool SquareCircleCollision (SquareColliderModel square, ICircleColliderModel circle)
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


    bool LayerCollidesWith (CollisionLayer a, CollisionLayer b)
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

    void RenderQuadTree ()
    {
        foreach (QuadTreeRect quad in quadTree.GetGrid())
            DebugExtension.DebugQuadTreeRect(quad, Color.red, physicsSettings.TimeStep);
    }
}
