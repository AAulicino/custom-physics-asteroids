using UnityEngine;

public class GameInitializer : MonoBehaviour
{
    GameSession gameSession;

    void Awake ()
    {
        UnityUpdater viewUpdater = new GameObject("ViewUpdater").AddComponent<UnityUpdater>();
        GameSettings gameSettings = Resources.Load<GameSettings>("Settings/GameSettings");

        gameSession = new GameSession(viewUpdater, gameSettings);
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
