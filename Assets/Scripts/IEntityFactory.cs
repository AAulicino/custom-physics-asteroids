using System;
using UnityEngine;

public interface IEntityFactory : IDisposable
{
    void CreatePlayer (int playerId, Vector3 position);
    void CreateAsteroid (int size, Vector3 position, Vector3 velocity);
    void CreateProjectile (Vector3 position, float rotation, Vector3 velocity);
}
