using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameContextModel : IDisposable
{
    public event Action<IEntityModel> OnEntityCreated;
    public event Action<bool> OnGameEnd;

    readonly IPhysicsUpdater physicsUpdater;

    readonly IStageBounds stageBounds;
    readonly IPhysicsEntityManager physicsEntityManager;
    readonly IEntityModelFactory entityModelFactory;
    readonly IGameSettings settings;

    readonly List<IEntityModel> activePlayers = new();
    readonly List<IEntityModel> activeAsteroids = new();

    bool gameEnded;

    public GameContextModel (
        IPhysicsUpdater physicsUpdater,
        IPhysicsEntityManager physicsEntityManager,
        IStageBounds stageBounds,
        IGameSettings settings,
        IEntityModelFactory entityModelFactory
    )
    {
        this.stageBounds = stageBounds;
        this.physicsUpdater = physicsUpdater;
        this.physicsEntityManager = physicsEntityManager;
        this.settings = settings;
        this.entityModelFactory = entityModelFactory;

        entityModelFactory.OnEntityCreated += HandleEntityCreated;
    }

    public void Initialize ()
    {
        physicsEntityManager.Initialize();
        stageBounds.Initialize();
        CreatePlayers();
        CreateAsteroids();
    }

    public void Pause (bool pause)
    {
        physicsUpdater.Pause(pause || gameEnded);
    }

    void CreatePlayers ()
    {
        entityModelFactory.CreatePlayer(1, Vector3.left);
        entityModelFactory.CreatePlayer(2, Vector3.right);
    }

    void CreateAsteroids ()
    {
        for (int i = 0; i < settings.AsteroidSettings.StartingCount; i++)
        {
            entityModelFactory.CreateAsteroid(
                settings.AsteroidSettings.StartingSize,
                stageBounds.RandomPointNearEdge(),
                new Vector2(Random.Range(-2f, 2f), Random.Range(-2f, 2f))
            );
        }
    }

    void HandleEntityCreated (IEntityModel entity)
    {
        if (entity is IAsteroidModel)
            activeAsteroids.Add(entity);
        else if (entity is IPlayerModel)
            activePlayers.Add(entity);

        physicsEntityManager.AddEntity(entity);
        entity.OnDestroy += HandleEntityDestroyed;
        OnEntityCreated?.Invoke(entity);
    }

    void HandleEntityDestroyed (IEntityModel entity)
    {
        entity.OnDestroy -= HandleEntityDestroyed;
        physicsEntityManager.RemoveEntity(entity);

        if (entity is AsteroidModel)
            HandleAsteroidDestroyed(entity);
        else if (entity is PlayerModel)
            HandlePlayerDestroyed(entity);
    }

    void HandleAsteroidDestroyed (IEntityModel asteroid)
    {
        asteroid.OnDestroy -= HandleAsteroidDestroyed;
        activeAsteroids.Remove(asteroid);

        if (activeAsteroids.Count == 0)
            HandleGameEnd(true);
    }

    void HandlePlayerDestroyed (IEntityModel player)
    {
        player.OnDestroy -= HandlePlayerDestroyed;
        activePlayers.Remove(player);

        if (activePlayers.Count == 0)
            HandleGameEnd(false);
    }

    void HandleGameEnd (bool ended)
    {
        OnGameEnd?.Invoke(ended);
        gameEnded = true;
    }

    public void Dispose ()
    {
        foreach (IEntityModel player in activePlayers)
            player.Destroy();
        foreach (IEntityModel asteroid in activeAsteroids)
            asteroid.Destroy();

        activePlayers.Clear();
        activeAsteroids.Clear();

        stageBounds.Dispose();
        physicsUpdater.Dispose();
        physicsEntityManager.Dispose();
    }
}
