using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GroundSpawner : MonoBehaviour {

    [SerializeField] private Transform groundContainer;
    [SerializeField] private List<GroundChunk> groundChunks = new();
    [Space]
    [SerializeField] private float groundChunkWidth = 20f;
    [SerializeField] private float groundChunkHeight = 5f;

    private List<Tuple<GroundChunk, Ground>> groundChunkHistory = new();
    private float currentGroundY;

    private float distance;
    private float distanceThisFrame;

    [Serializable]
    public class GroundChunk {
        public enum GroundChunkType { Flat, Up, Down }

        public GroundChunkType Type;
        public Ground Prefab;
    }

    private void Awake() {
        GameEvents.OnPlayerDistanceTraveled.AddListener(HandlePlayerDistanceTraveled);
        StartSpawning();
    }

    private void OnDestroy() {
        GameEvents.OnPlayerDistanceTraveled.RemoveListener(HandlePlayerDistanceTraveled);
    }

    private void HandlePlayerDistanceTraveled(float total, float frame) {
        distance += frame;
        distanceThisFrame = frame;

        ProcessGround();
    }

    private void StartSpawning() {
        for (int i = 0; i < 4; i++) {
            SpawnGroundChunk(GroundChunk.GroundChunkType.Flat);
        }
    }

    private void ProcessGround() {
        if (distance > groundChunkWidth) {
            distance = 0f;
            SpawnGroundChunk();
        }

        for (int i = groundChunkHistory.Count - 1; i >= 0; i--) {
            Ground ground = groundChunkHistory[i].Item2;
            ground.transform.localPosition += Vector3.left * distanceThisFrame;

            if (ground.transform.localPosition.x < -(groundChunkWidth * 3f)) {
                groundChunkHistory.RemoveAt(i);
                Destroy(ground);
            }
        }
    }

    private void SpawnGroundChunk(GroundChunk.GroundChunkType? forcedType = null) {
        GroundChunk chunk = GetNewGroundChunk(forcedType);
        Ground ground = Instantiate(chunk.Prefab, groundContainer);

        Vector3 position = Vector3.zero;
        if (groundChunkHistory.Count > 0) {
            position = groundChunkHistory[groundChunkHistory.Count - 1].Item2.transform.localPosition + (Vector3.right * groundChunkWidth);
            position.y = currentGroundY;
        }

        currentGroundY += chunk.Type == GroundChunk.GroundChunkType.Up ? groundChunkHeight :
                          chunk.Type == GroundChunk.GroundChunkType.Down ? -groundChunkHeight : 0;

        ground.transform.localPosition = position;
        groundChunkHistory.Add(new(chunk, ground));
    }

    private GroundChunk GetNewGroundChunk(GroundChunk.GroundChunkType? forcedType = null) {
        GroundChunk latestChunk = groundChunkHistory.Count > 0 ? groundChunkHistory[groundChunkHistory.Count - 1].Item1 : null;
        GroundChunk newChunk = null;

        if (forcedType != null) {
            newChunk = groundChunks.Where(x => x.Type == forcedType).Random();
        } else {
            if (latestChunk != null && latestChunk.Type == GroundChunk.GroundChunkType.Flat) {
                newChunk = groundChunks.Random();
            } else {
                newChunk = groundChunks.Where(x => x.Type == GroundChunk.GroundChunkType.Flat).Random();
            }
        }

        return newChunk;
    }
}
