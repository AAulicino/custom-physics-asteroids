using System;

public class PlayerModel : EntityModel, IPlayerModel
{
    public event Action<IPlayerModel> OnProjectileFired;

    readonly IEntityFactory entityFactory;

    public PlayerModel (
        IRigidBody rigidBody,
        ICollider collider,
        IEntityFactory entityFactory
    ) : base(rigidBody, collider)
    {
        this.entityFactory = entityFactory;
    }

    public void FireProjectile ()
    {
        entityFactory.CreateProjectile(
            RigidBody.Position,
            RigidBody.Rotation,
            RigidBody.Velocity
        );
    }
}
