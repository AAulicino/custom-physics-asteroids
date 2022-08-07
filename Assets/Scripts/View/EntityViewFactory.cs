using UnityEngine;

public class EntityViewFactory : IEntityViewFactory
{
    public PlayerView CreatePlayer (int playerId)
    {
        return GameObject.Instantiate(Resources.Load<PlayerView>("Player" + playerId));
    }

    public AsteroidView CreateAsteroid ()
    {
        return GameObject.Instantiate(Resources.Load<AsteroidView>("Asteroid"));
    }

    public ProjectileView CreateProjectile ()
    {
        return GameObject.Instantiate(Resources.Load<ProjectileView>("Projectile"));
    }
}
