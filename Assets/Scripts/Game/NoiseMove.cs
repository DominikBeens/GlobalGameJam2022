using UnityEngine;

public class NoiseMove : MonoBehaviour {

    [SerializeField] private float strength = 1f;
    [SerializeField] private float speed = 1f;

    private Vector3 defaultPosition;
    private Vector3 offset;

    private float xRandom;
    private float yRandom;

    private void Awake() {
        defaultPosition = transform.position;
        xRandom = Random.Range(0f, 100000f);
        yRandom = Random.Range(0f, 100000f);
    }

    private void Update() {
        offset.x = strength * Mathf.PerlinNoise(Time.time * speed, xRandom);
        offset.y = strength * Mathf.PerlinNoise(Time.time * speed, yRandom);
        transform.position = defaultPosition + offset;
    }
}
