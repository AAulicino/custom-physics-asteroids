using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOverUIView : MonoBehaviour
{
    public event Action OnClickClose;

    [SerializeField] TextMeshProUGUI gameOverText;
    [SerializeField] Button closeButton;

    void Awake ()
    {
        closeButton.onClick.AddListener(() => OnClickClose?.Invoke());
    }

    public void Setup (bool victory)
    {
        gameOverText.text = victory ? "You win!" : "You lose!";
    }
}
