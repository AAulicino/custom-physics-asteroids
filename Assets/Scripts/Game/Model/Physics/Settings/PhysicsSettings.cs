using System;
using UnityEngine;

[Serializable]
public class PhysicsSettings : IPhysicsSettings
{
    [field: SerializeField]
    public float TimeStep { get; private set; }

    [field: SerializeField]
    public float MaxTimeStep { get; private set; }
}
