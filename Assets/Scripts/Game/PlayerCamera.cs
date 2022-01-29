using UnityEngine;

public class PlayerCamera : MonoBehaviour {

    [SerializeField] private float speed = 10f;
    [SerializeField] private float scale = 1f;

    private CinemachineCameraOffset cameraOffset;

    public Camera Camera { get; private set; }

    private void Awake() {
        cameraOffset = GetComponentInChildren<CinemachineCameraOffset>();
        Camera = GetComponentInChildren<Camera>();
    }

    private void Update() {
        Vector3 offset = Camera.ScreenToViewportPoint(Input.mousePosition);
        Vector3 centeredOffset = offset - new Vector3(0.5f, 0.5f, 0f);
        centeredOffset = new Vector3(centeredOffset.x * scale, centeredOffset.y * scale, centeredOffset.z * scale);
        cameraOffset.m_Offset = Vector3.Lerp(cameraOffset.m_Offset, centeredOffset, Time.deltaTime * speed);
    }
}
