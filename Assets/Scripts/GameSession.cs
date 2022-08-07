using System;
using UnityEngine;

public class GameSession : IDisposable
{
    readonly IPhysicsUpdater physicsUpdater;
    readonly IViewUpdater viewUpdater;
    readonly IEntitiesViewManager entitiesViewManager;

    readonly IPhysicsEntityFactory physicsEntityFactory;
    readonly IStageBounds stageBounds;
    readonly Physics physics;

    public GameSession (
        IPhysicsUpdater updater,
        IViewUpdater viewUpdater,
        Physics physics,
        IEntitiesViewManager entitiesViewManager,
        IPhysicsEntityFactory physicsEntityFactory,
        IStageBounds stageBounds
    )
    {
        this.entitiesViewManager = entitiesViewManager;
        this.physicsEntityFactory = physicsEntityFactory;
        this.stageBounds = stageBounds;
        this.physicsUpdater = updater;
        this.viewUpdater = viewUpdater;
        this.physics = physics;
    }

    public void Initialize ()
    {
        physics.Initialize();
        entitiesViewManager.Initialize();
        stageBounds.Initialize();
        CreatePlayer();
        CreateAsteroids();
    }

    void CreatePlayer ()
    {
        PlayerView entity = GameObject.Instantiate(Resources.Load<PlayerView>("Player"));
        IPhysicsEntity entity1 = physicsEntityFactory.Create();
        physics.AddEntity(entity1);
        entity.Initialize(entity1);
        entitiesViewManager.AddEntity(entity);
    }

    void CreateAsteroids ()
    {
        for (int i = 0; i < 10; i++)
        {
            EntityView ett = GameObject.Instantiate(Resources.Load<EntityView>("Asteroid"));
            IPhysicsEntity ett1 = physicsEntityFactory.Create();
            physics.AddEntity(ett1);
            ett.Initialize(ett1);
            entitiesViewManager.AddEntity(ett);
        }
    }

    public void Dispose ()
    {
        stageBounds.Dispose();
        physicsUpdater.Dispose();
        physics.Dispose();
    }
}
