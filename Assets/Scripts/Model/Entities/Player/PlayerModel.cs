using System;
using System.Diagnostics;

public class PlayerModel : EntityModel, IPlayerModel
{
    public event Action<IPlayerModel> OnProjectileFired;

    const float fireRate = 0.2f;

    readonly IEntityFactory entityFactory;

    Stopwatch watch = Stopwatch.StartNew();

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
        if (watch.Elapsed.TotalSeconds < fireRate)
            return;
        entityFactory.CreateProjectile(
            RigidBody.Position,
            RigidBody.Rotation,
            RigidBody.Velocity
        );
        watch.Restart();
    }
}
