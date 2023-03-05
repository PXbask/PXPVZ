using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

public class ProjectTools
{
    [MenuItem("Tools/StartTest")]
    public static void StartTest()
    {
        if (EditorApplication.isPlaying)
        {
            EditorApplication.ExitPlaymode();
        }
        if(EditorSceneManager.GetActiveScene().isDirty)
        {
            EditorUtility.DisplayDialog("", "请先保存当前场景", "ok");
        }
        EditorSceneManager.OpenScene("Assets/Scenes/start.unity",OpenSceneMode.Single);
        EditorApplication.EnterPlaymode();
    }
}
