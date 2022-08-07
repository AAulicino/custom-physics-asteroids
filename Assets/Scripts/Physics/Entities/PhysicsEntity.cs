using System;

public class PhysicsEntity : IPhysicsEntity
{
    public event Action<Collision> OnCollision;
    public event Action OnDestroy;

    public IRigidBody RigidBody { get; }
    public ICollider Collider { get; }

    public PhysicsEntity (IRigidBody rigidBody, ICollider collider)
    {
        RigidBody = rigidBody;
        Collider = collider;
    }

    public void OnPhysicsStep (float deltaTime)
    {
        RigidBody.Step(deltaTime);
    }

    public void OnPostPhysicsStep ()
    {
        RigidBody.SyncComponents();
    }

    public void OnCollide (Collision collision)
    {
        OnCollision?.Invoke(collision);
    }

    public void Destroy ()
    {
        OnDestroy?.Invoke();
    }
}
