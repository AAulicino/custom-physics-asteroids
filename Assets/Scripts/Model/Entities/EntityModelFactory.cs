using UnityEngine;

public class EntityModelFactory : IEntityModelFactory
{
    readonly IStageBounds stageBounds;

    public EntityModelFactory (IStageBounds stageInfo)
    {
        this.stageBounds = stageInfo;
    }

    public IPlayerModel CreatePlayer (Vector3 position, IEntityFactory entityFactory)
    {
        PhysicsCollider collider = new(CollisionLayer.Player);
        PhysicsRigidBody rigidBody = new(stageBounds, collider, true) { Position = position };
        rigidBody.SyncComponents();

        return new PlayerModel(rigidBody, collider, entityFactory);
    }

    public IAsteroidModel CreateAsteroid ()
    {
        PhysicsCollider collider = new(CollisionLayer.Asteroid);
        PhysicsRigidBody rigidBody = new(stageBounds, collider, true)
        {
            Position = stageBounds.RandomPointNearEdge(),

        };
        rigidBody.SyncComponents();

        return new AsteroidModel(rigidBody, collider);
    }

    public IProjectileModel CreateProjectile (Vector3 position, float rotation, Vector3 velocity)
    {
        PhysicsCollider collider = new(CollisionLayer.Projectile);

        PhysicsRigidBody rigidBody = new(stageBounds, collider, false)
        {
            Position = position,
            Rotation = rotation,
            Velocity = new Vector3(0, 0, rotation)
        };
        rigidBody.SyncComponents();

        return new ProjectileModel(rigidBody, collider);
    }
}
