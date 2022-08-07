using UnityEngine;

public class GameInitializer : MonoBehaviour
{
    GameSessionModel gameSession;

    void Awake ()
    {
        ViewUpdater viewUpdater = new GameObject("ViewUpdater").AddComponent<ViewUpdater>();
        PhysicsUpdater physicsUpdater = new();
        Physics physics = new(
            physicsUpdater,
            new CollisionDetector()
        );
        StageBounds stageBounds = new(physicsUpdater, viewUpdater);
        EntitiesViewManager entitiesViewManager = new(physicsUpdater, viewUpdater);

        GameSettings gameSettings = Resources.Load<GameSettings>("Settings/GameSettings");

        EntityFactory entityFactory = new EntityFactory(
            new EntityModelFactory(
                stageBounds,
                gameSettings
            ),
            new EntityViewFactory(),
            physics,
            entitiesViewManager,
            viewUpdater
        );

        gameSession = new GameSessionModel(
            physicsUpdater,
            physics,
            entitiesViewManager,
            stageBounds,
            entityFactory,
            gameSettings
        );

        gameSession.Initialize();
    }

    void OnDestroy ()
    {
        gameSession.Dispose();
    }
}
