using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;
using Random = UnityEngine.Random;

public class Obstacle : MonoBehaviour {

    public Visual ActiveVisual { get; private set; }

    [SerializeField] private List<Visual> visuals = new();
    [SerializeField] private AudioClip hitClip;

    [Serializable]
    public struct Visual {
        public VisualType VisualType;
        public GameObject GameObject;
    }

    public void Intialize() {
        VisualType random;
        if (WorldTypeManager.Instance.VisualType == VisualType.Light) {
            random = Random.value < 0.8f ? VisualType.Dark : VisualType.Light;
        } else {
            random = Random.value < 0.8f ? VisualType.Light : VisualType.Dark;
        }
        SetVisual(random);

        ActiveVisual.GameObject.transform.localScale = Vector3.zero;
        ActiveVisual.GameObject.transform.DOScale(1f, 0.25f).SetEase(Ease.OutBack);
    }

    public void Hit(VisualType visualType) {
        if (visualType == ActiveVisual.VisualType) { return; }
        SetVisual(visualType);
        Animate();
        DB.SimpleFramework.SimpleAudioManager.SimpleAudioManager.Play2D(hitClip, pitch: Random.Range(0.9f, 1.1f));
    }

    private void OnTriggerEnter2D(Collider2D collider) {
        Player player = collider.GetComponent<Player>();
        if (player && WorldTypeManager.Instance.VisualType != ActiveVisual.VisualType) {
            GameEvents.OnPlayerDied.Invoke();
        }
    }

    private void SetVisual(VisualType visualType) {
        if (ActiveVisual.GameObject) {
            ActiveVisual.GameObject.transform.DOKill(true);
        }

        ActiveVisual = visuals.Find(x => x.VisualType == visualType);
        foreach (Visual visual in visuals) {
            visual.GameObject.SetActive(visual.VisualType == visualType);
        }
    }

    private void Animate() {
        ActiveVisual.GameObject.transform.DOKill(true);
        ActiveVisual.GameObject.transform.DOPunchScale(new Vector3(0.15f, 0.25f, 0.15f), 0.25f);
    }
}
