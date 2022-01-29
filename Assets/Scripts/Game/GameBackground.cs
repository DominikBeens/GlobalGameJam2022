using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;

public class GameBackground : MonoBehaviour {

    [SerializeField] private float fadeDuration = 0.2f;
    [SerializeField] private List<Background> backgrounds = new();

    [Serializable]
    public struct Background {
        public VisualType VisualType;
        public SpriteRenderer SpriteRenderer;
    }

    public void Awake() {
        GameEvents.OnVisualTypeChanged.AddListener(HandleVisualTypeChanged);
        ChangeBackground(WorldTypeManager.Instance.VisualType, true);
    }

    private void OnDestroy() {
        GameEvents.OnVisualTypeChanged.RemoveListener(HandleVisualTypeChanged);
    }

    private void HandleVisualTypeChanged(VisualType type) {
        ChangeBackground(type);
    }

    private void ChangeBackground(VisualType visualType, bool instant = false) {
        float duration = instant ? 0f : fadeDuration;
        foreach (Background background in backgrounds) {
            if (background.VisualType == visualType) {
                background.SpriteRenderer.DOFade(1f, duration);
            } else {
                background.SpriteRenderer.DOFade(0f, duration);
            }
        }
    }
}