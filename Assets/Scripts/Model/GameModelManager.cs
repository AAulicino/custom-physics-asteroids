using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameModelManager : IGameModelManager
{
    public event Action<IEntityModel> OnEntityCreated;

    readonly IEntityModelFactory entityModelFactory;
    readonly IStageBounds stageBounds;
    readonly IPhysicsEntityManager physicsEntityManager;
    readonly IGameSettings settings;

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
        physicsEntityManager.AddEntity(entity);
        entity.OnDestroy += HandleEntityDestroyed;
        OnEntityCreated?.Invoke(entity);
    }

    void HandleEntityDestroyed (IEntityModel entity)
    {
        entity.OnDestroy -= HandleEntityDestroyed;
        physicsEntityManager.RemoveEntity(entity);
    }
}
