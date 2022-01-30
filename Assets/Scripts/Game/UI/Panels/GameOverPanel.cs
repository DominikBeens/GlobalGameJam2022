using DG.Tweening;
using TMPro;
using UnityEngine;

public class GameOverPanel : UIPanel {

    public GameEvent OnRestartClicked = new();
    public GameEvent OnMenuClicked = new();

    public override bool IsBackButtonClosable => true;
    public override bool UsesBackgroundBlocker => true;

    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private UIButton restartButton;
    [SerializeField] private UIButton menuButton;

    public override void Show() {
        base.Show();

        panel.DOKill(true);
        panel.localScale = Vector3.one * 0.5f;
        panel.DOScale(1f, 0.25f).SetEase(Ease.OutBack).SetUpdate(true);

        canvasGroup.DOKill(true);
        canvasGroup.alpha = 0f;
        canvasGroup.DOFade(1f, 0.1f).SetUpdate(true);

        scoreText.text = $"Score: {GameStateMachine.Instance.Distance:F0}";

        restartButton.AddListener(OnRestartButtonClicked);
        menuButton.AddListener(OnMenuButtonClicked);
    }

    public override void Hide() {
        base.Hide();
        restartButton.RemoveListener(OnRestartButtonClicked);
        menuButton.RemoveListener(OnMenuButtonClicked);
    }

    private void OnRestartButtonClicked() {
        OnRestartClicked.Invoke();
    }

    private void OnMenuButtonClicked() {
        OnMenuClicked.Invoke();
    }
}
