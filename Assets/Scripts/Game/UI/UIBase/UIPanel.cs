using System.Collections;
using UnityEngine;

public class UIPanel : MonoBehaviour {

    public static GameEvent<UIPanel> OnUIPanelShow = new GameEvent<UIPanel>();
    public static GameEvent<UIPanel> OnUIPanelHide = new GameEvent<UIPanel>();

    public bool IsOpen { get; private set; }
    public Canvas Canvas { get; private set; }

    public float Height => (panel as RectTransform).rect.height;

    public virtual bool IsBackButtonClosable => false;
    public virtual bool UsesBackgroundBlocker => false;
    public virtual bool BackgroundBlockerClosesPanel => false;

    [SerializeField] protected Transform panel;

    private int defaultSortingOrder;
    private Coroutine showDelayedRoutine;

    protected CanvasGroup canvasGroup;

    public virtual void Initialize() {
        Canvas = GetComponentInChildren<Canvas>();
        if (!Canvas) {
            Debug.LogError($"No canvas found on {GetType()}!");
            return;
        }

        canvasGroup = Canvas.GetComponent<CanvasGroup>();
        defaultSortingOrder = Canvas.sortingOrder;
        Hide();
    }

    public virtual void Deinitialize() {
        Hide();
    }

    public virtual void Tick() { }
    public virtual void LateTick() { }

    public virtual void Show() {
        Canvas.enabled = true;
        ToggleInteraction(true);
        IsOpen = true;
        OnUIPanelShow.Invoke(this);
        this.TryStopRoutine(ref showDelayedRoutine);
    }

    public virtual void Hide() {
        Canvas.enabled = false;
        ToggleInteraction(false);
        IsOpen = false;
        OnUIPanelHide.Invoke(this);
    }

    public virtual void HideThroughBackButton() {
        Hide();
    }

    public void ShowDelayed(float delay) {
        this.TryStopRoutine(ref showDelayedRoutine);
        showDelayedRoutine = StartCoroutine(ShowDelayedRoutine(delay));
    }

    public void SetSortingOrder(int order) {
        Canvas.sortingOrder = order;
    }

    public void ResetSortingOrder() {
        SetSortingOrder(defaultSortingOrder);
    }

    protected void ToggleInteraction(bool state) {
        canvasGroup.ToggleInteraction(state);
    }

    private IEnumerator ShowDelayedRoutine(float delay) {
        yield return new WaitForSeconds(delay);
        Show();
        showDelayedRoutine = null;
    }
}
