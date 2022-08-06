using UnityEngine;

public class GameInitializer : MonoBehaviour
{
    GameSession gameSession;

    void Awake ()
    {
        var min = Camera.main.ScreenToWorldPoint(Vector2.zero);
        var max = Camera.main.ViewportToWorldPoint(Vector2.one);

        ScreenCollider[] screenBounds = new[]
        {
            new ScreenCollider(Rect.MinMaxRect(min.x, max.y, max.x, max.y + 1)),
            new ScreenCollider(Rect.MinMaxRect(min.x - 1, min.y, min.x, max.y)),
            new ScreenCollider(Rect.MinMaxRect(min.x, min.y - 1, max.x, min.y )),
            new ScreenCollider(Rect.MinMaxRect(max.x, min.y, max.x + 1, max.y)),
        };

        ViewUpdater viewUpdater = new GameObject("ViewUpdater").AddComponent<ViewUpdater>();

        PhysicsUpdater updater = new PhysicsUpdater();
        gameSession = new GameSession(
            updater,
            new Physics(
                updater,
                new CollisionDetector(screenBounds)
            ),
            new EntitiesViewManager(viewUpdater)
        );

        DebugExtension.DebugRect(screenBounds[0].Bounds, Color.red, 100);
        DebugExtension.DebugRect(screenBounds[1].Bounds, Color.green, 100);
        DebugExtension.DebugRect(screenBounds[2].Bounds, Color.blue, 100);
        DebugExtension.DebugRect(screenBounds[3].Bounds, Color.yellow, 100);

        gameSession.Initialize();
    }

    void OnDestroy ()
    {
        gameSession.Dispose();
    }
}
