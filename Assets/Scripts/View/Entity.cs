using UnityEngine;

public class Entity : MonoBehaviour
{
    public IPhysicsEntity physicsEntity;

    public void Initialize (IPhysicsEntity physicsEntity)
    {
        this.physicsEntity = physicsEntity;
    }

    void Update () => Sync();

    public void Sync ()
    {
        transform.position = physicsEntity.RigidBody.Position;
        transform.rotation = Quaternion.Euler(0, 0, physicsEntity.RigidBody.Rotation);
    }

    void OnDrawGizmosSelected ()
    {
        DebugExtension.DrawRect(physicsEntity.Collider.Bounds, Color.red);
    }
}
