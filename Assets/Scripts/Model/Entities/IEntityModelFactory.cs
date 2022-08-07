using UnityEngine;

public interface IEntityModelFactory
{
    IPlayerModel CreatePlayer (Vector3 position, IEntityFactory entityFactory);
    IAsteroidModel CreateAsteroid ();
    IProjectileModel CreateProjectile (Vector3 position, float rotation, Vector3 velocity);
}
