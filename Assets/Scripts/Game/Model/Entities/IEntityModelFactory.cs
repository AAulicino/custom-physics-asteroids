using System;
using UnityEngine;

public interface IEntityModelFactory
{
    event Action<IEntityModel> OnEntityCreated;

    IPlayerModel CreatePlayer (int playerId, Vector3 position);

    IAsteroidModel CreateAsteroid (
        int size,
        Vector3 position,
        Vector3 velocity
    );

    IProjectileModel CreateProjectile (Vector3 position, float rotation, Vector3 velocity);
}
