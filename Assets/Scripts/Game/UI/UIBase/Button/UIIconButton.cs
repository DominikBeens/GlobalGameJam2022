using UnityEngine.UI;
using UnityEngine;
using DG.Tweening;

public class UIIconButton : UIButton {

    [Header("Icon Button")]
    [SerializeField] protected Image iconImage;

    private Sprite defaultIcon;

    protected override void Awake() {
        base.Awake();
        defaultIcon = iconImage?.sprite;
    }

    public void SetIcon(Sprite icon, float opacity = 1f) {
        iconImage.sprite = icon;
        iconImage.DOFade(opacity, 0f);
    }

    public void ResetIcon(float opacity = 1f) {
        iconImage.sprite = defaultIcon;
        iconImage.DOFade(opacity, 0f);
    }
}
