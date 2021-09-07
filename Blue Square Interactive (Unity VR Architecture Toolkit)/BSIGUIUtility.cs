using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BSIGUIUtility{

    /************************************************************
     * GIZMO Utilities
     * **********************************************************/
    
    /// <summary>
    /// Draw a gizmo from a starting point.
    /// </summary>
    /// <param name="sPos">Starting point for arrow</param>
    /// <param name="dir">The direction the arrow will be drawn in</param>
    /// <param name="color">The arrow's color</param>
    /// <param name="arrowHeadLength">How long the arrow head lines will be</param>
    /// <param name="arrowHeadAngle">The angle of the arrow head's lines</param>
    public static void DrawGizmoArrow(Vector3 sPos, Vector3 dir, Color color, float arrowLength = 1.0f, float arrowHeadLength = 0.25f, float arrowHeadAngle = 20f)
    {
        //Debug.Log("drawingGizmo: " + sPos.ToString() + "  | " + (dir*arrowLength));
        Gizmos.color = color;

        Gizmos.DrawLine(sPos, (dir*arrowLength));

        Vector3 right = Quaternion.LookRotation(dir*arrowLength) * Quaternion.Euler(0, 180 + arrowHeadAngle, 0) * new Vector3(0, 0, 1);
        Vector3 left = Quaternion.LookRotation(dir*arrowLength) * Quaternion.Euler(0, 180 - arrowHeadAngle, 0) * new Vector3(0, 0, 1);

        Gizmos.DrawLine(sPos + dir, right * arrowHeadLength);
        Gizmos.DrawLine(sPos + dir, left * arrowHeadLength);
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
