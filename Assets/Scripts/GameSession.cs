using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameSessionModel : IDisposable
{
    readonly IPhysicsUpdater physicsUpdater;
    readonly IEntitiesViewManager entitiesViewManager;

    readonly IStageBounds stageBounds;
    readonly IEntityFactory entityFactory;
    readonly IGameSettings gameSettings;
    readonly Physics physics;

    public GameSessionModel (
        IPhysicsUpdater updater,
        Physics physics,
        IEntitiesViewManager entitiesViewManager,
        IStageBounds stageBounds,
        IEntityFactory entityFactory,
        IGameSettings gameSettings
    )
    {
        this.entitiesViewManager = entitiesViewManager;
        this.stageBounds = stageBounds;
        this.entityFactory = entityFactory;
        this.gameSettings = gameSettings;
        this.physicsUpdater = updater;
        this.physics = physics;
    }

    public void Initialize ()
    {
        physics.Initialize();
        entitiesViewManager.Initialize();
        stageBounds.Initialize();

        entityFactory.CreatePlayer(1, Vector3.left);
        entityFactory.CreatePlayer(2, Vector3.right);

        for (int i = 0; i < 10; i++)
        {
            entityFactory.CreateAsteroid(
                gameSettings.AsteroidSettings.StartingSize,
                stageBounds.RandomPointNearEdge(),
                new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f))
            );
        }
    }

    public void Dispose ()
    {
        stageBounds.Dispose();
        physicsUpdater.Dispose();
        physics.Dispose();
    }
}
