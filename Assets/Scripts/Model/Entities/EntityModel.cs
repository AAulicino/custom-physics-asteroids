using System;

public abstract class EntityModel : IEntityModel
{
    public event Action<Collision> OnCollision;
    public event Action<IEntityModel> OnDestroy;
    public event Action OnReadyToReceiveInputs;

    public IRigidBody RigidBody { get; }
    public ICollider Collider { get; }

    bool destroyed;

    public EntityModel (
        IRigidBody rigidBody,
        ICollider collider
    )
    {
        RigidBody = rigidBody;
        Collider = collider;

        RigidBody.OnOutOfBounds += Destroy;
    }

    public void OnPrePhysicsStep ()
    {
        OnReadyToReceiveInputs?.Invoke();
    }

    public void OnPhysicsStep (float deltaTime)
    {
        RigidBody.Step(deltaTime);
    }

    public void OnPostPhysicsStep ()
    {
        RigidBody.SyncComponents();
        if (destroyed)
            OnDestroy?.Invoke(this);
    }

    public virtual void OnCollide (Collision collision)
    {
    }

    public void Destroy ()
    {
        destroyed = true;
    }
}
