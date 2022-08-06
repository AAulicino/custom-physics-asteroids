using System;
using UnityEngine;

public class GameSession : IDisposable
{
    readonly IPhysicsUpdater updater;
    readonly Physics physics;

    public GameSession (IPhysicsUpdater updater, Physics physics)
    {
        this.updater = updater;
        this.physics = physics;
    }

    public void Initialize ()
    {
        physics.Initialize();

        Entity entity = GameObject.Instantiate(Resources.Load<Entity>("Entity"));
        entity.Initialize(physics.CreateEntity());
        entity.physicsEntity.RigidBody.Velocity = Vector2.one;
    }

    public void Dispose ()
    {
        updater.Dispose();
        physics.Dispose();
    }
}
