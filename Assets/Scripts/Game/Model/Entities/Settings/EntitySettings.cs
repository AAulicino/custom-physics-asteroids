using System;
using UnityEngine;

[Serializable]
public class EntitySettings : IEntitySettings
{
    [field: SerializeField]
    public float MaxSpeed { get; private set; }

    [field: SerializeField]
    public bool WrapOnScreenEdge { get; private set; }

    [field: SerializeField]
    public float Drag { get; private set; }
}
