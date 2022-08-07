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

        EntityFactory entityFactory = new EntityFactory(
            new EntityModelFactory(stageBounds),
            new EntityViewFactory(),
            physics,
            entitiesViewManager
        );

        gameSession = new GameSessionModel(
            physicsUpdater,
            physics,
            entitiesViewManager,
            stageBounds,
            entityFactory
        );

        gameSession.Initialize();
    }

    void OnDestroy ()
    {
        gameSession.Dispose();
    }
}
