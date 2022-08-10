using UnityEngine;

public static class GameContextViewFactory
{
    public static GameContextView Create (IUnityUpdater viewUpdater, IGameSettings gameSettings)
    {
        EntityViewFactory viewFactory = new EntityViewFactory();

        EntitiesViewManager entitiesViewManager = new(
            viewUpdater,
            viewFactory,
            gameSettings.DebugSettings
        );

        return new GameContextView(
            entitiesViewManager
        );
    }
}
