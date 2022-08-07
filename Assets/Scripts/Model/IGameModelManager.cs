using System;

public interface IGameModelManager
{
    event Action<IEntityModel> OnEntityCreated;

    void Initialize ();
}
