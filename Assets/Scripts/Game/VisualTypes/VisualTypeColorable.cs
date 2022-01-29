using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;

public class VisualTypeColorable : MonoBehaviour {

    [SerializeField] private float colorChangeDuration = 0.2f;
    [SerializeField] private List<Colorable> colorables = new();

    private SpriteRenderer[] spriteRenderers;

    [Serializable]
    public struct Colorable {
        public VisualType VisualType;
        public Color Color;
    }

    private void Awake() {
        spriteRenderers = GetComponentsInChildren<SpriteRenderer>();

        GameEvents.OnVisualTypeChanged.AddListener(HandleVisualTypeChanged);
        HandleVisualTypeChanged(WorldTypeManager.Instance.VisualType);
    }

    private void OnDestroy() {
        GameEvents.OnVisualTypeChanged.RemoveListener(HandleVisualTypeChanged);
    }

    private void HandleVisualTypeChanged(VisualType type) {
        Colorable colorable = colorables.Find(x => x.VisualType == type);
        foreach (SpriteRenderer spriteRenderer in spriteRenderers) {
            spriteRenderer.DOColor(colorable.Color, colorChangeDuration);
        }
    }
}
