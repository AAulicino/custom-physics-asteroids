using System;
using UnityEngine;

public class GameSessionModel : IDisposable
{
    readonly IPhysicsUpdater physicsUpdater;
    readonly IEntitiesViewManager entitiesViewManager;

    readonly IStageBounds stageBounds;
    readonly IEntityFactory entityFactory;
    readonly Physics physics;

    public GameSessionModel (
        IPhysicsUpdater updater,
        Physics physics,
        IEntitiesViewManager entitiesViewManager,
        IStageBounds stageBounds,
        IEntityFactory entityFactory
    )
    {
        this.entitiesViewManager = entitiesViewManager;
        this.stageBounds = stageBounds;
        this.entityFactory = entityFactory;
        this.physicsUpdater = updater;
        this.physics = physics;
    }

    public void Initialize ()
    {
        physics.Initialize();
        entitiesViewManager.Initialize();
        stageBounds.Initialize();
        entityFactory.CreatePlayer(Vector3.zero);
        for (int i = 0; i < 10; i++)
            entityFactory.CreateAsteroid(Vector3.zero);
    }

    public void Dispose ()
    {
        stageBounds.Dispose();
        physicsUpdater.Dispose();
        physics.Dispose();
    }
}
