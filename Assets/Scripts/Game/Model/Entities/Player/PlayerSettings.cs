using System;
using UnityEngine;

[Serializable]
public class PlayerSettings : EntitySettings, IPlayerSettings
{
    [field: SerializeField]
    public float FireRate { get; private set; }
}
