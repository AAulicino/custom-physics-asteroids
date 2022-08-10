using UnityEngine;

public interface IAsteroidSettings : IEntitySettings
{
    int StartingCount { get; }
    int StartingSize { get; }

    Vector2 MaximumStartingSpeed { get; }
}
