
public class GameStartState : MonoState {

    public override void Enter(params object[] data) {
        PlayerManager.Instance.SpawnPlayer();

        UIManager.Instance.GetPanel<TutorialPanel>().Show();
        UIManager.Instance.GetPanel<TutorialPanel>().OnCloseClicked.AddListener(HandleTutorialPanelCloseClicked);
    }

    public override void Exit() {
        UIManager.Instance.GetPanel<TutorialPanel>().Hide();
        UIManager.Instance.GetPanel<TutorialPanel>().OnCloseClicked.RemoveListener(HandleTutorialPanelCloseClicked);
    }

    public override void Tick() { }

    private void HandleTutorialPanelCloseClicked() {
        GameStateMachine.Instance.EnterState<GamePlayState>();
    }
}
