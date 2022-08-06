using System.Collections.Generic;

public class EntitiesViewManager : IEntitiesViewManager
{
    readonly IViewUpdater viewUpdater;

    readonly HashSet<EntityView> activeEntities = new();

    public EntitiesViewManager (IViewUpdater viewUpdater)
    {
        this.viewUpdater = viewUpdater;
    }

    public void Initialize ()
    {
        viewUpdater.OnUpdate += HandleUpdate;
    }

    public void AddEntity (EntityView entity)
    {
        activeEntities.Add(entity);
    }

    public void RemoveEntity (EntityView entity)
    {
        activeEntities.Remove(entity);
    }

    void HandleUpdate ()
    {
        foreach (EntityView entity in activeEntities)
            entity.Sync();
    }
}
