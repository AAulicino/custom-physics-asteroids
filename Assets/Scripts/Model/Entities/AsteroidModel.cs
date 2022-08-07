public class AsteroidModel : EntityModel, IAsteroidModel
{
    public AsteroidModel (IRigidBody rigidBody, ICollider collider) : base(rigidBody, collider)
    {
    }

    public override void OnCollide (Collision collision)
    {
        Destroy();
    }
}
