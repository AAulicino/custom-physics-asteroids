using UnityEngine;

public class EntityFactory : IEntityFactory
{
    readonly IEntityModelFactory modelFactory;
    readonly IEntityViewFactory viewFactory;
    readonly Physics physics;
    readonly IEntitiesViewManager viewManager;

    public EntityFactory (
        IEntityModelFactory modelFactory,
        IEntityViewFactory viewFactory,
        Physics physics,
        IEntitiesViewManager viewManager
    )
    {
        this.modelFactory = modelFactory;
        this.viewFactory = viewFactory;
        this.physics = physics;
        this.viewManager = viewManager;
    }

    public void CreatePlayer (Vector3 position)
    {
        IPlayerModel model = modelFactory.CreatePlayer(position, this);
        PlayerView view = viewFactory.CreatePlayer();
        CreateEntity(model, view);
    }

    public void CreateAsteroid (Vector3 position)
    {
        IAsteroidModel model = modelFactory.CreateAsteroid();
        AsteroidView view = viewFactory.CreateAsteroid();
        CreateEntity(model, view);
    }

    public void CreateProjectile (Vector3 position, float rotation, Vector3 velocity)
    {
        IProjectileModel model = modelFactory.CreateProjectile(position, rotation, velocity);
        ProjectileView view = viewFactory.CreateProjectile();
        CreateEntity(model, view);
    }

    void CreateEntity (IEntityModel model, EntityView view)
    {
        physics.AddEntity(model);
        view.Initialize(model);
        viewManager.AddEntity(view);
    }
}
