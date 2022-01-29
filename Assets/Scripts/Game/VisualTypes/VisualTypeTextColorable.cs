using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;
using TMPro;

public class VisualTypeTextColorable : MonoBehaviour {

    [SerializeField] private float colorChangeDuration = 0.2f;
    [SerializeField] private List<Colorable> colorables = new();

    private TextMeshProUGUI[] texts;

    [Serializable]
    public struct Colorable {
        public VisualType VisualType;
        public Color Color;
    }

    private void Awake() {
        texts = GetComponentsInChildren<TextMeshProUGUI>();

        GameEvents.OnVisualTypeChanged.AddListener(HandleVisualTypeChanged);
        HandleVisualTypeChanged(WorldTypeManager.Instance.VisualType);
    }

    private void OnDestroy() {
        GameEvents.OnVisualTypeChanged.RemoveListener(HandleVisualTypeChanged);
    }

    private void HandleVisualTypeChanged(VisualType type) {
        Colorable colorable = colorables.Find(x => x.VisualType == type);
        foreach (TextMeshProUGUI text in texts) {
            text.DOColor(colorable.Color, colorChangeDuration);
        }
    }
}
