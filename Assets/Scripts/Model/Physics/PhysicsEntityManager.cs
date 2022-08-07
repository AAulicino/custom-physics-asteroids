using System;
using System.Collections.Generic;
using System.Linq;

public class PhysicsEntityManager : IDisposable, IPhysicsEntityManager
{
    readonly IPhysicsUpdater updater;
    readonly CollisionDetector collisionDetector;
    readonly HashSet<IEntityModel> entities = new();

    readonly Queue<AddOrRemoveOperation<IEntityModel>> operationQueue = new();
    readonly List<Collision> collisionsBuffer = new();

    public PhysicsEntityManager (IPhysicsUpdater updater, CollisionDetector detector)
    {
        this.updater = updater;
        this.collisionDetector = detector;
    }

    public void Initialize ()
    {
        updater.OnPreStep += HandlePrePhysicsStep;
        updater.OnStep += HandlePhysicsStep;
        updater.OnPostStep += HandlePostPhysicsStep;
        updater.Initialize();
    }

    public void AddEntity (IEntityModel entity)
    {
        operationQueue.Enqueue(new AddOrRemoveOperation<IEntityModel>(true, entity));
    }

    public void RemoveEntity (IEntityModel entity)
    {
        operationQueue.Enqueue(new AddOrRemoveOperation<IEntityModel>(false, entity));
    }

    void HandlePrePhysicsStep ()
    {
        HandleOperationQueue();

        foreach (IEntityModel entity in entities)
            entity.OnPrePhysicsStep();
    }

    void HandlePhysicsStep (float step)
    {
        foreach (IEntityModel entity in entities)
            entity.OnPhysicsStep(step);

        collisionDetector.DetectCollisions(entities.ToArray(), collisionsBuffer);

        foreach (Collision col in collisionsBuffer)
            col.Self.OnCollide(col);
    }

    void HandlePostPhysicsStep ()
    {
        foreach (IEntityModel entity in entities)
            entity.OnPostPhysicsStep();
    }

    void HandleOperationQueue ()
    {
        while (operationQueue.Count > 0)
        {
            AddOrRemoveOperation<IEntityModel> operation = operationQueue.Dequeue();

            if (operation.IsAdd)
                entities.Add(operation.Value);
            else
                entities.Remove(operation.Value);
        }
    }

    public void Dispose ()
    {
        updater.OnPreStep -= HandlePrePhysicsStep;
        updater.OnStep -= HandlePhysicsStep;
        updater.OnPostStep -= HandlePostPhysicsStep;
    }
}
