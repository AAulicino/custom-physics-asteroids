public interface IGameSettings
{
    IPlayerSettings PlayerSettings { get; }
    IAsteroidSettings AsteroidSettings { get; }
    IEntitySettings ProjectileSettings { get; }
}
