using UnityEngine;

public class ProjectileModel : EntityModel, IProjectileModel
{
    const float projectileSpeed = 10;

    public ProjectileModel (
        IRigidBody rigidBody,
        ICollider collider
    ) : base(rigidBody, collider)
    {
        SetInitialSpeed();
    }

    void SetInitialSpeed ()
    {
        float radians = RigidBody.Rotation * Mathf.Deg2Rad;
        Vector2 direction = new Vector2(-Mathf.Sin(radians), Mathf.Cos(radians));

        RigidBody.Velocity = direction * projectileSpeed;
    }

    public override void OnCollide (Collision collision)
    {
        Destroy();
    }
}
