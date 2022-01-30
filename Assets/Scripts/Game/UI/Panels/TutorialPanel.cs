using UnityEngine;
using DG.Tweening;

public class TutorialPanel : UIPanel {

    public GameEvent OnCloseClicked = new();

    public override bool IsBackButtonClosable => true;
    public override bool UsesBackgroundBlocker => true;

    [SerializeField] private Transform panelPanel;
    [SerializeField] private UIButton closeButton;

    public override void Show() {
        base.Show();

        panelPanel.DOKill(true);
        panelPanel.localScale = Vector3.one * 0.5f;
        panelPanel.DOScale(1f, 0.25f).SetEase(Ease.OutBack);

        canvasGroup.DOKill(true);
        canvasGroup.alpha = 0f;
        canvasGroup.DOFade(1f, 0.1f);

        closeButton.AddListener(OnCloseButtonClicked);
    }

    public override void Hide() {
        base.Hide();
        closeButton.RemoveListener(OnCloseButtonClicked);
    }

    private void OnCloseButtonClicked() {
        OnCloseClicked.Invoke();
    }
}
