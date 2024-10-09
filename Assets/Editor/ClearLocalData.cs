using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public class ClearLocalData {
    [MenuItem("Tools/Clear Local Data")]
    public static void ClearPlayerPrefs() {
        if (EditorUtility.DisplayDialog("Clear Local Data", "Are you sure you want to clear all local data?", "Yes", "No")) {
            PlayerPrefs.DeleteAll();
            PlayerPrefs.Save();
            Debug.Log("All local data has been cleared.");
        }
    }

    public static bool mToggle = true;


    [MenuItem("BuildTools/PlayModeUseFirstScene")]
    static void UpdatePlayModeUseFirstScene() {
        mToggle = !mToggle;
        if (mToggle) {
            EditorSceneManager.playModeStartScene = null;
            Debug.Log("PlayModeUseFirstScene is enabled.");
        }
        else {
            SceneAsset scene = AssetDatabase.LoadAssetAtPath<SceneAsset>(EditorBuildSettings.scenes[0].path);
            EditorSceneManager.playModeStartScene = scene;
            Debug.Log("PlayModeUseFirstScene is disabled.");
        }
    }
}
