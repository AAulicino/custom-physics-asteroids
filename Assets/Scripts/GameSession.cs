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

        for (int i = 0; i < 100; i++)
        {
            Entity entity = GameObject.Instantiate(Resources.Load<Entity>("Entity"));
            entity.Initialize(physics.CreateEntity());
            entity.physicsEntity.RigidBody.Velocity = new Vector2(UnityEngine.Random.value, UnityEngine.Random.value);
        }
    }

    public void Dispose ()
    {
        updater.Dispose();
        physics.Dispose();
    }
}
