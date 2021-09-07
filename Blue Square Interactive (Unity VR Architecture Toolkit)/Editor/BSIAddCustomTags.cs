using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class BSIAddCustomTags {

    //A List of all custom tags that will be needed for Gameobject Classification.
    //This is used by the "BSIAddCustomTags" script to add custom tags
    //that are needed for other scripts to work properly.
    public static List<string> tagStrings = new List<string>()
    {
        BSITagsLayers.featurePoint,
        //Certain menus close on contact with trigger.  Add any object we don't
        //want to close menus to this tag.
        BSITagsLayers.ignoreCloseMenuTrigger,
        BSITagsLayers.ignoreBSIInteractibles,
    };

    [MenuItem("Blue Square Interactive/--Add Custom Tags--", false, 01)]
    public static void AddCustomTags()
    {
        //Debug.Log("Add Custom Tags");

        SerializedObject tagManager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
        SerializedProperty tagsProp = tagManager.FindProperty("tags");

        SerializedProperty layersProp = tagManager.FindProperty("layers");

        //Debug.Log(tagStrings.Count);
        foreach (string s in tagStrings)
        {
            tryAddTag(s, tagsProp, layersProp);
        }

        tagManager.ApplyModifiedProperties();

    }

    private static void tryAddTag(string s, SerializedProperty tagsProp, SerializedProperty layersProp)
    {
        //Debug.Log("tryAddTag");
        bool found = false;
        for (int i = 0; i < tagsProp.arraySize; i++)
        {
            SerializedProperty t = tagsProp.GetArrayElementAtIndex(i);
            if (t.stringValue.Equals(s)) { found = true; return; }
        }

        //If not found, add it
        Debug.Log("Adding tag: " + s);
        if (!found)
        {
            tagsProp.InsertArrayElementAtIndex(0);
            SerializedProperty n = tagsProp.GetArrayElementAtIndex(0);
            n.stringValue = s;
        }
    }
}
