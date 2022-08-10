using System;
using UnityEngine;
using UnityEngine.Pool;
using Object = UnityEngine.Object;

public class EntityViewFactory : IEntityViewFactory, IDisposable
{
    ObjectPool<ProjectileView> projectilePool;
    ObjectPool<AsteroidView> asteroidPool;
    Transform poolRoot;

    public EntityViewFactory ()
    {
        InitializePools();
    }

    public PlayerView CreatePlayer (int playerId)
    {
        PlayerView player = Object.Instantiate(
            Resources.Load<PlayerView>("Entities/Player" + playerId)
        );
        player.OnDestroy += HandleOnDestroy;
        return player;
    }

    public AsteroidView CreateAsteroid () => asteroidPool.Get();

    public ProjectileView CreateProjectile () => projectilePool.Get();

    void InitializePools ()
    {
        poolRoot = new GameObject("EntityViewFactoryPool").transform;

        projectilePool = new ObjectPool<ProjectileView>(
            HandleProjectileOnCreate,
            HandleOnGet,
            HandleOnRelease,
            HandleOnDestroy
        );

        asteroidPool = new ObjectPool<AsteroidView>(
            HandleAsteroidOnCreate,
            HandleOnGet,
            HandleOnRelease,
            HandleOnDestroy
        );
    }

    AsteroidView HandleAsteroidOnCreate ()
    {
        AsteroidView view = Object.Instantiate(
            Resources.Load<AsteroidView>("Entities/Asteroid")
        );
        view.transform.SetParent(poolRoot, false);
        view.OnDestroy += x => asteroidPool.Release(view);
        return view;
    }

    ProjectileView HandleProjectileOnCreate ()
    {
        ProjectileView view = Object.Instantiate(
            Resources.Load<ProjectileView>("Entities/Projectile")
        );
        view.transform.SetParent(poolRoot, false);
        view.OnDestroy += x => projectilePool.Release(view);
        return view;
    }

    void HandleOnGet (EntityView view)
    {
        view.gameObject.SetActive(true);
        view.transform.SetParent(null);
    }

    void HandleOnRelease (EntityView view)
    {
        view.DeInitialize();
        view.gameObject.SetActive(false);
        view.transform.SetParent(poolRoot, false);
    }

    void HandleOnDestroy (EntityView view)
    {
        view.DeInitialize();
        Object.Destroy(view.gameObject);
    }

    public void Dispose ()
    {
        projectilePool.Dispose();
    }
}
