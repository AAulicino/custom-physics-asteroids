using UnityEngine;

public class GameInitializer : MonoBehaviour
{
    GameSession gameSession;

    void Awake ()
    {
        ViewUpdater viewUpdater = new GameObject("ViewUpdater").AddComponent<ViewUpdater>();
        PhysicsUpdater physicsUpdater = new PhysicsUpdater();

        StageBounds stageBounds = new StageBounds(physicsUpdater, viewUpdater);
        IPhysicsEntityFactory entityFactory = new PhysicsEntityFactory(stageBounds);

        gameSession = new GameSession(
            physicsUpdater,
            viewUpdater,
            new Physics(
                physicsUpdater,
                new CollisionDetector()
            ),
            new EntitiesViewManager(viewUpdater),
            entityFactory,
            stageBounds
        );

        gameSession.Initialize();
    }

    void OnDestroy ()
    {
        gameSession.Dispose();
    }
}
