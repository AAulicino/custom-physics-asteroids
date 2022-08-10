using System;
using System.Collections.Generic;
using UnityEngine;

public class UnityUpdater : MonoBehaviour, IUnityUpdater
{
    public event Action OnUpdate;

    readonly Queue<Action> actionQueue = new();

    void Update ()
    {
        lock (actionQueue)
        {
            while (actionQueue.Count > 0)
                actionQueue.Dequeue()();
        }

        OnUpdate?.Invoke();
    }

    public void Schedule (Action action)
    {
        lock(actionQueue)
            actionQueue.Enqueue(action);
    }
}
