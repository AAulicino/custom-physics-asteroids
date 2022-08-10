using System.Collections.Generic;

public interface ICollisionHandler
{
    void DetectCollisions (IReadOnlyList<IEntityModel> entities, List<Collision> collisionsBuffer);
}
