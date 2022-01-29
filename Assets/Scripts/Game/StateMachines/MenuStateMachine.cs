using UnityEngine;
using UnityEngine.UI;

public class MenuStateMachine : MonoStateMachineSingleton<MenuStateMachine> {

    [Space]
    [SerializeField] private Button playButton;
    [SerializeField] private Button quitButton;

    public override void Enter(params object[] data) {
        base.Enter(data);
        playButton.onClick.AddListener(HandlePlayButtonClicked);
        quitButton.onClick.AddListener(HandleQuitButtonClicked);
        HandlePlayButtonClicked();
    }

    public override void Exit() {
        base.Exit();
        playButton.onClick.RemoveListener(HandlePlayButtonClicked);
        quitButton.onClick.RemoveListener(HandleQuitButtonClicked);
    }

    private void HandlePlayButtonClicked() {
        Game.Instance.LoadGame();
    }

    private void HandleQuitButtonClicked() {
        Game.Instance.QuitGame();
    }
}
