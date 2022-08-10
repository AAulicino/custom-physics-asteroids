using System;

public class GameSession : IDisposable
{
    readonly IUnityUpdater viewUpdater;
    readonly IGameSettings gameSettings;
    readonly PhysicsUpdater physicsUpdater;
    readonly StageBounds stageBounds;

    GameContextModel gameContextModel;
    GameContextView gameContextView;
    GameContextUIView gameContextUIView;

    public GameSession (IUnityUpdater viewUpdater, IGameSettings gameSettings)
    {
        this.viewUpdater = viewUpdater as UnityUpdater;
        this.gameSettings = gameSettings as GameSettings;

        physicsUpdater = new PhysicsUpdater(gameSettings.PhysicsSettings);
        stageBounds = new StageBounds(physicsUpdater, viewUpdater);
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

        gameContextView = GameContextViewFactory.Create(viewUpdater, gameSettings);
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
