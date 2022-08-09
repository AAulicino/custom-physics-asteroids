public interface IGameSettings
{
    IPhysicsSettings PhysicsSettings { get; }

    IPlayerSettings PlayerSettings { get; }
    IAsteroidSettings AsteroidSettings { get; }
    IEntitySettings ProjectileSettings { get; }

    IDebugSettings DebugSettings { get; }
}
