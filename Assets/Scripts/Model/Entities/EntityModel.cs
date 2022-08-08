using System;

public abstract class EntityModel : IEntityModel
{
    public event Action<IEntityModel> OnDestroy;
    public event Action OnReadyToReceiveInputs;

    public IRigidBodyModel RigidBody { get; }
    public IColliderModel Collider { get; }
    public bool IsAlive => !destroyed;

    bool destroyed;

    public EntityModel (
        IRigidBodyModel rigidBody,
        IColliderModel collider
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

    public virtual void OnPhysicsStep (float deltaTime)
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
        Destroy();
    }

    public void Destroy ()
    {
        destroyed = true;
    }
}
