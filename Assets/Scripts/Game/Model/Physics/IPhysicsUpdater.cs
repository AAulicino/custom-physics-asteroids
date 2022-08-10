using System;

public interface IPhysicsUpdater : IDisposable
{
    event Action OnPreStep;
    event Action<float> OnStep;
    event Action OnPostStep;

    void Initialize ();
    void Pause (bool pause);
}
