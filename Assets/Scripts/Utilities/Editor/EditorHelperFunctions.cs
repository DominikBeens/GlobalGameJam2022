using System.Collections.Generic;
using UnityEditor;

public class EditorHelperFunctions {

    public static T[] GetAssetsInFoldersOfType<T>(params string[] folders) where T : UnityEngine.Object {
        List<T> assets = new List<T>();
        string[] guids = AssetDatabase.FindAssets($"t:{typeof(T).Name}", folders);
        foreach (string guid in guids) {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            T asset = AssetDatabase.LoadAssetAtPath<T>(path);
            assets.Add(asset);
        }
        return assets.ToArray();
    }
}
