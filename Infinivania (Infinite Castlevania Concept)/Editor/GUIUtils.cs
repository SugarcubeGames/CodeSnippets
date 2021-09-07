using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class GUIUtils {
    
    public static void DrawHorizontalDivider()
    {
        GUILayout.BeginHorizontal();
        GUILayout.Box("", GUILayout.ExpandWidth(true), GUILayout.Height(1));
        GUILayout.EndHorizontal();
    }

    public static void DrawCenteredText(string text)
    {
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        GUILayout.Label(text);
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
    }

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

    public static void DrawTilesetSpritePreview(Sprite s, int width, int height)
    {
        Texture2D t = AssetPreview.GetAssetPreview(s);
        //if(t.width != width && t.height != height)
        //{
        //    TextureScale.Bilinear(t, width, height);
        //}

        GUILayout.BeginHorizontal(EditorStyles.helpBox, GUILayout.Height(height));
        GUILayout.FlexibleSpace();
        GUILayout.Label(t, GUILayout.Width(width+4), GUILayout.Height(height+4));
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();

    }
}
