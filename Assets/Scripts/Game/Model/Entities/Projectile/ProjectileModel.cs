using UnityEngine;

public class ProjectileModel : EntityModel, IProjectileModel
{
    public ProjectileModel (
        IRigidBodyModel rigidBody,
        IColliderModel collider
    ) : base(rigidBody, collider)
    {
        SetInitialSpeed();
    }

    void SetInitialSpeed ()
    {
        float radians = RigidBody.Rotation * Mathf.Deg2Rad;
        Vector2 direction = new Vector2(-Mathf.Sin(radians), Mathf.Cos(radians));

        RigidBody.Velocity = direction * RigidBody.MaxSpeed;
    }

    public override void OnCollide (Collision collision)
    {
        Destroy();
    }
}
