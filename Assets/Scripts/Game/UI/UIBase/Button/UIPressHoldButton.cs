using UnityEngine.UI;
using UnityEngine;
using DG.Tweening;
using System;

public class UIPressHoldButton : UIButton {

    public event Action OnLongClick = delegate { };

    [Header("Press Hold Button")]
    [SerializeField] private float holdDuration = 1f;
    [SerializeField] private float delay = 0.2f;
    [SerializeField] private bool autoClick;
    [SerializeField] private Image fillImage;
    [SerializeField] private Image iconImage;

    private float currentDuration;
    private float currentDelay;
    private Sprite defaultIcon;

    protected override bool allowClick => currentDuration < holdDuration;

    protected override void Awake() {
        base.Awake();
        defaultIcon = iconImage?.sprite;
    }

    private void Update() {
        HandleDuration();
    }

    public void SetIcon(Sprite icon, float opacity = 1f) {
        iconImage.sprite = icon;
        iconImage.DOFade(opacity, 0f);
    }

    public void ResetIcon(float opacity = 1f) {
        iconImage.sprite = defaultIcon;
        iconImage.DOFade(opacity, 0f);
    }

    protected override void HandleClick() {
        base.HandleClick();
        if (!allowClick) {
            OnLongClick();
        }
        ResetValues();
    }

    protected override void HandleCancelClick() {
        base.HandleCancelClick();
        ResetValues();
    }

    private void HandleDuration() {
        if (!isPressed) { return; }

        if (currentDelay < delay) {
            currentDelay += Time.deltaTime;
            return;
        }

        currentDuration += Time.deltaTime;
        UpdateFill();
    }

    private void UpdateFill() {
        if (!fillImage) { return; }
        fillImage.fillAmount = currentDuration / holdDuration;
        if (fillImage.fillAmount >= 1f && autoClick) {
            ForceClick();
        }
    }

    private void ResetValues() {
        currentDuration = 0f;
        currentDelay = 0f;
        UpdateFill();
    }
}
