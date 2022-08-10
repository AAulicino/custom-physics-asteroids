using System;
using UnityEngine;

public class GameContextUIView : MonoBehaviour
{
    public event Action OnStartGameRequested;

    [SerializeField] MainMenuUIView mainMenuUIView;
    [SerializeField] GameOverUIView gameOverUIView;

    public void Initialize ()
    {
        mainMenuUIView.OnClickStartGame += HandleClickStartGame;
        gameOverUIView.OnClickClose += HandleGameOverClose;
        ShowMainMenu();
    }

    public void ShowMainMenu ()
    {
        mainMenuUIView.gameObject.SetActive(true);
    }

    public void ShowGameOver (bool victory)
    {
        gameOverUIView.Setup(victory);
        gameOverUIView.gameObject.SetActive(true);
    }

    void HandleGameOverClose ()
    {
        gameOverUIView.gameObject.SetActive(false);
        ShowMainMenu();
    }

    void HandleClickStartGame ()
    {
        mainMenuUIView.gameObject.SetActive(false);
        OnStartGameRequested?.Invoke();
    }
}
