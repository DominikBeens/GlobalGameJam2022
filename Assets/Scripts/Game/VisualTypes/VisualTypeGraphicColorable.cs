using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;
using UnityEngine.UI;

public class VisualTypeGraphicColorable : MonoBehaviour {

    [SerializeField] private float colorChangeDuration = 0.2f;
    [SerializeField] private List<Colorable> colorables = new();

    private Graphic[] graphics;

    [Serializable]
    public struct Colorable {
        public VisualType VisualType;
        public Color Color;
    }

    private void Awake() {
        graphics = GetComponentsInChildren<Graphic>();

        GameEvents.OnVisualTypeChanged.AddListener(HandleVisualTypeChanged);
        HandleVisualTypeChanged(WorldTypeManager.Instance.VisualType);

        Game.OnGameLoadingEnded.AddListener(HandleLoadingEnded);
    }

    private void OnDestroy() {
        GameEvents.OnVisualTypeChanged.RemoveListener(HandleVisualTypeChanged);
        Game.OnGameLoadingEnded.RemoveListener(HandleLoadingEnded);
    }

    private void HandleVisualTypeChanged(VisualType type) {
        Colorable colorable = colorables.Find(x => x.VisualType == type);
        foreach (Graphic graphic in graphics) {
            graphic.DOColor(colorable.Color, colorChangeDuration);
        }
    }

    private void HandleLoadingEnded() {
        HandleVisualTypeChanged(WorldTypeManager.Instance.VisualType);
    }
}
