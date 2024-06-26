using UnityEngine;
using Random = System.Random;

public class AsteroidModel : EntityModel, IAsteroidModel
{
    readonly IEntityModelFactory entityFactory;
    readonly Random random = new();

    readonly int size;

    public AsteroidModel (
        int size,
        IRigidBodyModel rigidBody,
        IColliderModel collider,
        IEntityModelFactory entityFactory
    ) : base(rigidBody, collider)
    {
        this.entityFactory = entityFactory;
        this.size = size;

        SetRandomAngularVelocity();
        SetSize();
    }

    void SetRandomAngularVelocity ()
    {
        RigidBody.AngularVelocity = random.Range(-200, 200);
    }

    void SetSize ()
    {
        Collider.Scale = size;
    }

    public override void OnCollide (Collision collision)
    {
        Destroy();

        if (size == 1)
            return;

        CreateAsteroid();
        CreateAsteroid();
    }

    void CreateAsteroid ()
    {
        Vector2 velocity = new Vector2(
            random.Range(-RigidBody.MaxSpeed, RigidBody.MaxSpeed),
            random.Range(-RigidBody.MaxSpeed, RigidBody.MaxSpeed)
        );

        velocity = velocity.ClampMagnitude(1, RigidBody.MaxSpeed);

        entityFactory.CreateAsteroid(
            size - 1,
            RigidBody.Position,
            velocity
        );
    }
}
