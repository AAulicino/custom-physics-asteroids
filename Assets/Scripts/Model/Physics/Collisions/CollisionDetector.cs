using System.Collections.Generic;
using UnityEngine;

public class CollisionDetector
{
    readonly IQuadTree<IEntityModel> quadTree;
    readonly IGameSettings gameSettings;

    public CollisionDetector (IQuadTree<IEntityModel> quadTree, IGameSettings gameSettings)
    {
        this.gameSettings = gameSettings;
        this.quadTree = quadTree;
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

                if (collider.CollidesWith(other.Collider))
                    collisionsBuffer.Add(new Collision(entities[i], other.Collider));
            }
        }
    }

    void RenderQuadTree ()
    {
        foreach (QuadTreeRect quad in quadTree.GetGrid())
            DebugExtension.DebugQuadTreeRect(quad, Color.red, gameSettings.TimeStep);
    }
}
