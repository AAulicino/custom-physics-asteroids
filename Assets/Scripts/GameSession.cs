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
    readonly IPhysicsEntityManager physics;

    public GameSessionModel (
        IPhysicsUpdater updater,
        IPhysicsEntityManager physics,
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

        CreatePlayers();
        CreateAsteroids();
    }

    public void Pause (bool pause)
    {
        physicsUpdater.Pause(pause);
    }

    void CreatePlayers ()
    {
        entityFactory.CreatePlayer(1, Vector3.left);
        entityFactory.CreatePlayer(2, Vector3.right);
    }

    void CreateAsteroids ()
    {
        for (int i = 0; i < gameSettings.AsteroidSettings.StartingCount; i++)
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
