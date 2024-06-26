using UnityEngine;

public static class GameContextModelFactory
{
    public static GameContextModel Create (
        IPhysicsUpdater physicsUpdater,
        IGameSettings gameSettings,
        IStageBoundsModel stageBounds
    )
    {
        IPhysicsEntityManager physicsEntityManager = new PhysicsEntityManager(
            physicsUpdater,
            new CollisionHandlerModel(
                new QuadTree<IEntityModel>(
                    Camera.main.ViewportToWorldPoint(Vector2.zero),
                    Camera.main.ViewportToWorldPoint(Vector2.one),
                    new EntityBounds()
                ),
                gameSettings.PhysicsSettings,
                gameSettings.DebugSettings,
                stageBounds,
                new ColliderCollisionDetectorModel()
            )
        );

        IEntityModelFactory entityModelFactory = new EntityModelFactory(stageBounds, gameSettings);

        return new GameContextModel(
            physicsUpdater,
            physicsEntityManager,
            stageBounds,
            gameSettings,
            entityModelFactory
        );
    }
}
