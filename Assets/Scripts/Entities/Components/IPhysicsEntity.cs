public interface IPhysicsEntity
{
    int Id { get; }
    IRigidBody RigidBody { get; }
    ICollider Collider { get; }

    void OnStep (float deltaTime);
    void OnPostStep ();

    void OnCollision (Collision collision);
}
