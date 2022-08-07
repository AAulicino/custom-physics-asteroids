using System;
using UnityEngine;

public interface IEntityFactory : IDisposable
{
    void CreateAsteroid (Vector3 position);
    void CreatePlayer (Vector3 position);
    void CreateProjectile (Vector3 position, float rotation, Vector3 velocity);
}
