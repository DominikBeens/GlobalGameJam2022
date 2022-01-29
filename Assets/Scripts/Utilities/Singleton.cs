using UnityEngine;
using System.Linq;

public abstract class ScriptableObjectSingleton<T> : ScriptableObject where T : ScriptableObject {

    private static T instance;

    public static T Instance {
        get {
            if (!instance) {
                Resources.LoadAll<T>("");
                instance = Resources.FindObjectsOfTypeAll<T>().FirstOrDefault();
                if (!instance) {
                    Debug.LogError($"No scriptable object instance of type {typeof(T)} has been found! Try calling {typeof(T)}.Exists first.");
                }
            }
            return instance;
        }
    }
}

public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour {

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
