using System;
using UnityEngine;

public abstract class EntityView : MonoBehaviour
{
    public event Action<EntityView> OnDestroy;

    [SerializeField] Vector2 bounds;

    protected IEntityModel model;
    bool ModelDestroyed => !model.IsAlive;

    public virtual void Initialize (IEntityModel model)
    {
        this.model = model;

        model.Collider.SetSize(bounds);
        transform.localScale = Vector3.one * model.Collider.Scale;

        model.OnReadyToReceiveInputs += OnPrePhysicsStep;
    }

    public void DeInitialize ()
    {
        model.OnReadyToReceiveInputs -= OnPrePhysicsStep;
    }

    public virtual void OnPrePhysicsStep ()
    {
    }

    public virtual void OnViewUpdate ()
    {
        if (ModelDestroyed)
        {
            OnDestroy?.Invoke(this);
            return;
        }

        transform.position = model.RigidBody.Position;
        transform.rotation = Quaternion.Euler(0, 0, model.RigidBody.Rotation);
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
        if (model != null)
            DebugExtension.DrawRect(model.Collider.Bounds, Color.green);
    }
}
