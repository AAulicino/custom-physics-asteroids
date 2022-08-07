using System.Collections.Generic;

public class EntitiesViewManager : IEntitiesViewManager
{
    readonly IPhysicsUpdater physicsUpdater;
    readonly IViewUpdater viewUpdater;

    readonly HashSet<EntityView> activeEntities = new();
    readonly Queue<AddOrRemoveOperation<EntityView>> operations = new();

    public EntitiesViewManager (IPhysicsUpdater physicsUpdater, IViewUpdater viewUpdater)
    {
        this.physicsUpdater = physicsUpdater;
        this.viewUpdater = viewUpdater;
    }

    public void Initialize ()
    {
        viewUpdater.OnUpdate += HandleUpdate;
    }

    public void AddEntity (EntityView entity)
    {
        operations.Enqueue(new AddOrRemoveOperation<EntityView>(true, entity));

        entity.OnDestroy += HandleEntityDestroyed;
    }

    void HandleUpdate ()
    {
        while (operations.Count > 0)
        {
            AddOrRemoveOperation<EntityView> operation = operations.Dequeue();
            if (operation.IsAdd)
                activeEntities.Add(operation.Value);
            else
                activeEntities.Remove(operation.Value);
        }

        foreach (EntityView entity in activeEntities)
            entity.OnViewUpdate();
    }

    void HandleEntityDestroyed (EntityView entity)
    {
        entity.OnDestroy -= HandleEntityDestroyed;

        operations.Enqueue(new AddOrRemoveOperation<EntityView>(false, entity));
    }
}
