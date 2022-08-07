using System;
using System.Collections.Generic;
using UnityEngine;

public class EntityFactory : IEntityFactory
{
    readonly IEntityModelFactory modelFactory;
    readonly IEntityViewFactory viewFactory;
    readonly Physics physics;
    readonly IEntitiesViewManager viewManager;
    readonly ViewUpdater viewUpdater;

    readonly Queue<Action> factoryQueue = new();

    public EntityFactory (
        IEntityModelFactory modelFactory,
        IEntityViewFactory viewFactory,
        Physics physics,
        IEntitiesViewManager viewManager,
        ViewUpdater viewUpdater
    )
    {
        this.modelFactory = modelFactory;
        this.viewFactory = viewFactory;
        this.physics = physics;
        this.viewManager = viewManager;
        this.viewUpdater = viewUpdater;

        viewUpdater.OnUpdate += ProcessQueue;
    }

    public void CreatePlayer (int playerId, Vector3 position)
    {
        factoryQueue.Enqueue(() =>
        {
            IPlayerModel model = modelFactory.CreatePlayer(playerId, position, this);
            PlayerView view = viewFactory.CreatePlayer(playerId);
            CreateEntity(model, view);
        });
    }

    public void CreateAsteroid (int size, Vector3 position, Vector3 velocity)
    {
        factoryQueue.Enqueue(() =>
        {
            IAsteroidModel model = modelFactory.CreateAsteroid(size, position, velocity, this);
            AsteroidView view = viewFactory.CreateAsteroid();
            CreateEntity(model, view);
        });
    }

    public void CreateProjectile (Vector3 position, float rotation, Vector3 velocity)
    {
        factoryQueue.Enqueue(() =>
        {
            IProjectileModel model = modelFactory.CreateProjectile(position, rotation, velocity);
            ProjectileView view = viewFactory.CreateProjectile();
            CreateEntity(model, view);
        });
    }

    void ProcessQueue ()
    {
        while (factoryQueue.Count > 0)
            factoryQueue.Dequeue()();
    }

    void CreateEntity (IEntityModel model, EntityView view)
    {
        physics.AddEntity(model);
        view.Initialize(model);
        viewManager.AddEntity(view);
        model.OnDestroy += HandleEntityDestroyed;
    }

    void HandleEntityDestroyed (IEntityModel model)
    {
        model.OnDestroy -= HandleEntityDestroyed;
        physics.RemoveEntity(model);
    }

    public void Dispose ()
    {
        viewUpdater.OnUpdate -= ProcessQueue;
    }
}
