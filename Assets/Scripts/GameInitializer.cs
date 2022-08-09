using UnityEngine;

public class GameInitializer : MonoBehaviour
{
    GameSessionModel gameSession;

    void Awake ()
    {
        gameSession = GameSessionFactory.Create();
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
