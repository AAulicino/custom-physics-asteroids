public interface IGameSettings
{
    float TimeStep { get; }

    IPlayerSettings PlayerSettings { get; }
    IAsteroidSettings AsteroidSettings { get; }
    IEntitySettings ProjectileSettings { get; }

    bool RenderCollisionQuadTree { get; }
}
