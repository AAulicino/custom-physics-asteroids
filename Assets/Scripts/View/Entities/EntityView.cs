using UnityEngine;

public abstract class EntityView : MonoBehaviour
{
    [SerializeField] Vector2 bounds;

    protected IPhysicsEntity physicsEntity;

    public virtual void Initialize (IPhysicsEntity physicsEntity)
    {
        this.physicsEntity = physicsEntity;
        physicsEntity.Collider.SetSize(bounds);
    }

    public virtual void Sync ()
    {
        transform.position = physicsEntity.RigidBody.Position;
        transform.rotation = Quaternion.Euler(0, 0, physicsEntity.RigidBody.Rotation);
    }

    void OnDrawGizmosSelected ()
    {
        DebugExtension.DrawRect(
            new Rect(
                transform.position.x - bounds.x / 2,
                transform.position.y - bounds.y / 2,
                bounds.x,
                bounds.y
            ),
            Color.red
        );
        if (physicsEntity != null)
            DebugExtension.DrawRect(physicsEntity.Collider.Bounds, Color.green);
    }
}
