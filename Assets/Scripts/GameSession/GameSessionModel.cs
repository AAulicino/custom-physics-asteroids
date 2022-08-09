using System;

public class GameSessionModel : IDisposable
{
    readonly IPhysicsUpdater physicsUpdater;

    readonly IStageBounds stageBounds;
    readonly IEntityViewFactory viewFactory;
    readonly IGameModelManager gameModelManager;
    readonly IEntitiesViewManager viewManager;
    readonly IPhysicsEntityManager physics;

    public GameSessionModel (
        IPhysicsUpdater updater,
        IPhysicsEntityManager physics,
        IStageBounds stageBounds,
        IEntityViewFactory viewFactory,
        IGameModelManager gameModelManager,
        IEntitiesViewManager viewManager
    )
    {
        this.stageBounds = stageBounds;
        this.viewFactory = viewFactory;
        this.gameModelManager = gameModelManager;
        this.viewManager = viewManager;
        this.physicsUpdater = updater;
        this.physics = physics;

        gameModelManager.OnGameEnd += HandleGameEnd;
    }

    public void Initialize ()
    {
        physics.Initialize();
        viewManager.Initialize();
        stageBounds.Initialize();

        gameModelManager.OnEntityCreated += HandleEntityCreated;
        gameModelManager.Initialize();
    }

    void HandleEntityCreated (IEntityModel entity)
    {
        viewManager.CreateEntity(entity);
    }

    public void Pause (bool pause)
    {
        physicsUpdater.Pause(pause || gameModelManager.GameEnded);
    }

    void HandleGameEnd (bool obj)
    {
        Pause(true);
    }

    public void Dispose ()
    {
        gameModelManager.OnEntityCreated -= HandleEntityCreated;
        stageBounds.Dispose();
        physicsUpdater.Dispose();
        physics.Dispose();
    }
}
