using System.Collections.Generic;

public class CollisionDetector
{
    public ICollection<Collision> DetectCollisions (IReadOnlyList<IPhysicsEntity> entities)
    {
        List<Collision> collisions = new List<Collision>();

        for (int i = 0; i < entities.Count; i++)
        {
            ICollider collider = entities[i].Collider;

            for (int j = 0; j < entities.Count; j++)
            {
                if (i == j)
                    continue;

                ICollider other = entities[j].Collider;

                if (collider.CollidesWith(other))
                    collisions.Add(new Collision(entities[i], other));
            }
        }

        return collisions;
    }
}
