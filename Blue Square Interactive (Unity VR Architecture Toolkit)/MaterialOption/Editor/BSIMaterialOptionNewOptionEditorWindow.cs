using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

public class BSIMaterialOptionNewOptionEditorWindow : EditorWindow{

    private static BSIMaterialOptionManager _manager;
    private static string newOptionName;

    public static void ShowNewMaterialOptionWindow(BSIMaterialOptionManager manager)
    {
        BSIMaterialOptionNewOptionEditorWindow window = ScriptableObject.CreateInstance<BSIMaterialOptionNewOptionEditorWindow>();
        window.position = new Rect(Screen.width / 2, Screen.height / 2, 250, 150);
        //window.ShowPopup();
        //window.ShowAuxWindow();
        window.ShowUtility();
        window.minSize = new Vector2(350, 100);

        _manager = manager;
        newOptionName = "";
    }

    private void OnGUI()
    {
        EditorGUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        EditorGUILayout.LabelField("New Material Option name:");
        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();

        newOptionName = EditorGUILayout.TextField("New Option Name: ", newOptionName, GUILayout.ExpandWidth(true));

        GUILayout.Space(10);
        EditorGUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("Add New Option", GUILayout.Width(150)))
        {
            if(newOptionName == "")
            {
                Debug.LogError("Please give the new Material Option a unique name.");
            } else
            {
                if(_manager == null)
                {
                    Debug.LogError("BSIMaterialOptionNewOptionEditorWindow::OnGUI : _manager is null");
                } else
                {
                    //Create a new material
                    Material newMat = new Material(Shader.Find("Standard"));
                    string matName = "mat_BSI_MO_" + newOptionName.Replace(" ", "_") + ".mat";
                    try
                    {
                        AssetDatabase.CreateAsset(newMat, BSIMaterialOptionManager.materialLocationPath + matName);
                    } catch (Exception ex)
                    {
                        Debug.LogError(ex.Message);
                    }
                    _manager.AddMaterialOption(newOptionName, matName, newMat);


                    EditorUtility.SetDirty(_manager);
                    AssetDatabase.SaveAssets();
                }
                this.Close();
            }
        }
        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("Cancel", GUILayout.Width(150)))
        {
            this.Close();
        }
        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();

    }

    
}
