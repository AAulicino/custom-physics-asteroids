using System;
using System.Diagnostics;
using System.Threading;
using Debug = UnityEngine.Debug;

public class PhysicsUpdater : IDisposable, IPhysicsUpdater
{
    const float MAX_DELTA = 0.3f;
    const float FIXED_TIME_STEP = 0.02f;

    public event Action OnPreStep;
    public event Action<float> OnStep;
    public event Action OnPostStep;

    readonly Stopwatch time = Stopwatch.StartNew();

    Thread physicsThread;
    bool keepRunning;

    double currentTime;
    double accumulator;

    public void Initialize()
    {
        keepRunning = true;
        physicsThread = new Thread(PhysicsLoop)
        {
            Name = "Physics Thread"
        };
        physicsThread.Start();
    }

    void PhysicsLoop()
    {
        while (keepRunning)
        {
            try
            {
                double newTime = time.Elapsed.TotalSeconds;
                double frameTime = newTime - currentTime;

                if (frameTime > MAX_DELTA)
                    frameTime = MAX_DELTA;

                currentTime = newTime;

                accumulator += frameTime;

                while (accumulator >= FIXED_TIME_STEP)
                {
                    double deltaTime = FIXED_TIME_STEP;

                    OnPreStep?.Invoke();
                    OnStep?.Invoke((float)deltaTime);
                    OnPostStep?.Invoke();

                    accumulator -= deltaTime;
                }

                // float alpha = accumulator / (FIXED_TIME_STEP );

                // foreach (var body in bodies)
                // {
                //     body.Interpolate(ref alpha);
                // }
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }
        }
    }

    public void Dispose()
    {
        keepRunning = false;
    }
}
