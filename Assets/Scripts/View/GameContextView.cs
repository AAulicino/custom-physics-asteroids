using System;

public class GameContextView : IDisposable
{
    readonly IEntitiesViewManager viewManager;

    public GameContextView (IEntitiesViewManager viewManager)
    {
        this.viewManager = viewManager;
    }

    public void Initialize ()
    {
        viewManager.Initialize();
    }

    public void CreateEntity (IEntityModel entity)
    {
        viewManager.CreateEntity(entity);
    }

    public void Dispose ()
    {
        viewManager.Dispose();
    }
}
