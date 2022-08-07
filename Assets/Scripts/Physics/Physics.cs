using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Profiling;

public class Physics : IDisposable
{
    readonly IPhysicsUpdater updater;
    readonly CollisionDetector collisionDetector;

    Queue<PhysicsOperation> operationQueue = new Queue<PhysicsOperation>();

    HashSet<IPhysicsEntity> entities = new();

    public Physics (IPhysicsUpdater updater, CollisionDetector detector)
    {
        this.updater = updater;
        this.collisionDetector = detector;
    }

    public void Initialize ()
    {
        updater.Initialize();
        updater.OnPreStep += HandleOperationQueue;
        updater.OnStep += HandlePhysicsStep;
        updater.OnPostStep += HandlePostPhysicsStep;
    }

    public void AddEntity (IPhysicsEntity entity)
    {
        entities.Add(entity);
    }

    public void RemoveEntity (IPhysicsEntity entity)
    {
        entities.Remove(entity);
    }

    void HandleOperationQueue ()
    {
        while (operationQueue.Count > 0)
        {
            PhysicsOperation operation = operationQueue.Dequeue();

            if (operation.IsAdd)
                entities.Add(operation.Entity);
            else
                entities.Remove(operation.Entity);
        }
    }

    void HandlePhysicsStep (float step)
    {
        Profiler.BeginSample("Physics.Step");

        foreach (IPhysicsEntity entity in entities)
            entity.OnPhysicsStep(step);

        Profiler.BeginSample("Physics.Step.DetectCollisions");

        ICollection<Collision> collisions = collisionDetector.DetectCollisions(entities.ToArray());

        Profiler.EndSample();

        Profiler.BeginSample("Physics.Step.ResolveCollisions");

        foreach (Collision col in collisions)
            col.Self.OnCollide(col);

        Profiler.EndSample();

        Profiler.EndSample();
    }

    void HandlePostPhysicsStep ()
    {
        foreach (IPhysicsEntity entity in entities)
            entity.OnPostPhysicsStep();
    }

    public void Dispose ()
    {
        updater.OnPreStep -= HandleOperationQueue;
    }
}

public readonly struct PhysicsOperation
{
    public readonly bool IsAdd;
    public readonly IPhysicsEntity Entity;

    public PhysicsOperation (bool isAdd, IPhysicsEntity entity)
    {
        IsAdd = isAdd;
        Entity = entity;
    }
}
