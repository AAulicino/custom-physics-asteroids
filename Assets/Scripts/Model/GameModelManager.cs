using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameModelManager : IGameModelManager
{
    public event Action<IEntityModel> OnEntityCreated;
    public event Action<bool> OnGameEnd;

    public bool GameEnded { get; private set; }

    readonly IEntityModelFactory entityModelFactory;
    readonly IStageBounds stageBounds;
    readonly IPhysicsEntityManager physicsEntityManager;
    readonly IGameSettings settings;

    readonly List<IEntityModel> activePlayers = new();
    readonly List<IEntityModel> activeAsteroids = new();

    public GameModelManager (
        IGameSettings settings,
        IEntityModelFactory entityModelFactory,
        IStageBounds stageBounds,
        IPhysicsEntityManager physicsEntityManager
    )
    {
        this.settings = settings;
        this.entityModelFactory = entityModelFactory;
        this.stageBounds = stageBounds;
        this.physicsEntityManager = physicsEntityManager;
        entityModelFactory.OnEntityCreated += HandleEntityCreated;
    }

    public void Initialize ()
    {
        CreatePlayers();
        CreateAsteroids();
    }

    void CreatePlayers ()
    {
        entityModelFactory.CreatePlayer(1, Vector3.left);
        entityModelFactory.CreatePlayer(2, Vector3.right);
    }

    void CreateAsteroids ()
    {
        Debug.DrawLine(stageBounds.RandomPointNearEdge(), stageBounds.RandomPointNearEdge(), Color.red, 10f);
        for (int i = 0; i < settings.AsteroidSettings.StartingCount; i++)
        {
            entityModelFactory.CreateAsteroid(
                settings.AsteroidSettings.StartingSize,
                stageBounds.RandomPointNearEdge(),
                new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f))
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
        {
            OnGameEnd?.Invoke(true);
            GameEnded = true;
        }
    }

    void HandlePlayerDestroyed (IEntityModel player)
    {
        player.OnDestroy -= HandlePlayerDestroyed;
        activePlayers.Remove(player);

        if (activePlayers.Count == 0)
        {
            OnGameEnd?.Invoke(false);
            GameEnded = true;
        }
    }
}
