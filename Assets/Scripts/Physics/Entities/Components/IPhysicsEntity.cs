public interface IPhysicsEntity
{
    IRigidBody RigidBody { get; }
    ICollider Collider { get; }

    void OnPhysicsStep (float deltaTime);
    void OnPostPhysicsStep ();

    void OnCollide (Collision collision);

    void Destroy ();
}
