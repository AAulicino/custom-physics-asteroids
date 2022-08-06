using System;

public interface IViewUpdater
{
    event Action OnUpdate;
}
