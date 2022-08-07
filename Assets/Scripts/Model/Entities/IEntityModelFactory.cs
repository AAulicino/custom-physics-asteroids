using UnityEngine;

public interface IEntityModelFactory
{
    IPlayerModel CreatePlayer (int playerId, Vector3 position, IEntityFactory entityFactory);

    IAsteroidModel CreateAsteroid (
        int size,
        Vector3 position,
        Vector3 velocity,
        IEntityFactory entityFactory
    );

    IProjectileModel CreateProjectile (Vector3 position, float rotation, Vector3 velocity);
}
