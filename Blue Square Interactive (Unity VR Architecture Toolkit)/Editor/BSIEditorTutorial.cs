using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class BSIEditorTutorial : EditorWindow {

    [MenuItem("Blue Square Interactive/--Setup Tutorial | Reminders--", false, 00)]
    public static void showTutorial()
    {
        BSIEditorTutorial window = (BSIEditorTutorial)EditorWindow.GetWindow(typeof(BSIEditorTutorial));

        window.ShowAuxWindow();
    }

    public void OnGUI()
    {
        EditorGUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        EditorGUILayout.LabelField("Before Beginning Development:");
        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();

        BSIGUIUtility.DrawHorizontalDivider();

        EditorGUILayout.LabelField("1 : Add custom tags (Blue Square Interactive / --Add Custom Tags--)", EditorStyles.textArea);

        GUILayout.Space(4);

        EditorGUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        EditorGUILayout.LabelField("During development:");
        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();

        BSIGUIUtility.DrawHorizontalDivider();

        EditorGUILayout.LabelField("1 : If using material options in a scene, be sure to update that scene's references.  (Blue Square Interactive / Update Option References in Scene.)", EditorStyles.textArea);
        EditorGUILayout.LabelField("2 : If using the Realtime Reflection Probe, be sure to enable the BSI_IgnoreInteractible tag so it doesn't interfere with interaction menus.", EditorStyles.textArea);
    }
}
