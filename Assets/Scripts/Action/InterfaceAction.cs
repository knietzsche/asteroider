using System;

public static class InterfaceAction
{
    public static Action<int> UpdateLife;
    public static Action<int> UpdateScore;
    public static Action<bool> UpdateGamePaused;
    public static Action UpdateGameOver;
    public static Action ClearGameScreen;
}
