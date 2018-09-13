using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

// #代表Shift，%代表Ctrl，&代表Alt
public class EditorUtil
{
    [MenuItem("Tools/Start Launcher %G")] //Ctrl+G  
    public static void StartLauncher()
    {
        //if (EditorUtility.DisplayDialog("Start game",
        //    "Do you want to run game anyway? \n\nAll unsaved datas in current scene will be LOST!!!!!!", "GoGoGo!!!", "No") == false)
        //{
        //    return;
        //}

        EditorApplication.isPaused = false;
        EditorApplication.isPlaying = false;
        EditorSceneManager.OpenScene("Assets/Scenes/Launcher.unity");
    }

    [MenuItem("Tools/Start Test %T")]
    public static void StartTest()
    {
        EditorApplication.isPaused = false;
        EditorApplication.isPlaying = false;
        EditorSceneManager.OpenScene("Assets/Test/Scenes/Test.unity");
    }
}
