using System;

public interface IEntityModel
{
    event Action<IEntityModel> OnDestroy;
    event Action OnReadyToReceiveInputs;

    IRigidBodyModel RigidBody { get; }
    IColliderModel Collider { get; }

    void OnPrePhysicsStep ();
    void OnPhysicsStep (float deltaTime);
    void OnPostPhysicsStep ();

    void OnCollide (Collision collision);

    void Destroy ();
}
