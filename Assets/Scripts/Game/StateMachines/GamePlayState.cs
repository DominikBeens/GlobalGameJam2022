
public class GamePlayState : MonoState {

    public override void Enter(params object[] data) {
        GameStateMachine.Instance.StartGame();
        WorldTypeManager.Instance.StartClock();

        UIManager.Instance.GetPanel<ScorePanel>().Show();
        UIManager.Instance.GetPanel<VisualSwitchPanel>().Show();

        GameEvents.OnPlayerDied.AddListener(HandlePlayerDied);
    }

    public override void Exit() {
        GameEvents.OnPlayerDied.RemoveListener(HandlePlayerDied);

        WorldTypeManager.Instance.StopClock();

        UIManager.Instance.GetPanel<ScorePanel>().Hide();
        UIManager.Instance.GetPanel<VisualSwitchPanel>().Hide();
    }

    public override void Tick() { }

    private void HandlePlayerDied() {
        GameStateMachine.Instance.StopGame();
    }
}
