using UnityEngine;

public class StageBoundsView : MonoBehaviour
{
    public Rect Rect { get; set; }

    new Camera camera;
    Rect localRect;
    IStageBoundsModel model;

    public void Setup (IStageBoundsModel model)
    {
        this.model = model;

        camera = Camera.main;
        RefreshLocalBounds();
        RefreshPhysicsBounds();

        model.OnReadyToReceiveInputs += RefreshPhysicsBounds;
    }

    public void Initialize ()
    {
    }

    void Update () => RefreshLocalBounds();

    void RefreshLocalBounds ()
    {
        Vector3 min = camera.ViewportToWorldPoint(Vector2.zero);
        Vector3 max = camera.ViewportToWorldPoint(Vector2.one);

        localRect = new Rect(min.x, min.y, max.x - min.x, max.y - min.y);
    }

    void RefreshPhysicsBounds ()
    {
        model.UpdateRect(localRect);
    }

    public void Dispose ()
    {
        Destroy(gameObject);
    }
}
