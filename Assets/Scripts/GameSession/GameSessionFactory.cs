using UnityEngine;

public static class GameSessionFactory
{
    public static GameSessionModel Create ()
    {
        ViewUpdater viewUpdater = new GameObject("ViewUpdater").AddComponent<ViewUpdater>();
        GameSettings gameSettings = Resources.Load<GameSettings>("Settings/GameSettings");
        PhysicsUpdater physicsUpdater = new(gameSettings.PhysicsSettings);
        StageBounds stageBounds = new(physicsUpdater, viewUpdater);

        IPhysicsEntityManager physicsEntityManager = new PhysicsEntityManager(
            physicsUpdater,
            new CollisionDetector(
                new QuadTree<IEntityModel>(
                    Camera.main.ViewportToWorldPoint(Vector2.zero),
                    Camera.main.ViewportToWorldPoint(Vector2.one),
                    new EntityBounds()
                ),
                gameSettings.PhysicsSettings,
                gameSettings.DebugSettings,
                stageBounds
            )
        );
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

        return new GameSessionModel(
            physicsUpdater,
            physicsEntityManager,
            stageBounds,
            viewFactory,
            gameModelManager,
            entitiesViewManager
        );
    }
}
