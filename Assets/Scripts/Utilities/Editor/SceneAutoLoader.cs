using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEditor;

[InitializeOnLoad]
public static class SceneAutoLoader {

    private static string prefKeyLoadMasterOnPlay => $"SceneAutoLoader.LoadMasterOnPlay.{Application.productName}";
    private static string prefKeyMasterScene => $"SceneAutoLoader.MasterScene.{Application.productName}";
    private static string prefKeyPreviousScene => $"SceneAutoLoader.PreviousScene.{Application.productName}";

    private static bool LoadMasterOnPlay {
        get { return EditorPrefs.GetBool(prefKeyLoadMasterOnPlay, false); }
        set { EditorPrefs.SetBool(prefKeyLoadMasterOnPlay, value); }
    }

    private static string MasterScene {
        get { return EditorPrefs.GetString(prefKeyMasterScene, "Master.unity"); }
        set { EditorPrefs.SetString(prefKeyMasterScene, value); }
    }

    private static string PreviousScene {
        get { return EditorPrefs.GetString(prefKeyPreviousScene, EditorSceneManager.GetActiveScene().path); }
        set { EditorPrefs.SetString(prefKeyPreviousScene, value); }
    }

    static SceneAutoLoader() {
        EditorApplication.playModeStateChanged += OnPlayModeChanged;
    }

    [MenuItem("Tools/Scene Autoload/Select Master Scene...")]
    private static void SelectMasterScene() {
        string masterScene = EditorUtility.OpenFilePanel("Select Master Scene", Application.dataPath, "unity");
        masterScene = masterScene.Replace(Application.dataPath, "Assets");
        if (!string.IsNullOrEmpty(masterScene)) {
            MasterScene = masterScene;
            LoadMasterOnPlay = true;
        }
    }

    [MenuItem("Tools/Scene Autoload/Load Master On Play", true)]
    private static bool ShowLoadMasterOnPlay() {
        return !LoadMasterOnPlay;
    }
    [MenuItem("Tools/Scene Autoload/Load Master On Play")]
    private static void EnableLoadMasterOnPlay() {
        LoadMasterOnPlay = true;
    }

    [MenuItem("Tools/Scene Autoload/Don't Load Master On Play", true)]
    private static bool ShowDontLoadMasterOnPlay() {
        return LoadMasterOnPlay;
    }
    [MenuItem("Tools/Scene Autoload/Don't Load Master On Play")]
    private static void DisableLoadMasterOnPlay() {
        LoadMasterOnPlay = false;
    }

    private static void OnPlayModeChanged(PlayModeStateChange state) {
        if (!LoadMasterOnPlay) { return; }

        if (!EditorApplication.isPlaying && EditorApplication.isPlayingOrWillChangePlaymode) {
            PreviousScene = SceneManager.GetActiveScene().path;
            if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo()) {
                try {
                    EditorSceneManager.OpenScene(MasterScene);
                } catch {
                    Debug.LogError($"Error: scene not found: {MasterScene}");
                    EditorApplication.isPlaying = false;
                }
            } else {
                EditorApplication.isPlaying = false;
            }
        }

        if (!EditorApplication.isPlaying && !EditorApplication.isPlayingOrWillChangePlaymode) {
            try {
                EditorSceneManager.OpenScene(PreviousScene);
            } catch {
                Debug.LogError($"Error: scene not found: {PreviousScene}");
            }
        }
    }
}
