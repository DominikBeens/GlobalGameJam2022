using UnityEngine;
using DG.Tweening;

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

    public bool GameStarted { get; private set; }

    private float worldMoveSpeed;
    private float worldSpeedIncreaseInterval;

    private float distance;
    private float distanceThisFrame;
    private float distanceForSpeedIncrease;

    protected override void Awake() {
        base.Awake();
        gameBackgroundSpawner.Spawn();
    }

    public override void Enter(params object[] data) {
        base.Enter(data);
        EnterState<GameStartState>();
    }

    public override void Tick() {
        base.Tick();
        if (!GameStarted) { return; }

        ProcessSpeed();
        ProcessDistance();
        ProcessGameBackgroundPosition();
    }

    public void StartGame() {
        worldMoveSpeed = baseWorldMoveSpeed;
        worldSpeedIncreaseInterval = baseSpeedIncreaseInterval;

        Time.timeScale = 1f;

        GameEvents.OnGameStarted.Invoke();
        GameStarted = true;
    }

    public void StopGame() {
        float timeScale = Time.timeScale;
        DOTween.To(() => timeScale, x => timeScale = x, 0.25f, 1f).SetDelay(1f).SetUpdate(true).OnUpdate(() => {
            Time.timeScale = timeScale;
        });

        EnterState<GameEndState>();

        GameEvents.OnGameEnded.Invoke();
        GameStarted = false;
    }

    private void ProcessSpeed() {
        if (worldMoveSpeed >= maxWorldSpeed) { return; }

        distanceForSpeedIncrease += distanceThisFrame;
        if (distanceForSpeedIncrease < worldSpeedIncreaseInterval) { return; }

        distanceForSpeedIncrease = 0f;
        worldMoveSpeed += speedIncrease;
        worldSpeedIncreaseInterval += (baseSpeedIncreaseInterval * 2f);
    }

    private void ProcessDistance() {
        distanceThisFrame = worldMoveSpeed * Time.deltaTime;
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
