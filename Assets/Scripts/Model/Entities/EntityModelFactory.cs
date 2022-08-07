using UnityEngine;

public class EntityModelFactory : IEntityModelFactory
{
    readonly IStageBounds stageBounds;
    readonly IGameSettings gameSettings;

    public EntityModelFactory (IStageBounds stageInfo, IGameSettings gameSettings)
    {
        this.gameSettings = gameSettings;
        this.stageBounds = stageInfo;
    }

    public IPlayerModel CreatePlayer (int playerId, Vector3 position, IEntityFactory entityFactory)
    {
        ColliderModel collider = new(CollisionLayer.Player);
        RigidBodyModel rigidBody = new(gameSettings.PlayerSettings, stageBounds, collider)
        {
            Position = position
        };
        rigidBody.SyncComponents();

        return new PlayerModel(
            playerId,
            gameSettings.PlayerSettings,
            rigidBody,
            collider,
            entityFactory
        );
    }

    public IAsteroidModel CreateAsteroid (
        int size,
        Vector3 Position,
        Vector3 velocity,
        IEntityFactory entityFactory
    )
    {
        ColliderModel collider = new(CollisionLayer.Asteroid);
        RigidBodyModel rigidBody = new(gameSettings.AsteroidSettings, stageBounds, collider)
        {
            Position = Position,
            Velocity = velocity
        };
        rigidBody.SyncComponents();

        return new AsteroidModel(size, rigidBody, collider, entityFactory);
    }

    public IProjectileModel CreateProjectile (Vector3 position, float rotation, Vector3 velocity)
    {
        ColliderModel collider = new(CollisionLayer.Projectile);

        RigidBodyModel rigidBody = new(gameSettings.ProjectileSettings, stageBounds, collider)
        {
            Position = position,
            Rotation = rotation,
            Velocity = new Vector3(0, 0, rotation)
        };
        rigidBody.SyncComponents();

        return new ProjectileModel(rigidBody, collider);
    }
}
