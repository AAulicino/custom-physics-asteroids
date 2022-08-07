using System;
using UnityEngine;

[Serializable]
public class AsteroidSettings : EntitySettings, IAsteroidSettings
{
    [field: SerializeField]
    public int StartingSize { get; private set; }
}
