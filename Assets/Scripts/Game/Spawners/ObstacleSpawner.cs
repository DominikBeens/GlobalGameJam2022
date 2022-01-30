using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour {

    [SerializeField] private List<Obstacle> obstacles = new();
    [Space]
    [SerializeField] private float startSpawnAfterDistance = 100f;
    [SerializeField] private float interval = 25f;
    [SerializeField] private float xSpawnStart = 25f;
    [SerializeField] private float xSpawnOffset = 5f;

    private List<Obstacle> history = new();

    private int groundLayerMask;

    private float distance;
    private float distanceThisFrame;

    private void Awake() {
        GameEvents.OnPlayerDistanceTraveled.AddListener(HandlePlayerDistanceTraveled);
        groundLayerMask = LayerMask.GetMask("Ground");
    }

    private void OnDestroy() {
        GameEvents.OnPlayerDistanceTraveled.RemoveListener(HandlePlayerDistanceTraveled);
    }

    private void Update() {
        ProcessSpawning();
    }

    private void HandlePlayerDistanceTraveled(float total, float frame) {
        if (total < startSpawnAfterDistance) { return; }
        distance += frame;
        distanceThisFrame = frame;
    }

    private void ProcessSpawning() {
        if (distance < interval) { return; }
        distance = 0f;
        SpawnObstacle();
    }

    private void SpawnObstacle() {
        Vector3 position = GetSpawnPosition(out Transform groundChunk);
        if (position == Vector3.zero) { return; }

        Obstacle obstacle = Instantiate(GetNewObstacle(), position, Quaternion.identity);
        obstacle.transform.SetParent(groundChunk.parent);
        obstacle.Intialize();
    }

    private Vector3 GetSpawnPosition(out Transform groundChunk) {
        Vector3 rayStart = new Vector3(xSpawnStart + Random.Range(-xSpawnOffset, xSpawnOffset), FindObjectOfType<Player>().transform.position.y + 10f, 0f);

        RaycastHit2D hit = Physics2D.Raycast(rayStart, Vector3.down, 100f, groundLayerMask);
        if (hit.collider != null) {
            RaycastHit2D hitLeft = Physics2D.Raycast(hit.point + Vector2.up * 2 + Vector2.left * 1.75f, Vector3.down, 10f, groundLayerMask);
            if (hitLeft.collider != null) {
                RaycastHit2D hitRight = Physics2D.Raycast(hit.point + Vector2.up * 2 + Vector2.right * 1.75f, Vector3.down, 10f, groundLayerMask);
                if (hitRight.collider != null) {
                    groundChunk = hit.transform;
                    return hit.point;
                }
            }
        }

        groundChunk = null;
        return Vector3.zero;
    }

    private Obstacle GetNewObstacle() {
        return obstacles.Random();
    }
}
