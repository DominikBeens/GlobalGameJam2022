using UnityEngine;
using DG.Tweening;

public class PlayerProjectile : MonoBehaviour {

    [SerializeField] private Transform visual;
    [SerializeField] private VisualType visualType;
    [SerializeField] private ParticleSystem hitParticle;
    [Space]
    [SerializeField] private float force = 5f;
    [SerializeField] private float lifetime = 5f;

    private Rigidbody2D body;
    private bool isDestroying;
    private float remainingLifetime;

    public void Initialize(Vector3 direction) {
        body = GetComponent<Rigidbody2D>();
        remainingLifetime = lifetime;

        Vector2 scaledForce = direction * force;
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

        body.isKinematic = true;
        body.velocity = Vector2.zero;
        body.angularVelocity = 0f;

        visual.DOKill();
        visual.DOScale(0f, 0.2f).OnComplete(() => {
            Destroy(gameObject, 2f);
        });
    }

    private void OnTriggerEnter2D(Collider2D collider) {
        if (isDestroying) { return; }

        Obstacle obstacle = collider.GetComponent<Obstacle>();
        if (obstacle) {
            obstacle.Hit(visualType);
            hitParticle.Play();
            Destroy();
        }
    }
}
