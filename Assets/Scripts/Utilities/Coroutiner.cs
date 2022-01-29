using System;
using System.Collections;
using UnityEngine;

public class Coroutiner : MonoBehaviour {

    private static Coroutiner instance;

    public static Coroutine Delay(float duration, Action onDelayed, bool unscaled = true) {
        if (duration <= 0) {
            onDelayed?.Invoke();
            return null;
        }
        return GetInstance().StartCoroutine(DelayRoutine(duration, onDelayed, unscaled));
    }

    public static Coroutine DelayFrame(Action onDelayed, int frames = 1) {
        return GetInstance().StartCoroutine(DelayFramesRoutine(1, onDelayed));
    }

    public static Coroutine Start(IEnumerator routine) {
        return GetInstance().StartLocalCoroutine(routine);
    }

    public static void Stop(Coroutine routine) {
        if (routine == null) { return; }
        GetInstance().StopCoroutine(routine);
    }

    private static Coroutiner GetInstance() {
        if (instance == null) {
            GameObject go = new GameObject("Coroutiner");
            instance = go.AddComponent<Coroutiner>();
            DontDestroyOnLoad(instance);
        }
        return instance;
    }

    private static IEnumerator DelayRoutine(float duration, Action onDelayed, bool unscaled) {
        if (unscaled) {
            yield return new WaitForSecondsRealtime(duration);
        } else {
            yield return new WaitForSeconds(duration);
        }

        onDelayed?.Invoke();
    }

    private static IEnumerator DelayFramesRoutine(int frames, Action onDelayed) {
        for (int i = 0; i < frames; i++) {
            yield return null;
        }
        onDelayed?.Invoke();
    }

    private Coroutine StartLocalCoroutine(IEnumerator routine) {
        return StartCoroutine(routine);
    }
}
