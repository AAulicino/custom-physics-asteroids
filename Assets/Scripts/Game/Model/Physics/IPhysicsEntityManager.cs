using System;

public interface IPhysicsEntityManager : IDisposable
{
    void AddEntity (IEntityModel entity);
    void Initialize ();
    void RemoveEntity (IEntityModel entity);
}
