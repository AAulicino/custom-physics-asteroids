using UnityEngine;

public class GameInitializer : MonoBehaviour
{
    GameSessionModel gameSession;

    void Awake ()
    {
        ViewUpdater viewUpdater = new GameObject("ViewUpdater").AddComponent<ViewUpdater>();
        GameSettings gameSettings = Resources.Load<GameSettings>("Settings/GameSettings");
        PhysicsUpdater physicsUpdater = new(gameSettings.PhysicsSettings);

        IPhysicsEntityManager physicsEntityManager = new PhysicsEntityManager(
            physicsUpdater,
            new CollisionDetector(
                new QuadTree<IEntityModel>(
                    Camera.main.ViewportToWorldPoint(Vector2.zero),
                    Camera.main.ViewportToWorldPoint(Vector2.one),
                    new EntityBounds()
                ),
                gameSettings.PhysicsSettings,
                gameSettings.DebugSettings
            )
        );
        StageBounds stageBounds = new(physicsUpdater, viewUpdater);
        EntityViewFactory viewFactory = new EntityViewFactory();

        EntitiesViewManager entitiesViewManager = new(
            viewUpdater,
            viewFactory,
            gameSettings.DebugSettings
        );

        GameModelManager gameModelManager = new GameModelManager(
            gameSettings,
            new EntityModelFactory(stageBounds, gameSettings),
            stageBounds,
            physicsEntityManager
        );

        gameSession = new GameSessionModel(
            physicsUpdater,
            physicsEntityManager,
            stageBounds,
            viewFactory,
            gameModelManager,
            entitiesViewManager
        );

        gameSession.Initialize();

        ListenToEditorPause();
    }

    void OnApplicationPause (bool pauseStatus)
    {
        gameSession.Pause(pauseStatus || UnityEditor.EditorApplication.isPaused);
    }

    void OnDestroy ()
    {
        gameSession.Dispose();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.pauseStateChanged -= HandlePauseStateChanged;
#endif
    }

    void ListenToEditorPause ()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.pauseStateChanged += HandlePauseStateChanged;
    }

    void HandlePauseStateChanged (UnityEditor.PauseState state)
    {
        OnApplicationPause(state == UnityEditor.PauseState.Paused);
#endif
    }
}
