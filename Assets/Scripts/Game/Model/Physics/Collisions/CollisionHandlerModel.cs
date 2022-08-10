using System.Collections.Generic;
using UnityEngine;

public class CollisionHandlerModel : ICollisionHandlerModel
{
    readonly IPhysicsSettings physicsSettings;
    readonly IDebugSettings gameSettings;
    readonly IStageBounds stageBounds;
    readonly IColliderCollisionDetectorModel colliderCollisions;
    readonly IQuadTree<IEntityModel> quadTree;

    public CollisionHandlerModel (
        IQuadTree<IEntityModel> quadTree,
        IPhysicsSettings physicsSettings,
        IDebugSettings debugSettings,
        IStageBounds stageBounds,
        IColliderCollisionDetectorModel shapeCollisions
    )
    {
        this.gameSettings = debugSettings;
        this.stageBounds = stageBounds;
        this.colliderCollisions = shapeCollisions;
        this.quadTree = quadTree;
        this.physicsSettings = physicsSettings;
    }

    public void DetectCollisions (
        IReadOnlyList<IEntityModel> entities,
        List<Collision> collisionsBuffer
    )
    {
        quadTree.ClearAndUpdateMainRect(stageBounds.Rect);
        quadTree.InsertRange(entities);

        if (gameSettings.RenderCollisionQuadTree)
            RenderQuadTree();

        collisionsBuffer.Clear();

        for (int i = 0; i < entities.Count; i++)
        {
            IEntityModel current = entities[i];
            IColliderModel collider = current.Collider;

            foreach (IEntityModel other in quadTree.GetNearestObjects(current))
            {
                if (current == other)
                    continue;

                if (DetectCollision(collider, other.Collider))
                    collisionsBuffer.Add(new Collision(current, other.Collider));
            }
        }
    }

    bool DetectCollision (IColliderModel a, IColliderModel b)
    {
        if (!colliderCollisions.LayerCollidesWith(a.Layer, b.Layer))
            return false;

        ICircleColliderModel aCircle = a as ICircleColliderModel;
        ICircleColliderModel bCircle = b as ICircleColliderModel;

        if (aCircle != null && bCircle != null)
            return colliderCollisions.IsColliding(aCircle, bCircle);

        ISquareColliderModel aSquare = a as ISquareColliderModel;
        ISquareColliderModel bSquare = b as ISquareColliderModel;

        if (aSquare != null && bSquare != null)
            return colliderCollisions.IsColliding(aSquare, bSquare);

        return colliderCollisions.IsColliding(aSquare ?? bSquare, aCircle ?? bCircle);
    }

    void RenderQuadTree ()
    {
        foreach (QuadTreeRect quad in quadTree.GetGrid())
            DebugExtension.DebugQuadTreeRect(quad, Color.red, physicsSettings.TimeStep);
    }
}
