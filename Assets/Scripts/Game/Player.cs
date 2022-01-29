using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using DG.Tweening;

public class Player : MonoBehaviour {

    private float VISUAL_Y_ROT_LEFT = 0;
    private float VISUAL_Y_ROT_RIGHT = 180;

    [SerializeField] private Transform visualContainer;
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
        ProcessOrientation();
        ProcessAttack1();
    }

    private void ProcessAiming() {
        Vector2 mousePosition = Input.mousePosition;
        Vector3 target = PlayerCameraManager.Instance.Camera.WorldToScreenPoint(shooter.position);
        Vector2 direction = new Vector2(mousePosition.x - target.x, mousePosition.y - target.y);
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        shooter.transform.localRotation = Quaternion.Euler(0, 0, angle - 90);
    }

    private void ProcessOrientation() {
        Vector2 mousePosition = Input.mousePosition;
        Vector3 position = PlayerCameraManager.Instance.Camera.WorldToScreenPoint(transform.position);

        if (mousePosition.x < position.x && visualContainer.localEulerAngles.y != VISUAL_Y_ROT_LEFT) {
            visualContainer.DOKill();
            visualContainer.DOLocalRotate(Vector3.up * VISUAL_Y_ROT_LEFT, 0.1f);
        } else if (mousePosition.x >= position.x && visualContainer.localEulerAngles.y != VISUAL_Y_ROT_RIGHT) {
            visualContainer.DOKill();
            visualContainer.DOLocalRotate(Vector3.up * VISUAL_Y_ROT_RIGHT, 0.1f);
        }
    }

    private void ProcessAttack1() {
        if (attack1Timer > 0f) {
            attack1Timer -= Time.deltaTime;
            return;
        }

        if (!Input.GetMouseButtonDown(0)) { return; }

        attack1Timer = attack1Cooldown;

        Coroutiner.Delay(0.2f, () => {
            Attack1 attack = attack1.FirstOrDefault(x => x.VisualType == WorldTypeManager.Instance.VisualType);
            PlayerProjectile projectile = Instantiate(attack.ProjectilePrefab, projectileSpawn.position, Quaternion.identity);
            projectile.Initialize(projectileSpawn.up);
        });

        GetAnimator().SetTrigger("Fight");
    }

    private Animator GetAnimator() {
        return WorldTypeManager.Instance.VisualType == VisualType.Light ? animatorLight : animatorDark;
    }
}
