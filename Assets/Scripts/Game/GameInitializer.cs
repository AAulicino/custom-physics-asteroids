using UnityEngine;

public class GameInitializer : MonoBehaviour
{
    GameSession gameSession;

    void Awake ()
    {
        IUnityUpdater viewUpdater = new GameObject("ViewUpdater").AddComponent<UnityUpdater>();
        IGameSettings gameSettings = Resources.Load<GameSettings>("Settings/GameSettings");

        gameSession = new GameSession(
            new PhysicsUpdater(gameSettings.PhysicsSettings),
            viewUpdater,
            gameSettings
        );
        gameSession.Initialize();
        ListenToEditorPause();
    }

    void OnApplicationPause (bool pauseStatus)
    {
        gameSession.Pause(pauseStatus || UnityEditor.EditorApplication.isPaused);
    }

    void OnDestroy ()
    {
        gameSession.Dispose();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.pauseStateChanged -= HandlePauseStateChanged;
#endif
    }

    void ListenToEditorPause ()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.pauseStateChanged += HandlePauseStateChanged;
    }

    void HandlePauseStateChanged (UnityEditor.PauseState state)
    {
        OnApplicationPause(state == UnityEditor.PauseState.Paused);
#endif
    }
}
