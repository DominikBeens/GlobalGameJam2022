using UnityEngine;

public class UITabButton : UIIconButton {

    [Header("Tab")]
    [SerializeField] private Sprite selectedSprite;
    [SerializeField] private GameObject tabObjectReference;

    public GameObject TabObjectReference => tabObjectReference;

    public bool IsSelected { get; private set; }

    private Sprite defaultSprite;

    protected override void Awake() {
        base.Awake();
        defaultSprite = background.sprite;
    }

    public void ToggleSelected(bool state) {
        background.sprite = state ? selectedSprite : defaultSprite;
        IsSelected = state;
    }
}
