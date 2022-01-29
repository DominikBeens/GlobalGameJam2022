using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Collections;
using DG.Tweening;
using System.Linq;
using UnityEngine;

public class SceneLoadManager : Manager<SceneLoadManager> {

    private const string CORE_SCENE = "Boot";

    [SerializeField] private CanvasGroup canvasGroup;

    private Coroutine loadRoutine;

    public override void Initialize() {
        base.Initialize();
        canvasGroup.gameObject.SetActive(true);
    }

    public Coroutine Load(string scene) {
        if (loadRoutine != null) { return null; }
        loadRoutine = StartCoroutine(LoadRoutine(scene));
        return loadRoutine;
    }

    public Coroutine FadeIn(float delay = 0f) {
        return StartCoroutine(FadeCanvasGroup(1f, delay));
    }

    public Coroutine FadeOut() {
        return StartCoroutine(FadeCanvasGroup(0f));
    }

    private IEnumerator LoadRoutine(string scene) {
        yield return UnloadAllScenesExcept(CORE_SCENE);
        yield return LoadScene(scene, true);
        yield return null;
        loadRoutine = null;
    }

    private IEnumerator LoadScene(string scene, bool setAsActiveScene) {
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive);
        while (loadOperation.isDone == false) {
            yield return null;
        }
        if (setAsActiveScene) {
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(scene));
        }
    }

    private IEnumerator UnloadScene(string scene) {
        AsyncOperation loadOperation = SceneManager.UnloadSceneAsync(scene);
        while (loadOperation.isDone == false) {
            yield return null;
        }
    }

    private IEnumerator UnloadAllScenesExcept(params string[] scenes) {
        List<Scene> loadedScenes = new List<Scene>();
        for (int i = 0; i < SceneManager.sceneCount; i++) {
            Scene scene = SceneManager.GetSceneAt(i);
            if (scenes.Contains(scene.name) == false) {
                loadedScenes.Add(scene);
            }
        }

        foreach (Scene scene in loadedScenes) {
            yield return UnloadScene(scene.name);
        }
    }

    private IEnumerator FadeCanvasGroup(float value, float delay = 0f) {
        if (value == canvasGroup.alpha) { yield break; }

        if (value == 1f) {
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
        }

        if (delay > 0f) {
            yield return new WaitForSeconds(delay);
        }
        yield return canvasGroup.DOFade(value, 0.1f).WaitForCompletion();

        if (value == 0f) {
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
        }
    }
}
