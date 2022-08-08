using System;

public interface IGameModelManager
{
    event Action<IEntityModel> OnEntityCreated;
    event Action<bool> OnGameEnd;

    bool GameEnded { get; }

    void Initialize ();
}
