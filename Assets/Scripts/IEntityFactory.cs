using UnityEngine;

public interface IEntityFactory
{
    void CreateAsteroid (Vector3 position);
    void CreatePlayer (Vector3 position);
    void CreateProjectile (Vector3 position, float rotation, Vector3 velocity);
}
