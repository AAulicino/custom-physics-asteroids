using System;
using UnityEngine;

public class GameSession : IDisposable
{
    readonly IPhysicsUpdater updater;
    readonly Physics physics;
    readonly IEntitiesViewManager entitiesViewManager;

    public GameSession (
        IPhysicsUpdater updater,
        Physics physics,
        IEntitiesViewManager entitiesViewManager
    )
    {
        this.entitiesViewManager = entitiesViewManager;
        this.updater = updater;
        this.physics = physics;
    }

    public void Initialize ()
    {
        physics.Initialize();
        entitiesViewManager.Initialize();

        PlayerView entity = GameObject.Instantiate(Resources.Load<PlayerView>("Player"));
        entity.Initialize(physics.CreateEntity());
        entitiesViewManager.AddEntity(entity);
    }

    public void Dispose ()
    {
        updater.Dispose();
        physics.Dispose();
    }
}
