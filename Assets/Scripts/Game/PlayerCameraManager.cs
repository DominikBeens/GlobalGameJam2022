using UnityEngine;
using Cinemachine;
using DG.Tweening;

public class PlayerCameraManager : Manager<PlayerCameraManager> {

    [SerializeField] private float speed = 10f;
    [SerializeField] private float scale = 1f;

    private CinemachineVirtualCamera virtualCamera;
    private CinemachineCameraOffset cameraOffset;

    public Camera Camera { get; private set; }

    private float defaultOrthoSize;

    public override void Initialize() {
        base.Initialize();

        virtualCamera = GetComponentInChildren<CinemachineVirtualCamera>();
        cameraOffset = GetComponentInChildren<CinemachineCameraOffset>();
        Camera = GetComponentInChildren<Camera>();

        defaultOrthoSize = virtualCamera.m_Lens.OrthographicSize;

        GameEvents.OnPlayerSpawned.AddListener(HandlePlayerSpawned);
        GameEvents.OnGameStarted.AddListener(HandleGameStarted);
        GameEvents.OnGameEnded.AddListener(HandleGameEnded);

        Game.OnGameLoadingEnded.AddListener(HandleGameLoadingEnded);
    }

    public override void Deinitialize() {
        base.Deinitialize();

        GameEvents.OnPlayerSpawned.RemoveListener(HandlePlayerSpawned);
        GameEvents.OnGameStarted.RemoveListener(HandleGameStarted);
        GameEvents.OnGameEnded.AddListener(HandleGameEnded);

        Game.OnGameLoadingEnded.RemoveListener(HandleGameLoadingEnded);
    }

    private void Update() {
        if (GameStateMachine.Exists && !GameStateMachine.Instance.GameStarted) { return; }

        Vector3 offset = Camera.ScreenToViewportPoint(Input.mousePosition);
        Vector3 centeredOffset = offset - new Vector3(0.5f, 0.5f, 0f);
        centeredOffset = new Vector3(centeredOffset.x * scale, centeredOffset.y * scale, centeredOffset.z * scale);
        cameraOffset.m_Offset = Vector3.Lerp(cameraOffset.m_Offset, centeredOffset, Time.deltaTime * speed);
    }

    private void HandlePlayerSpawned() {
        virtualCamera.Follow = PlayerManager.Instance.Player.transform;
    }

    private void HandleGameStarted() {
        virtualCamera.m_Lens.OrthographicSize = defaultOrthoSize;
    }

    private void HandleGameEnded() {
        float orthoSize = virtualCamera.m_Lens.OrthographicSize;
        DOTween.To(() => orthoSize, x => orthoSize = x, defaultOrthoSize * 0.75f, 2f).SetUpdate(true).OnUpdate(() => {
            virtualCamera.m_Lens.OrthographicSize = orthoSize;
        });
    }

    private void HandleGameLoadingEnded() {
        virtualCamera.m_Lens.OrthographicSize = defaultOrthoSize;
    }
}
