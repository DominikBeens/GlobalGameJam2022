using UnityEngine.UI;
using UnityEngine;

public class UIButton : UIButtonBase {

    [SerializeField] protected Transform container;
    [SerializeField] protected Image background;

    [Header("Animation")]
    [SerializeField] private bool isAnimated = true;
    [SerializeField] private float pointerDownSize = 0.85f;
    [SerializeField] private float pointerClickOvershoot = 8f;

    protected CanvasGroup canvasGroup;

    protected virtual void Awake() {
        TryGetCanvasGroup();
    }

    private void OnDisable() {
        container?.DOKillTween();
    }

    public void ToggleInteraction(bool state, bool adjustAlpha = true) {
        TryGetCanvasGroup();

        canvasGroup.interactable = state;
        canvasGroup.blocksRaycasts = state;
        IsInteractable = state;

        if (adjustAlpha) {
            canvasGroup.alpha = state ? 1f : 0.65f;
        }
    }

    protected override void HandlePointerDown() {
        base.HandlePointerDown();
        if (isAnimated) {
            container?.DOButtonScaleIn(pointerDownSize);
        }
    }

    protected override void HandleClick() {
        base.HandleClick();
        if (isAnimated) {
            container?.DOButtonScaleClick(pointerClickOvershoot);
        }
    }

    protected override void HandleCancelClick() {
        base.HandleCancelClick();
        if (isAnimated) {
            container?.DOButtonScaleOut();
        }
    }

    private void TryGetCanvasGroup() {
        if (canvasGroup) { return; }
        canvasGroup = GetComponent<CanvasGroup>();
    }
}
