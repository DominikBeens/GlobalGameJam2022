using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;
using UnityEngine.U2D;

public class VisualTypeSpriteShapeColorable : MonoBehaviour {

    [SerializeField] private float colorChangeDuration = 0.2f;
    [SerializeField] private List<Colorable> colorables = new();

    private SpriteShapeRenderer[] spriteRenderers;

    [Serializable]
    public struct Colorable {
        public VisualType VisualType;
        public Color Color;
    }

    private void Awake() {
        spriteRenderers = GetComponentsInChildren<SpriteShapeRenderer>();

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
        Colorable colorable = colorables.Find(x => x.VisualType == type);
        foreach (SpriteShapeRenderer spriteRenderer in spriteRenderers) {
            if (instant) {
                spriteRenderer.color = colorable.Color;
            } else {
                Color color = spriteRenderer.color;
                DOTween.To(() => color, x => color = x, colorable.Color, colorChangeDuration).OnUpdate(() => {
                    spriteRenderer.color = color;
                });
            }
        }
    }
}
