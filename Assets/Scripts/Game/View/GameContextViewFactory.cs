using UnityEngine;

public static class GameContextViewFactory
{
    public static GameContextView Create (
        IUnityUpdater viewUpdater,
        IGameSettings gameSettings,
        IStageBoundsModel stageBoundsModel
    )
    {
        EntityViewFactory viewFactory = new EntityViewFactory();

        EntitiesViewManager entitiesViewManager = new(
            viewUpdater,
            viewFactory,
            gameSettings.DebugSettings
        );

        StageBoundsView bounds = new GameObject("StageBoundsView").AddComponent<StageBoundsView>();
        bounds.Setup(stageBoundsModel);

        return new GameContextView(
            entitiesViewManager,
            bounds
        );
    }
}
