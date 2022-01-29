using UnityEngine;
using DG.Tweening;

public static class TweenHelper {

    public static void DOKillTween(this Transform t, bool complete = false) {
        t.DOKill(complete);
    }

    public static Tween DOButtonScaleIn(this Transform t, float scale = 0.85f) {
        t.DOKill(true);
        return t.DOScale(scale, 0.1f).SetEase(Ease.OutSine);
    }

    public static Tween DOButtonScaleOut(this Transform t) {
        t.DOKill(true);
        return t.DOScale(1f, 0.1f).SetEase(Ease.OutSine);
    }

    public static Tween DOButtonScaleClick(this Transform t, float overshoot = 8f) {
        t.DOKill(true);
        return t.DOScale(1f, 0.2f).SetEase(Ease.OutBack, overshoot);
    }

    public static Tween DOButtonScaleClick2(this Transform t) {
        t.DOKill(true);
        return t.DOPunchScale(Vector3.one * -0.2f, 0.2f);
    }

    public static Tween DOAppear(this Transform t, float scaleStart = 0.5f, float delay = 0f) {
        t.DOKill(true);
        t.localScale = Vector3.one * scaleStart;
        t.gameObject.SetActive(delay <= 0f);
        return t.DOScale(1f, 0.2f).SetEase(Ease.OutBack).SetDelay(delay).OnStart(() => t.gameObject.SetActive(true));
    }

    public static Tween DODisappear(this Transform t, float scaleEnd = 0.5f, float delay = 0f) {
        t.DOKill(true);
        return t.DOScale(scaleEnd, 0.15f).SetEase(Ease.InBack).SetDelay(delay).OnComplete(() => t.gameObject.SetActive(false));
    }

    public static Tween DOFadeMoveUp(this Transform t, CanvasGroup canvasGroup, float moveAmount = 25f, float duration = 0.35f) {
        Vector3 offset = Vector3.down * moveAmount;
        return t.DOFadeMove(canvasGroup, offset, duration);
    }

    public static Tween DOFadeMoveDown(this Transform t, CanvasGroup canvasGroup, float moveAmount = 25f, float duration = 0.35f) {
        Vector3 offset = Vector3.up * moveAmount;
        return t.DOFadeMove(canvasGroup, offset, duration);
    }

    private static Tween DOFadeMove(this Transform t, CanvasGroup canvasGroup, Vector3 offset, float duration = 0.35f) {
        t.DOKill(true);
        canvasGroup.DOKill(true);

        Vector3 position = t.localPosition;
        t.localPosition += offset;

        canvasGroup.alpha = 0.25f;

        Sequence sequence = DOTween.Sequence();
        sequence.Join(t.DOLocalMove(position, duration).SetEase(Ease.OutCubic));
        sequence.Join(canvasGroup.DOFade(1f, duration * 0.15f).SetEase(Ease.InSine));
        return sequence;
    }
}
