using System;
using UnityEngine;

public interface IStageBounds : IDisposable
{
    Rect Rect { get; }

    void Initialize ();
}
