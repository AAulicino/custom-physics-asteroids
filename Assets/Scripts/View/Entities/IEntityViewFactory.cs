public interface IEntityViewFactory
{
    AsteroidView CreateAsteroid ();
    PlayerView CreatePlayer (int playerId);
    ProjectileView CreateProjectile ();
}
