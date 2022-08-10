using System;

public interface IEntitiesViewManager : IDisposable
{
    void Initialize ();
    void CreateEntity (IEntityModel entity);
}
