using System;
using UnityEngine;

public class ViewUpdater : MonoBehaviour, IViewUpdater
{
    public event Action OnUpdate;

    void Update () => OnUpdate?.Invoke();
}
