using UnityEngine;

public abstract class MonoStateMachineSingleton<T> : MonoStateMachine where T : MonoStateMachine {

    private static T instance;

    public static T Instance {
        get {
            if (!instance) {
                instance = FindObjectOfType<T>();
                if (!instance) {
                    Debug.LogError($"No singleton instance of type {typeof(T)} has been found! Try calling {typeof(T)}.Exists first.");
                }
            }
            return instance;
        }
    }

    public static bool Exists {
        get {
            if (!instance) {
                instance = FindObjectOfType<T>();
            }
            return instance != null;
        }
    }
}
