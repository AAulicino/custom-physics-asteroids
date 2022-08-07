using UnityEngine;

public class PhysicsEntityFactory : IPhysicsEntityFactory
{
    readonly IStageBounds stageInfo;

    public PhysicsEntityFactory (IStageBounds stageInfo)
    {
        this.stageInfo = stageInfo;
    }

    public IPhysicsEntity Create ()
    {
        PhysicsCollider collider = new PhysicsCollider(
            new Rect(0, 0, 1, 1f),
            -1
        );
        PhysicsRigidBody rigidBody = new PhysicsRigidBody(stageInfo, collider);

        return new PhysicsEntity(rigidBody, collider);
    }
}
