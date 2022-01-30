
public static class GameEvents {

    public static GameEvent<float, float> OnPlayerDistanceTraveled = new();
    public static GameEvent<VisualType> OnVisualTypeChanged = new();

    public static GameEvent OnGameStarted = new();
    public static GameEvent OnGameEnded = new();

    public static GameEvent OnPlayerSpawned = new();
    public static GameEvent OnPlayerDied = new();
}
