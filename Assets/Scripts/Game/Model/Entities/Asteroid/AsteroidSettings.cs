using System;
using UnityEngine;

[Serializable]
public class AsteroidSettings : EntitySettings, IAsteroidSettings
{
    [field: SerializeField]
    public int StartingSize { get; private set; }

    [field: SerializeField]
    public int StartingCount { get; private set; }

    [field: SerializeField]
    public Vector2 MaximumStartingSpeed { get; private set; }
}
