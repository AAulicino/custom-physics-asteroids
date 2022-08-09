using System;
using UnityEngine;

[Serializable]
public class DebugSettings : IDebugSettings
{
    [field: SerializeField]
    public bool RenderCollisionQuadTree { get; private set; }

    [field: SerializeField]
    public bool RenderColliders { get; private set; }
}
