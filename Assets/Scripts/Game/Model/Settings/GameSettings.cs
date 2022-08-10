using UnityEngine;

[CreateAssetMenu(fileName = "GameSettings", menuName = "GameSettings")]
public class GameSettings : ScriptableObject, IGameSettings
{
    [SerializeField] PhysicsSettings physicsSettings;

    [field: Space(24)]
    [SerializeField] PlayerSettings playerSettings;
    [SerializeField] EntitySettings projectileSettings;
    [SerializeField] AsteroidSettings asteroidSettings;

    [field: Space(24)]
    [SerializeField] DebugSettings debugSettings;

    public IPlayerSettings PlayerSettings => playerSettings;
    public IAsteroidSettings AsteroidSettings => asteroidSettings;
    public IEntitySettings ProjectileSettings => projectileSettings;
    public IDebugSettings DebugSettings => debugSettings;
    public IPhysicsSettings PhysicsSettings => physicsSettings;
}
