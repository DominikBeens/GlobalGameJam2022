using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class Player : MonoBehaviour {

    [SerializeField] private GameObject visualLight;
    [SerializeField] private GameObject visualDark;
    [Space]
    [SerializeField] private Animator animatorLight;
    [SerializeField] private Animator animatorDark;
    [Space]
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

    private float attack1Timer;

    private void Awake() {
        GameEvents.OnVisualTypeChanged.AddListener(HandleVisualTypeChanged);
        HandleVisualTypeChanged(WorldTypeManager.Instance.VisualType);
    }

    private void OnDestroy() {
        GameEvents.OnVisualTypeChanged.RemoveListener(HandleVisualTypeChanged);
    }

    private void HandleVisualTypeChanged(VisualType type) {
        visualLight.SetActive(type == VisualType.Light);
        visualDark.SetActive(type == VisualType.Dark);
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
        Attack1 attack = attack1.FirstOrDefault(x => x.VisualType == WorldTypeManager.Instance.VisualType);
        PlayerProjectile projectile = Instantiate(attack.ProjectilePrefab, projectileSpawn.position, Quaternion.identity);
        projectile.Initialize(projectileSpawn.up);
    }
}
