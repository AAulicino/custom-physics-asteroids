using UnityEngine;

public class StageBounds : IStageBounds
{
    public Rect Rect { get; set; }

    readonly IPhysicsUpdater physicsUpdater;
    readonly IViewUpdater viewUpdater;

    Rect localRect;

    public StageBounds (IPhysicsUpdater physicsUpdater, IViewUpdater viewUpdater)
    {
        this.viewUpdater = viewUpdater;
        this.physicsUpdater = physicsUpdater;
    }

    public void Initialize ()
    {
        viewUpdater.OnUpdate += RefreshLocalBounds;
        physicsUpdater.OnPreStep += RefreshPhysicsBounds;
    }

    void RefreshLocalBounds ()
    {
        Vector3 min = Camera.main.ScreenToWorldPoint(Vector2.zero);
        Vector3 max = Camera.main.ViewportToWorldPoint(Vector2.one);

        localRect = new Rect(min.x, min.y, max.x - min.x, max.y - min.y);
    }

    void RefreshPhysicsBounds ()
    {
        Rect = localRect;
    }

    public void Dispose ()
    {
        physicsUpdater.OnPreStep -= RefreshPhysicsBounds;
    }
}
