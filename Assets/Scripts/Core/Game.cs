using System.Collections;
using UnityEngine;
using System;
using DB.SimpleFramework.SimpleAudioManager;

public class Game : Singleton<Game> {

    public enum GameScene { Undefined, Menu, Game }

    public static GameEvent OnGameLoadingStarted = new GameEvent();
    public static GameEvent OnGameLoadingEnded = new GameEvent();

    public GameScene ActiveGameScene { get; private set; }

    private IManager[] managers;
    private bool isLoading;
    private bool isQuitting;

    private void Awake() {
        StartCoroutine(InitializeRoutine());
    }

    private void Update() {
        for (int i = managers.Length - 1; i >= 0; i--) {
            managers[i].Tick();
        }
    }

    private void LateUpdate() {
        for (int i = managers.Length - 1; i >= 0; i--) {
            managers[i].LateTick();
        }
    }

    private void OnDestroy() {
#if !UNITY_EDITOR
        Deinitialize();
#endif
    }

    private IEnumerator InitializeRoutine() {
        managers = GetComponentsInChildren<IManager>();
        foreach (IManager manager in managers) {
            manager.Initialize();
        }

        yield return null;
        SimpleAudioManager.Initialize();
        yield return null;

        yield return SceneLoadManager.Instance.FadeIn();
        yield return SceneLoadManager.Instance.Load(GameScene.Menu.ToString());

        MenuStateMachine.Instance.EnterState<MenuStateMachine>();
        ActiveGameScene = GameScene.Menu;

        yield return SceneLoadManager.Instance.FadeOut();
    }

    private void Deinitialize() {
        foreach (MonoStateMachineSingleton<MonoStateMachine> stateMachine in FindObjectsOfType<MonoStateMachineSingleton<MonoStateMachine>>()) {
            stateMachine.Deinitialize();
        }
        foreach (IManager manager in managers) {
            manager.Deinitialize();
        }
    }

    public void LoadMenu() {
        if (isLoading) { return; }
        StartCoroutine(LoadRoutine(GameScene.Menu.ToString(), () => {
            MenuStateMachine.Instance.EnterState<MenuStateMachine>();
            ActiveGameScene = GameScene.Menu;
        }));
    }

    public void LoadGame() {
        if (isLoading) { return; }
        StartCoroutine(LoadRoutine(GameScene.Game.ToString(), () => {
            GameStateMachine.Instance.EnterState<GameStateMachine>();
            ActiveGameScene = GameScene.Game;
        }));
    }

    public void RestartGame() {
        LoadGame();
    }

    public void QuitGame() {
        if (isQuitting) { return; }
        isQuitting = true;
        StartCoroutine(QuitRoutine());
    }

    private IEnumerator LoadRoutine(string scene, Action onLoadComplete = null) {
        isLoading = true;
        Time.timeScale = 1f;

        yield return SceneLoadManager.Instance.FadeIn(0.2f);
        yield return new WaitForSeconds(0.2f);

        OnGameLoadingStarted.Invoke();

        switch (ActiveGameScene) {
            case GameScene.Menu:
                MenuStateMachine.Instance.Deinitialize();
                break;
            case GameScene.Game:
                GameStateMachine.Instance.Deinitialize();
                break;
        }

        yield return SceneLoadManager.Instance.Load(scene);
        onLoadComplete?.Invoke();
        OnGameLoadingEnded.Invoke();
        yield return SceneLoadManager.Instance.FadeOut();

        isLoading = false;
    }

    private IEnumerator QuitRoutine() {
        yield return SceneLoadManager.Instance.FadeIn(0.1f);
        Deinitialize();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }
}
