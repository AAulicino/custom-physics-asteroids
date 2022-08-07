using UnityEngine;

public class EntityViewFactory : IEntityViewFactory
{
    public PlayerView CreatePlayer ()
    {
        return GameObject.Instantiate(Resources.Load<PlayerView>("Player"));
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
