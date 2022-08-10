using System.Collections.Generic;

public interface ICollisionHandlerModel
{
    void DetectCollisions (IReadOnlyList<IEntityModel> entities, List<Collision> collisionsBuffer);
}
