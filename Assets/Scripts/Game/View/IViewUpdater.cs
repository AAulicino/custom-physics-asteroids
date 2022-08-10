using System;

public interface IUnityUpdater
{
    event Action OnUpdate;

    void Schedule (Action action);
}
