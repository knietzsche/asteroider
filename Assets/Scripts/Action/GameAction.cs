using System;

public static class GameAction
{
    public static Action<GameManager.GameState> SetGameState;
    public static Action<int> AddScore;
    public static Action<bool> UpdateAsteroidCount;
    public static Action ShipDestroyed;
    public static Action SpecialDestroyed;
    public static Action<Type> CollidableColided;
}
