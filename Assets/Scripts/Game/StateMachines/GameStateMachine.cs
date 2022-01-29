using UnityEngine;

public class GameStateMachine : MonoStateMachineSingleton<GameStateMachine> {

    [Space]
    [SerializeField] private float baseWorldMoveSpeed = 5f;
    [SerializeField] private float maxWorldSpeed = 100f;
    [SerializeField] private float speedIncrease = 1f;
    [SerializeField] private float baseSpeedIncreaseInterval = 5f;
    [Space]
    [SerializeField] private GroundSpawner groundSpawner;
    [SerializeField] private ObstacleSpawner obstacleSpawner;
    [SerializeField] private GameBackgroundSpawner gameBackgroundSpawner;

    public float WorldMoveSpeed { get; private set; }

    private float worldSpeedIncreaseInterval;

    private float distance;
    private float distanceThisFrame;
    private float distanceForSpeedIncrease;

    protected override void Awake() {
        base.Awake();
        PlayerManager.Instance.SpawnPlayer();
        gameBackgroundSpawner.Spawn();
    }

    public override void Enter(params object[] data) {
        base.Enter(data);

        WorldMoveSpeed = baseWorldMoveSpeed;
        worldSpeedIncreaseInterval = baseSpeedIncreaseInterval;

        WorldTypeManager.Instance.StartClock();
        UIManager.Instance.GetPanel<ScorePanel>().Show();
        UIManager.Instance.GetPanel<VisualSwitchPanel>().Show();

        GameEvents.OnGameStarted.Invoke();
    }

    public override void Exit() {
        base.Exit();
        WorldTypeManager.Instance.StopClock();
        UIManager.Instance.GetPanel<ScorePanel>().Hide();
        UIManager.Instance.GetPanel<VisualSwitchPanel>().Hide();
    }

    public override void Tick() {
        base.Tick();
        ProcessSpeed();
        ProcessDistance();
        ProcessGameBackgroundPosition();
    }

    private void ProcessSpeed() {
        if (WorldMoveSpeed >= maxWorldSpeed) { return; }

        distanceForSpeedIncrease += distanceThisFrame;
        if (distanceForSpeedIncrease < worldSpeedIncreaseInterval) { return; }

        distanceForSpeedIncrease = 0f;
        WorldMoveSpeed += speedIncrease;
        worldSpeedIncreaseInterval += (baseSpeedIncreaseInterval * 2f);
    }

    private void ProcessDistance() {
        distanceThisFrame = WorldMoveSpeed * Time.deltaTime;
        distance += distanceThisFrame;
        GameEvents.OnPlayerDistanceTraveled.Invoke(distance, distanceThisFrame);
    }

    // Background spawns in viewport but level can go up or down randomly resulting in the player potentially being able to 
    // go 'out of bounds' on the Y axis meaning he will go abve or below the spawn bounds of the background.
    // EZ FIX: just move the background Y with the player 🤪
    private void ProcessGameBackgroundPosition() {
        Vector3 target = gameBackgroundSpawner.transform.position;
        target.y = PlayerManager.Instance.Player.transform.position.y;
        gameBackgroundSpawner.transform.position = Vector3.Lerp(gameBackgroundSpawner.transform.position, target, Time.deltaTime);
    }
}
