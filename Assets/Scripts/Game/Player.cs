using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class Player : MonoBehaviour {

    [SerializeField] private List<Attack1> attack1 = new();
    [SerializeField] private float attack1Cooldown = 1f;
    [Space]
    [SerializeField] private Transform shooter;
    [SerializeField] private Transform projectileSpawn;

    [Serializable]
    public struct Attack1 {
        public VisualType VisualType;
        public PlayerProjectile ProjectilePrefab;
    }

    private VisualType visualType;
    private float attack1Timer;
    private float velocity;

    private void Awake() {
        GameEvents.OnPlayerDistanceTraveled.AddListener(HandlePlayerDistanceTraveled);
    }

    private void OnDestroy() {
        GameEvents.OnPlayerDistanceTraveled.RemoveListener(HandlePlayerDistanceTraveled);
    }

    private void HandlePlayerDistanceTraveled(float total, float frame) {
        velocity = frame;
    }

    private void Update() {
        ProcessAiming();
        ProcessAttack1();
    }

    private void ProcessAiming() {
        Vector2 mousePosition = Input.mousePosition;
        Vector3 target = Camera.main.WorldToScreenPoint(transform.localPosition);
        Vector2 direction = new Vector2(mousePosition.x - target.x, mousePosition.y - target.y);
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        shooter.transform.localRotation = Quaternion.Euler(0, 0, angle - 90);
    }

    private void ProcessAttack1() {
        if (attack1Timer > 0f) {
            attack1Timer -= Time.deltaTime;
            return;
        }

        if (!Input.GetMouseButtonDown(0)) { return; }

        attack1Timer = attack1Cooldown;
        Attack1 attack = attack1.FirstOrDefault(x => x.VisualType == visualType);
        PlayerProjectile projectile = Instantiate(attack.ProjectilePrefab, projectileSpawn.position, Quaternion.identity);
        projectile.Initialize(projectileSpawn.up, velocity);
    }
}
