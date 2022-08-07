using System;
using System.Collections.Generic;
using System.Linq;

public class Physics : IDisposable
{
    readonly IPhysicsUpdater updater;
    readonly CollisionDetector collisionDetector;
    readonly HashSet<IEntityModel> entities = new();

    readonly Queue<AddOrRemoveOperation<IEntityModel>> operationQueue = new();

    public Physics (IPhysicsUpdater updater, CollisionDetector detector)
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
        entities.Add(entity);
    }

    public void RemoveEntity (IEntityModel entity)
    {
        entities.Remove(entity);
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

        ICollection<Collision> collisions = collisionDetector.DetectCollisions(entities.ToArray());

        foreach (Collision col in collisions)
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
