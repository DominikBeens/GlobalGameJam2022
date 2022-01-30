
public class GameEndState : MonoState {

    public override void Enter(params object[] data) {
        Coroutiner.Delay(1.75f, () => {
            UIManager.Instance.GetPanel<GameOverPanel>().Show();
            UIManager.Instance.GetPanel<GameOverPanel>().OnRestartClicked.AddListener(HandleGameOverPanelRestartClicked);
            UIManager.Instance.GetPanel<GameOverPanel>().OnMenuClicked.AddListener(HandleGameOverPanelMenuClicked);
        });
    }

    public override void Exit() {
        UIManager.Instance.GetPanel<GameOverPanel>().Hide();
        UIManager.Instance.GetPanel<GameOverPanel>().OnRestartClicked.RemoveListener(HandleGameOverPanelRestartClicked);
        UIManager.Instance.GetPanel<GameOverPanel>().OnMenuClicked.RemoveListener(HandleGameOverPanelMenuClicked);
    }

    public override void Tick() { }

    private void HandleGameOverPanelRestartClicked() {
        Game.Instance.RestartGame();
    }

    private void HandleGameOverPanelMenuClicked() {
        Game.Instance.LoadMenu();
    }
}
