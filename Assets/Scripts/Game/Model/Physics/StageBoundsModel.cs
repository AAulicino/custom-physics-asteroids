using UnityEngine;

public class StageBoundsModel : IStageBoundsModel
{
    public event System.Action OnReadyToReceiveInputs;

    public Rect Rect { get; private set; }

    readonly IPhysicsUpdater physicsUpdater;

    public StageBoundsModel (IPhysicsUpdater physicsUpdater)
    {
        this.physicsUpdater = physicsUpdater;
    }

    public void Initialize ()
    {
        physicsUpdater.OnPreStep += HandlePrePhysicsStep;
        HandlePrePhysicsStep();
    }

    public void UpdateRect (Rect rect) => Rect = rect;

    public Vector2 RandomPointNearEdge ()
        => Rect.center + Random.insideUnitCircle.normalized * Rect.size.magnitude / 2;

    void HandlePrePhysicsStep () => OnReadyToReceiveInputs?.Invoke();

    public void Dispose ()
    {
        physicsUpdater.OnPreStep -= HandlePrePhysicsStep;
    }

}
