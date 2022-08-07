using UnityEngine;

[CreateAssetMenu(fileName = "GameSettings", menuName = "GameSettings")]
public class GameSettings : ScriptableObject, IGameSettings
{
    [SerializeField] PlayerSettings playerSettings;
    [SerializeField] AsteroidSettings asteroidSettings;
    [SerializeField] EntitySettings projectileSettings;

    public IPlayerSettings PlayerSettings => playerSettings;
    public IAsteroidSettings AsteroidSettings => asteroidSettings;
    public IEntitySettings ProjectileSettings => projectileSettings;
}
