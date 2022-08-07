using System;

public interface IEntityModel
{
    event Action<IEntityModel> OnDestroy;
    event Action OnReadyToReceiveInputs;

    IRigidBody RigidBody { get; }
    ICollider Collider { get; }

    void OnPrePhysicsStep ();
    void OnPhysicsStep (float deltaTime);
    void OnPostPhysicsStep ();

    void OnCollide (Collision collision);

    void Destroy ();
}
