using UnityEditor;
using UnityEngine;

public class ClearLocalData
{
    [MenuItem("Tools/Clear Local Data")]
    public static void ClearPlayerPrefs()
    {
        if (EditorUtility.DisplayDialog("Clear Local Data", "Are you sure you want to clear all local data?", "Yes", "No"))
        {
            PlayerPrefs.DeleteAll();
            PlayerPrefs.Save();
            Debug.Log("All local data has been cleared.");
        }
    }
}