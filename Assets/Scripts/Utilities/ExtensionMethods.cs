using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class ExtensionMethods {

    public static Vector3 ToFlatVector3(this Vector2 v2) {
        return new Vector3(v2.x, 0f, v2.y);
    }

    public static Vector3 ToFlatVector3(this Vector3 v3) {
        return new Vector3(v3.x, 0f, v3.z);
    }

    public static T Clone<T>(this T so) where T : ScriptableObject {
        T clone = Object.Instantiate(so);
        clone.name = so.name;
        return clone;
    }

    public static void TryStopRoutine(this MonoBehaviour behaviour, ref Coroutine routine) {
        if (routine == null) { return; }
        behaviour.StopCoroutine(routine);
        routine = null;
    }

    public static T Random<T>(this IList<T> list) {
        return list[UnityEngine.Random.Range(0, list.Count)];
    }

    public static T Random<T>(this IEnumerable<T> enumerable) {
        int count = enumerable.Count();
        if (count == 0) { return default; }
        return enumerable.ElementAt(UnityEngine.Random.Range(0, count));
    }

    public static Transform GetRandomChild(this Transform t) {
        if (t.childCount <= 0) { return null; }
        return t.GetChild(UnityEngine.Random.Range(0, t.childCount));
    }

    public static void ResetTransform(this Transform t) {
        t.localPosition = Vector3.zero;
        t.localScale = Vector3.one;
        t.localEulerAngles = Vector3.zero;
    }

    public static float Remap(this float value, float from1, float to1, float from2, float to2) {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }

    public static void Shuffle<T>(this IList<T> list) {
        int n = list.Count;
        while (n > 1) {
            n--;
            int k = UnityEngine.Random.Range(0, n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

    public static void ToggleInteraction(this CanvasGroup canvasGroup, bool state) {
        canvasGroup.interactable = state;
        canvasGroup.blocksRaycasts = state;
    }

    public static bool Contains(this Vector2 bounds, Vector3 position) {
        float halfX = bounds.x / 2;
        float halfY = bounds.y / 2;
        return position.x >= -halfX
            && position.x <= halfX
            && position.z >= -halfY
            && position.z <= halfY;
    }
}
