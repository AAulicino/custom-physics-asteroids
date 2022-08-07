using UnityEngine;

public class GameInitializer : MonoBehaviour
{
    GameSessionModel gameSession;

    void Awake ()
    {
        ViewUpdater viewUpdater = new GameObject("ViewUpdater").AddComponent<ViewUpdater>();
        GameSettings gameSettings = Resources.Load<GameSettings>("Settings/GameSettings");
        PhysicsUpdater physicsUpdater = new();

        Physics physics = new(
            physicsUpdater,
            new CollisionDetector(
                new QuadTree<IEntityModel>(
                    Camera.main.ViewportToWorldPoint(Vector2.zero),
                    Camera.main.ViewportToWorldPoint(Vector2.one),
                    new EntityBounds()
                ),
                gameSettings
            )
        );
        StageBounds stageBounds = new(physicsUpdater, viewUpdater);
        EntitiesViewManager entitiesViewManager = new(physicsUpdater, viewUpdater);

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
