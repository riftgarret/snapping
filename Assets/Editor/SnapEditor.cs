﻿/**
 *	@sixbyseven studio
 *	7/8/2013
 */
#if UNITY_4_3 || UNITY_4_3_0 || UNITY_4_3_1 || UNITY_4_3_2 || UNITY_4_3_3 || UNITY_4_3_4 || UNITY_4_3_5
#define UNITY_4_3
#elif UNITY_4_0 || UNITY_4_0_1 || UNITY_4_1 || UNITY_4_2 || UNITY_4_3 || UNITY_4_3_0 || UNITY_4_3_1 || UNITY_4_3_2 || UNITY_4_3_3 || UNITY_4_3_4 || UNITY_4_3_5
#define UNITY_4
#elif UNITY_3_0 || UNITY_3_0_0 || UNITY_3_1 || UNITY_3_2 || UNITY_3_3 || UNITY_3_4 || UNITY_3_5
#define UNITY_3
#endif

#define PRO

using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Reflection;
using snaptest;


[System.Serializable]
public class SnapEditor : EditorWindow
{

    private GUIContent gc_SnapEnabled = new GUIContent("Enable", "Toggles snapping on or off.");
    private GUIContent gc_RunTests = new GUIContent("Run Tests", "Run unit tests.");

    [MenuItem("Tools/Modular Snap/Snap Window", false, 15)]
    public static void InitSnapWindow()
    {
        EditorWindow.GetWindow(typeof(SnapEditor), false, "SM", true).autoRepaintOnSceneChange = true;
        SceneView.RepaintAll();

    }


    void OnGUI()
    {
        EditorGUILayout.BeginVertical();

        bool enable = EditorGUILayout.Toggle(gc_SnapEnabled, SnapEngine.Instance.IsEnabled);
        SnapEngine.Instance.IsEnabled = enable;

        if (GUILayout.Button(gc_RunTests))
        {
            SnapTestSuite test = new SnapTestSuite();
            test.testAll();
        }

        EditorGUILayout.EndVertical();
    }


}