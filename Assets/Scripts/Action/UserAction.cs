using System;
using static ScreenManager;

public static class UserAction
{
    public static Action<bool> Select;
    public static Action Confirm;
    public static Action Escape;

    public static Action<ScreenType> ChangeScreen;

    public static Action PauseGame;

    public static Action<float> Thrust;
    public static Action<float> Rotate;
    public static Action Fire;
}
