public class ProjectileModel : EntityModel, IProjectileModel
{
    public ProjectileModel (
        IRigidBody rigidBody,
        ICollider collider
    ) : base(rigidBody, collider)
    {
    }

    public override void OnCollide (Collision collision)
    {
        Destroy();
    }
}
