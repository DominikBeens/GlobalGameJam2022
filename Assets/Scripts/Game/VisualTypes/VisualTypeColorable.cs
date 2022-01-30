using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;

public class VisualTypeColorable : MonoBehaviour {

    [SerializeField] private float colorChangeDuration = 0.2f;
    [SerializeField] private List<Colorable> colorables = new();

    private SpriteRenderer[] spriteRenderers;
    private Colorable colorable;

    [Serializable]
    public struct Colorable {
        public VisualType VisualType;
        public Color Color;
    }

    private void Awake() {
        spriteRenderers = GetComponentsInChildren<SpriteRenderer>();

        GameEvents.OnVisualTypeChanged.AddListener(HandleVisualTypeChanged);
        ChangeColor(WorldTypeManager.Instance.VisualType, true);
    }

    private void OnDestroy() {
        GameEvents.OnVisualTypeChanged.RemoveListener(HandleVisualTypeChanged);
    }

    private void HandleVisualTypeChanged(VisualType type) {
        ChangeColor(type);
    }

    private void ChangeColor(VisualType type, bool instant = false) {
        colorable = colorables.Find(x => x.VisualType == type);
        foreach (SpriteRenderer spriteRenderer in spriteRenderers) {
            if (instant) {
                spriteRenderer.color = colorable.Color;
            } else {
                spriteRenderer.DOColor(colorable.Color, colorChangeDuration);
            }
        }
    }
}
