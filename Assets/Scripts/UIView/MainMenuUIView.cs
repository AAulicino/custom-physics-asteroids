using System;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUIView : MonoBehaviour
{
    public event Action OnClickStartGame;

    [SerializeField] Button startGame;

    void Awake ()
    {
        startGame.onClick.AddListener(() => OnClickStartGame?.Invoke());
    }
}
