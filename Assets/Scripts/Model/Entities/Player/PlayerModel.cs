using System.Diagnostics;
using UnityEngine;

public class PlayerModel : EntityModel, IPlayerModel
{
    public int PlayerId { get; private set; }

    readonly IPlayerSettings playerSettings;
    readonly IEntityModelFactory entityFactory;
    readonly Stopwatch watch = Stopwatch.StartNew();

    public PlayerModel (
        int playerId,
        IPlayerSettings playerSettings,
        IRigidBodyModel rigidBody,
        IColliderModel collider,
        IEntityModelFactory entityFactory
    ) : base(rigidBody, collider)
    {
        PlayerId = playerId;
        this.playerSettings = playerSettings;
        this.entityFactory = entityFactory;
    }

    public override void OnPhysicsStep (float deltaTime)
    {
        base.OnPhysicsStep(deltaTime);

        Vector2 velocity = RigidBody.Velocity;

        if (velocity.sqrMagnitude <= 0)
            return;

        RigidBody.Rotation = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg - 90;
    }

    public void FireProjectile ()
    {
        if (watch.Elapsed.TotalSeconds < playerSettings.FireRate)
            return;
        entityFactory.CreateProjectile(
            RigidBody.Position,
            RigidBody.Rotation,
            RigidBody.Velocity
        );
        watch.Restart();
    }
}
