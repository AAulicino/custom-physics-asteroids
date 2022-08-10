using System;
using System.Collections.Generic;

public class EntitiesViewManager : IEntitiesViewManager
{
    readonly IUnityUpdater viewUpdater;
    readonly EntityViewFactory viewFactory;
    readonly IDebugSettings debugSettings;
    readonly HashSet<EntityView> activeEntities = new();
    readonly Queue<AddOrRemoveOperation<object>> operations = new();

    public EntitiesViewManager (
        IUnityUpdater viewUpdater,
        EntityViewFactory viewFactory,
        IDebugSettings debugSettings
    )
    {
        this.viewUpdater = viewUpdater;
        this.viewFactory = viewFactory;
        this.debugSettings = debugSettings;
    }

    public void Initialize ()
    {
        viewUpdater.OnUpdate += HandleUpdate;
    }

    public void CreateEntity (IEntityModel entity)
    {
        lock (operations)
            operations.Enqueue(new AddOrRemoveOperation<object>(true, entity));
    }

    void HandleUpdate ()
    {
        lock (operations)
        {
            while (operations.Count > 0)
            {
                AddOrRemoveOperation<object> operation = operations.Dequeue();

                if (operation.IsAdd)
                    activeEntities.Add(CreateView((IEntityModel)operation.Value));
                else
                    activeEntities.Remove((EntityView)operation.Value);
            }

            foreach (EntityView entity in activeEntities)
                entity.OnViewUpdate();
        }
    }

    EntityView CreateView (IEntityModel entity)
    {
        EntityView view = entity switch
        {
            IPlayerModel player => viewFactory.CreatePlayer(player.PlayerId),
            IAsteroidModel => viewFactory.CreateAsteroid(),
            IProjectileModel => viewFactory.CreateProjectile(),
            _ => throw new ArgumentException($"Unknown entity type: {entity.GetType()}"),
        };
        view.OnDestroy += HandleEntityDestroyed;
        view.Initialize(entity, debugSettings);
        return view;
    }

    void HandleEntityDestroyed (EntityView entity)
    {
        entity.OnDestroy -= HandleEntityDestroyed;

        lock (operations)
            operations.Enqueue(new AddOrRemoveOperation<object>(false, entity));
    }

    public void Dispose ()
    {
        lock (operations)
            operations.Clear();
        viewFactory.Dispose();
    }
}
