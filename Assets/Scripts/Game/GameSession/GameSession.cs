using System;

public class GameSession : IDisposable
{
    readonly IUnityUpdater viewUpdater;
    readonly IGameSettings gameSettings;
    readonly IPhysicsUpdater physicsUpdater;
    readonly StageBoundsModel stageBounds;

    GameContextModel gameContextModel;
    GameContextView gameContextView;
    GameContextUIView gameContextUIView;

    public GameSession (
        IPhysicsUpdater physicsUpdater,
        IUnityUpdater viewUpdater,
        IGameSettings gameSettings
    )
    {
        this.viewUpdater = viewUpdater;
        this.gameSettings = gameSettings;
        this.physicsUpdater = physicsUpdater;

        stageBounds = new StageBoundsModel(physicsUpdater);
    }

    public void Initialize ()
    {
        gameContextUIView = GameContextUIViewFactory.Create();
        gameContextUIView.OnStartGameRequested += HandleStartGameRequested;
        gameContextUIView.Initialize();
    }

    void HandleStartGameRequested ()
    {
        gameContextModel = GameContextModelFactory.Create(
            physicsUpdater,
            gameSettings,
            stageBounds
        );

        gameContextView = GameContextViewFactory.Create(viewUpdater, gameSettings, stageBounds);
        gameContextModel.OnEntityCreated += HandleEntityCreated;
        gameContextModel.OnGameEnd += HandleGameEnd;

        gameContextModel.Initialize();
        gameContextView.Initialize();
        Pause(false);
    }

    public void Pause (bool pause)
    {
        gameContextModel?.Pause(pause);
    }

    void HandleEntityCreated (IEntityModel entity)
    {
        gameContextView.CreateEntity(entity);
    }

    void HandleGameEnd (bool victory)
    {
        Pause(true);
        gameContextModel.Dispose();
        viewUpdater.Schedule(() => gameContextUIView.ShowGameOver(victory));
    }

    public void Dispose ()
    {
        physicsUpdater.Dispose();
    }
}
