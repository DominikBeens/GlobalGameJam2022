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

    public float WorldMoveSpeed { get; private set; }

    private float worldSpeedIncreaseInterval;

    private float distance;
    private float distanceThisFrame;
    private float distanceForSpeedIncrease;

    protected override void Awake() {
        base.Awake();
        GameStateMachine.Instance.EnterState<GameStateMachine>();
        PlayerManager.Instance.SpawnPlayer();
    }

    public override void Enter(params object[] data) {
        base.Enter(data);

        WorldMoveSpeed = baseWorldMoveSpeed;
        worldSpeedIncreaseInterval = baseSpeedIncreaseInterval;

        WorldTypeManager.Instance.StartClock();
    }

    public override void Exit() {
        base.Exit();
        WorldTypeManager.Instance.StopClock();
    }

    public override void Tick() {
        base.Tick();
        ProcessSpeed();
        ProcessDistance();
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
}
