using System.Collections;
using System.Collections.Generic;
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

    private float worldMoveSpeed;
    private float worldSpeedIncreaseInterval;

    private float distance;
    private float distanceThisFrame;
    private float distanceForSpeedIncrease;

    protected override void Awake() {
        base.Awake();
        GameStateMachine.Instance.EnterState<GameStateMachine>();
    }

    public override void Enter(params object[] data) {
        base.Enter(data);

        worldMoveSpeed = baseWorldMoveSpeed;
        worldSpeedIncreaseInterval = baseSpeedIncreaseInterval;
    }

    public override void Tick() {
        base.Tick();
        ProcessSpeed();
        ProcessDistance();
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
}
