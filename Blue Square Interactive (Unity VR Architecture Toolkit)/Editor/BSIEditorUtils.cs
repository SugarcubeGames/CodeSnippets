using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class BSIEditorUtils {

    public static Texture2D newTex = null;
    public static Texture2D saveTex = null;
    public static Texture2D openTex = null;

    public static ScriptableObject LoadScriptableObject(string path = "")
    {
        ScriptableObject so = null; 
        string pathToSO = EditorUtility.OpenFilePanel("Load Scriptable Object:", path, "");
        if (pathToSO.StartsWith(Application.dataPath))
        {
            pathToSO = "Assets" + pathToSO.Substring(Application.dataPath.Length);
        }

        so = AssetDatabase.LoadAssetAtPath(pathToSO, typeof(ScriptableObject)) as ScriptableObject;

        return so;
    }

    public static string SaveScriptableObject(ScriptableObject so, string path = "")
    {
        if(path == "")
        {
            path = EditorUtility.SaveFilePanel("Save ScriptableObject to: ", path, "", ".asset");

            //Reduce the returned path down to a realtive application path
            if (path.StartsWith(Application.dataPath))
            {
                path = "Assets" + path.Substring(Application.dataPath.Length);
            }

            AssetDatabase.CreateAsset(so, path);
            AssetDatabase.SaveAssets();
        } else
        {
            AssetDatabase.SaveAssets();
        }

        return path;

    }


    /***********************************************************************************************
     * Custom Editor utilities
     * *********************************************************************************************/
    
    public static void DrawTopToolbar()
    {
        //Make sure the textures are loaded
        //if (newTex == null) newTex = AssetDatabase.LoadAssetAtPath("Assets/Resources/Textures/Editor/tex_Editor_NewSO.png", typeof(Texture2D)) as Texture2D;
        //if (saveTex == null) saveTex = AssetDatabase.LoadAssetAtPath("Assets/Resources/Textures/Editor/tex_Editor_SaveSO.png", typeof(Texture2D)) as Texture2D;
        //if (openTex == null) openTex = AssetDatabase.LoadAssetAtPath("Assets/Resources/Textures/Editor/tex_Editor_OpenO.png", typeof(Texture2D)) as Texture2D;
        if (newTex == null) newTex = Resources.Load("Textures/Editor/tex_Editor_NewSO", typeof(Texture2D)) as Texture2D;
        if (saveTex == null) saveTex = Resources.Load("Textures/Editor/tex_Editor_SaveSO", typeof(Texture2D)) as Texture2D;
        if (openTex == null) openTex = Resources.Load("Textures/Editor/tex_Editor_OpenSO", typeof(Texture2D)) as Texture2D;

        //Debug.Log(newTex.name + "\n" + openTex.name + "\n" + saveTex.name);

        EditorGUILayout.BeginHorizontal(EditorStyles.helpBox, GUILayout.ExpandWidth(true));

        if(GUILayout.Button(newTex, EditorStyles.label, GUILayout.Width(32), GUILayout.Height(32)))
        {
            Debug.Log("New");
        }
        if (GUILayout.Button(saveTex, EditorStyles.label, GUILayout.Width(32), GUILayout.Height(32)))
        {
            Debug.Log("Save");
        }
        if (GUILayout.Button(openTex, EditorStyles.label, GUILayout.Width(32), GUILayout.Height(32)))
        {
            Debug.Log("Open");
        }
        EditorGUILayout.EndHorizontal();
    }

    /****************************************************
     * GUI Utilities
     * **************************************************/

    public static bool DrawCenteredButtonHorz(string txt, params GUILayoutOption[] options)
    {
        bool hasPressed = false;
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        if (GUILayout.Button(txt, options))
        {
            hasPressed = true;
        }
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();

        return hasPressed;
    }

    public static bool DrawCenteredButtonHorz(string txt, GUIStyle style, params GUILayoutOption[] options)
    {
        bool hasPressed = false;
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        if (GUILayout.Button(txt, style, options))
        {
            hasPressed = true;
        }
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();

        return hasPressed;
    }

    public static void DrawHorizontalDivider()
    {
        GUILayout.BeginHorizontal();
        GUILayout.Box("", GUILayout.ExpandWidth(true), GUILayout.Height(1));
        GUILayout.EndHorizontal();
    }

    public static void DrawDoubleHorizontalDivider(int spacing = 4)
    {
        DrawHorizontalDivider();
        GUILayout.Space(spacing);
        DrawHorizontalDivider();
    }


    public static void DrawCenteredHorizontalDivider(int length = 100)
    {
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        GUILayout.Box("", GUILayout.Width(length), GUILayout.Height(1));
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
    }

    public static void DrawDoubleCenteredHorizontalDivider(int spacing = 4, int length = 100)
    {
        DrawCenteredHorizontalDivider(length);
        GUILayout.Space(spacing);
        DrawCenteredHorizontalDivider(length);
    }
}
