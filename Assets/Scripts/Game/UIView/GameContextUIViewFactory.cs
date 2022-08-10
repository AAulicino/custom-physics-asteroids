using UnityEngine;

public static class GameContextUIViewFactory
{
    public static GameContextUIView Create ()
    {
        return Object.Instantiate(Resources.Load<GameContextUIView>("UI/GameContextUIView"));
    }

}
