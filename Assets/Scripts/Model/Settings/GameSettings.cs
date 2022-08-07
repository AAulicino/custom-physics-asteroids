using UnityEngine;

[CreateAssetMenu(fileName = "GameSettings", menuName = "GameSettings")]
public class GameSettings : ScriptableObject, IGameSettings
{
    [field: Header("Physics")]
    public float TimeStep { get; } = 0.2f;

    [Header("Entities")]
    [SerializeField] PlayerSettings playerSettings;
    [SerializeField] AsteroidSettings asteroidSettings;
    [SerializeField] EntitySettings projectileSettings;

    [field: Header("Debug")]
    [field: SerializeField]
    public bool RenderCollisionQuadTree { get; private set; }

    public IPlayerSettings PlayerSettings => playerSettings;
    public IAsteroidSettings AsteroidSettings => asteroidSettings;
    public IEntitySettings ProjectileSettings => projectileSettings;
}
