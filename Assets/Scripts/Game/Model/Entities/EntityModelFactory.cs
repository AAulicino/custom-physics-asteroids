using System;
using UnityEngine;

public class EntityModelFactory : IEntityModelFactory
{
    public event Action<IEntityModel> OnEntityCreated;

    readonly IStageBoundsModel stageBounds;
    readonly IGameSettings gameSettings;

    public EntityModelFactory (IStageBoundsModel stageBounds, IGameSettings gameSettings)
    {
        this.gameSettings = gameSettings;
        this.stageBounds = stageBounds;
    }

    public IPlayerModel CreatePlayer (int playerId, Vector3 position)
    {
        SquareColliderModel collider = new(CollisionLayer.Player);
        RigidBodyModel rigidBody = new(gameSettings.PlayerSettings, stageBounds, collider)
        {
            Position = position
        };
        rigidBody.SyncComponents();

        PlayerModel player = new PlayerModel(
            playerId,
            gameSettings.PlayerSettings,
            rigidBody,
            collider,
            this
        );
        OnEntityCreated(player);
        return player;
    }

    public IAsteroidModel CreateAsteroid (
        int size,
        Vector3 Position,
        Vector3 velocity
    )
    {
        CircleColliderModel collider = new(CollisionLayer.Asteroid);
        RigidBodyModel rigidBody = new(gameSettings.AsteroidSettings, stageBounds, collider)
        {
            Position = Position,
            Velocity = velocity
        };
        rigidBody.SyncComponents();

        AsteroidModel asteroid = new AsteroidModel(size, rigidBody, collider, this);
        OnEntityCreated(asteroid);
        return asteroid;
    }

    public IProjectileModel CreateProjectile (Vector3 position, float rotation, Vector3 velocity)
    {
        CircleColliderModel collider = new(CollisionLayer.Projectile);

        RigidBodyModel rigidBody = new(gameSettings.ProjectileSettings, stageBounds, collider)
        {
            Position = position,
            Rotation = rotation,
            Velocity = new Vector3(0, 0, rotation)
        };
        rigidBody.SyncComponents();

        ProjectileModel projectile = new ProjectileModel(rigidBody, collider);
        OnEntityCreated(projectile);
        return projectile;
    }
}
