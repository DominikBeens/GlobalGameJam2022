using UnityEngine;
using DG.Tweening;

public class PlayerProjectile : MonoBehaviour {

    [SerializeField] private Transform visual;
    [SerializeField] private VisualType visualType;
    [Space]
    [SerializeField] private float force = 5f;
    [SerializeField] private float lifetime = 5f;

    private Rigidbody2D body;
    private bool isDestroying;
    private float remainingLifetime;

    public void Initialize(Vector3 direction) {
        body = GetComponent<Rigidbody2D>();
        remainingLifetime = lifetime;

        Vector2 scaledForce = direction * force * Time.deltaTime;
        body.AddForce(scaledForce, ForceMode2D.Impulse);

        visual.localScale = Vector3.zero;
        visual.DOScale(1f, 0.3f).SetEase(Ease.OutBack);
    }

    private void Update() {
        ProcessLifetime();
    }

    private void ProcessLifetime() {
        if (isDestroying) { return; }

        remainingLifetime -= Time.deltaTime;
        if (remainingLifetime > 0f) { return; }

        Destroy();
    }

    private void Destroy() {
        isDestroying = true;
        visual.DOKill();
        visual.DOScale(0f, 0.2f).OnComplete(() => Destroy(gameObject));
    }

    private void OnTriggerEnter2D(Collider2D collider) {
        if (isDestroying) { return; }

        Obstacle obstacle = collider.GetComponent<Obstacle>();
        if (obstacle) {
            obstacle.Hit(visualType);
            Destroy();
        }
    }
}
