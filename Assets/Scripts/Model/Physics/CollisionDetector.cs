using System.Collections.Generic;

public class CollisionDetector
{
    public ICollection<Collision> DetectCollisions (IReadOnlyList<IEntityModel> entities)
    {
        List<Collision> collisions = new List<Collision>();

        for (int i = 0; i < entities.Count; i++)
        {
            IColliderModel collider = entities[i].Collider;

            for (int j = 0; j < entities.Count; j++)
            {
                if (i == j)
                    continue;

                IColliderModel other = entities[j].Collider;

                if (collider.CollidesWith(other))
                    collisions.Add(new Collision(entities[i], other));
            }
        }

        return collisions;
    }
}
