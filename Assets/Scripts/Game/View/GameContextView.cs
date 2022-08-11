using System;

public class GameContextView : IDisposable
{
    readonly IEntitiesViewManager viewManager;
    readonly StageBoundsView stageBoundsView;

    public GameContextView (
        IEntitiesViewManager viewManager,
        StageBoundsView stageBoundsView
    )
    {
        this.viewManager = viewManager;
        this.stageBoundsView = stageBoundsView;
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
        stageBoundsView.Dispose();
    }
}
