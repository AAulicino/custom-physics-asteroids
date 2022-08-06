using System;
using System.Collections.Generic;
using System.Linq;

public class Physics : IDisposable
{
    readonly IPhysicsUpdater updater;
    private readonly CollisionDetector detector;
    Queue<PhysicsOperation> operationQueue = new Queue<PhysicsOperation>();

    HashSet<IPhysicsEntity> entities = new();

    public Physics (IPhysicsUpdater updater, CollisionDetector detector)
    {
        this.updater = updater;
        this.detector = detector;
    }

    public void Initialize ()
    {
        updater.Initialize();
        updater.OnPreStep += HandleOperationQueue;
        updater.OnStep += HandlePhysicsStep;
        updater.OnPostStep += HandlePostPhysicsStep;
    }
    static int id;

    public IPhysicsEntity CreateEntity ()
    {
        PhysicsCollider collider = new PhysicsCollider(id++, new UnityEngine.Rect(0, 0, .1f, .1f), -1);
        PhysicsRigidBody rigidBody = new PhysicsRigidBody(collider);

        var entity = new PhysicsEntity(id, rigidBody, collider);
        entities.Add(entity);
        return entity;
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
        foreach (IPhysicsEntity entity in entities)
            entity.OnStep(step);

        var collisions = detector.DetectCollisions(entities.ToArray());

        foreach (var col in collisions)
            col.Self.OnCollision(col);
    }

    void HandlePostPhysicsStep ()
    {
        foreach (IPhysicsEntity entity in entities)
            entity.OnPostStep();
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
