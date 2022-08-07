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
        PhysicsRigidBody rigidBody = new(stageBounds, collider) { Position = position };
        rigidBody.SyncComponents();

        return new PlayerModel(rigidBody, collider, entityFactory);
    }

    public IAsteroidModel CreateAsteroid ()
    {
        PhysicsCollider collider = new(CollisionLayer.Asteroid);
        PhysicsRigidBody rigidBody = new(stageBounds, collider)
        {
            Position = stageBounds.RandomPointNearEdge()
        };
        rigidBody.SyncComponents();

        return new AsteroidModel(rigidBody, collider);
    }

    public IProjectileModel CreateProjectile (Vector3 position, float rotation, Vector3 velocity)
    {
        PhysicsCollider collider = new(CollisionLayer.Projectile);

        PhysicsRigidBody rigidBody = new(stageBounds, collider)
        {
            Position = position,
            Rotation = rotation,
            Velocity = velocity
        };
        rigidBody.SyncComponents();

        return new ProjectileModel(rigidBody, collider);
    }
}
