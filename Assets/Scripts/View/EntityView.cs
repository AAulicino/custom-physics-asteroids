using UnityEngine;

public class EntityView : MonoBehaviour
{
    protected IPhysicsEntity physicsEntity;

    public void Initialize (IPhysicsEntity physicsEntity)
    {
        this.physicsEntity = physicsEntity;
    }

    public virtual void Sync ()
    {
        transform.position = physicsEntity.RigidBody.Position;
        transform.rotation = Quaternion.Euler(0, 0, physicsEntity.RigidBody.Rotation);
    }

    void OnDrawGizmosSelected ()
    {
        if (physicsEntity != null)
            DebugExtension.DrawRect(physicsEntity.Collider.Bounds, Color.red);
    }
}
