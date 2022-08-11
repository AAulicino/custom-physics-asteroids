using System;
using UnityEngine;

public interface IStageBoundsModel : IDisposable
{
    event Action OnReadyToReceiveInputs;

    Rect Rect { get; }

    void Initialize ();

    Vector2 RandomPointNearEdge ();
    void UpdateRect (Rect rect);
}
