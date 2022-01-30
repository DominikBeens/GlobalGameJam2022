using UnityEngine;

public class MenuStateMachine : MonoStateMachineSingleton<MenuStateMachine> {

    [Space]
    [SerializeField] private Canvas canvas;
    [Space]
    [SerializeField] private UIButton playButton;
    [SerializeField] private UIButton quitButton;

    public override void Enter(params object[] data) {
        base.Enter(data);
        canvas.worldCamera = PlayerCameraManager.Instance.Camera;
        playButton.AddListener(HandlePlayButtonClicked);
        quitButton.AddListener(HandleQuitButtonClicked);
    }

    public override void Exit() {
        base.Exit();
        playButton.RemoveListener(HandlePlayButtonClicked);
        quitButton.RemoveListener(HandleQuitButtonClicked);
    }

    private void HandlePlayButtonClicked() {
        Game.Instance.LoadGame();
        ToggleInteractions(false);
    }

    private void HandleQuitButtonClicked() {
        Game.Instance.QuitGame();
        ToggleInteractions(false);
    }

    private void ToggleInteractions(bool state) {
        playButton.ToggleInteraction(state);
        quitButton.ToggleInteraction(state);
    }
}
