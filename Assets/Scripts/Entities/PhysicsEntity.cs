using System;

public class PhysicsEntity : IPhysicsEntity
{
    public int Id { get; }

    public IRigidBody RigidBody { get; }
    public ICollider Collider { get; }

    public PhysicsEntity (int id, IRigidBody rigidBody, ICollider collider)
    {
        Id = id;
        RigidBody = rigidBody;
        Collider = collider;
    }

    public void OnStep (float deltaTime)
    {
        RigidBody.Step(deltaTime);
    }

    public void OnPostStep ()
    {
        RigidBody.SyncComponents();
    }

    public void OnCollision (Collision collision)
    {
        RigidBody.ResolveCollision(collision);
    }
}
