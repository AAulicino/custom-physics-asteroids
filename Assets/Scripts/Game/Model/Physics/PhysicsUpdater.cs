using System;
using System.Diagnostics;
using System.Threading;
using Debug = UnityEngine.Debug;

public class PhysicsUpdater : IDisposable, IPhysicsUpdater
{
    public event Action OnPreStep;
    public event Action<float> OnStep;
    public event Action OnPostStep;

    readonly Stopwatch time = Stopwatch.StartNew();
    readonly ManualResetEvent pauseHandler = new(true);
    readonly IPhysicsSettings settings;

    Thread physicsThread;
    bool keepRunning;

    double currentTime;
    double accumulator;

    public PhysicsUpdater (IPhysicsSettings settings)
    {
        this.settings = settings;
    }

    public void Initialize ()
    {
        keepRunning = true;
        physicsThread = new Thread(PhysicsLoop)
        {
            Name = "Physics Thread"
        };
        physicsThread.Start();
    }

    public void Pause (bool pause)
    {
        if (pause)
        {
            pauseHandler.Reset();
            time.Stop();
        }
        else
        {
            time.Start();
            pauseHandler.Set();
        }
    }

    void PhysicsLoop ()
    {
        while (keepRunning)
        {
            pauseHandler.WaitOne();
            try
            {
                double newTime = time.Elapsed.TotalSeconds;
                double frameTime = newTime - currentTime;

                if (frameTime > settings.MaxTimeStep)
                    frameTime = settings.MaxTimeStep;

                currentTime = newTime;

                accumulator += frameTime;

                while (accumulator >= settings.TimeStep)
                {
                    double deltaTime = settings.TimeStep;

                    OnPreStep?.Invoke();
                    OnStep?.Invoke((float)deltaTime);
                    OnPostStep?.Invoke();

                    accumulator -= deltaTime;
                }
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }
        }
    }

    public void Dispose ()
    {
        keepRunning = false;
    }
}
