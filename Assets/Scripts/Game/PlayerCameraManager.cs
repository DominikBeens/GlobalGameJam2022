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

    private void Awake() {
        virtualCamera = GetComponentInChildren<CinemachineVirtualCamera>();
        cameraOffset = GetComponentInChildren<CinemachineCameraOffset>();
        Camera = GetComponentInChildren<Camera>();

        defaultOrthoSize = virtualCamera.m_Lens.OrthographicSize;

        GameEvents.OnPlayerSpawned.AddListener(HandlePlayerSpawned);
        GameEvents.OnGameStarted.AddListener(HandleGameStarted);
        GameEvents.OnGameEnded.AddListener(HandleGameEnded);
    }

    private void OnDestroy() {
        GameEvents.OnPlayerSpawned.RemoveListener(HandlePlayerSpawned);
        GameEvents.OnGameStarted.RemoveListener(HandleGameStarted);
        GameEvents.OnGameEnded.AddListener(HandleGameEnded);
    }

    private void Update() {
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
}
