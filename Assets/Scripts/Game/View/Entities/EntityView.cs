using System;
using UnityEngine;

public abstract class EntityView : MonoBehaviour
{
    public event Action<EntityView> OnDestroy;

    [SerializeField] Vector2 boundsSize;

    bool ModelDestroyed => !model.IsAlive;

    protected IEntityModel model;
    IDebugSettings debugSettings;

    public virtual void Initialize (IEntityModel model, IDebugSettings debugSettings)
    {
        this.debugSettings = debugSettings;
        this.model = model;

        model.Collider.SetSize(boundsSize);
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

    void OnDrawGizmos ()
    {
        if (model is null || !debugSettings.RenderColliders)
            return;

        if (model.Collider is ICircleColliderModel circle)
        {
            DebugExtension.DrawCircle(
                circle.Bounds.center,
                Vector3.back,
                Color.green,
                circle.Radius
            );
        }
        else if (model.Collider is ISquareColliderModel rect)
        {
            DebugExtension.DrawRect(
                rect.Bounds,
                Color.green
            );
        }
    }

    void OnDrawGizmosSelected ()
    {
        Vector3 position = transform.position;
        Vector2 size = boundsSize * (model?.Collider.Scale ?? 1);

        DebugExtension.DrawRect(
            new Rect(position.x - size.x / 2, position.y - size.y / 2, size.x, size.y),
            Color.red
        );
    }
}
