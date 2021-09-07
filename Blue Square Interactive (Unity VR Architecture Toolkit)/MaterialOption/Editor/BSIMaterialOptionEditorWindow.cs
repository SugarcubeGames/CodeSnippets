using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

public class BSIMaterialOptionEditorWindow : EditorWindow{

    /// <summary>
    /// Reference to the BSIMaterialOptionManager
    /// </summary>
    private BSIMaterialOptionManager manager;

    /// <summary>
    /// Index of the Option we are currently editing
    /// </summary>
    private int activeIndex = 0;

    /// <summary>
    /// Active index we're switching to.
    /// </summary>
    private int newActiveindex = -1;


    /// <summary>
    /// position of the scrollview window for displaying current option materials
    /// </summary>
    private Vector2 optionScrollViewPos;


    /// <summary>
    /// Material being added to active option
    /// </summary>
    private Material matToAdd;
    /// <summary>
    /// Material being removed from the active option
    /// </summary>
    private Material matToRemove;

    //Track materials that are being reordered.
    private Material matToReorderUp;
    private Material matToReorderDown;

    //Track Material Option being removed
    private BSIMaterialOption optionToRemove;

    /// <summary>
    /// Track if the manager asset needs to be saved
    /// </summary>
    private bool managerHasBeenUpdated;

	[MenuItem("Blue Square Interactive/Material Option Editor", false, 41)]
    static void init()
    {
        BSIMaterialOptionEditorWindow window = (BSIMaterialOptionEditorWindow)
            EditorWindow.GetWindow(typeof(BSIMaterialOptionEditorWindow));
        window.ShowAuxWindow();
        window.Init();

        window.minSize = new Vector2(100.0f, 100.0f);
    }

    [MenuItem("Blue Square Interactive/Create Material Option Manager", false, 21)]
    static void createNew()
    {
        //Try to find a manager first, if one exists, this overwrites it.
        BSIMaterialOptionManager curManager = AssetDatabase.LoadAssetAtPath(BSIMaterialOptionManager.managerPath, typeof(BSIMaterialOptionManager)) as BSIMaterialOptionManager;
        if(curManager == null)
        {
            BSIMaterialOptionManager newManager = ScriptableObject.CreateInstance<BSIMaterialOptionManager>();
            newManager.init();
            EditorUtility.SetDirty(newManager);

            AssetDatabase.CreateAsset(newManager, BSIMaterialOptionManager.managerPath);
            AssetDatabase.SaveAssets();
        } else
        {
            EditorUtility.DisplayDialog("Material Option Manager already exists", "The material option manager already exists.", "OK");
        }
    }

    public void Init()
    {
        manager = BSIMaterialOptionManager.Instance;
        optionScrollViewPos = new Vector2(0, 0);

        managerHasBeenUpdated = false;

        optionToRemove = null;
    }

    public void OnGUI()
    {
        //No longer need this, as there is only one manager which
        //stores all relevant data.
        //BSIEditorUtils.DrawTopToolbar();

        //Check for changes
        EditorGUI.BeginChangeCheck();

        //Sidebar and main window
        EditorGUILayout.BeginHorizontal();

        //Horizontal sidebar lists all material options in the manager.
        //New button at the bottom.
        EditorGUILayout.BeginVertical(EditorStyles.helpBox, GUILayout.Width(150), GUILayout.ExpandHeight(true));

        for(int i = 0; i<manager.Options.Count; i++)
        {
            if(i == activeIndex)
            {
                EditorGUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                EditorGUILayout.LabelField(manager.Options[i].Name, EditorStyles.objectField, GUILayout.Width(130));
                GUILayout.FlexibleSpace();
                EditorGUILayout.EndHorizontal();
            } else
            {
                if (BSIEditorUtils.DrawCenteredButtonHorz(manager.Options[i].Name, EditorStyles.helpBox, GUILayout.Width(130)))
                {
                    newActiveindex = i;
                }
            }
        }

        GUILayout.FlexibleSpace();

        if (GUILayout.Button("Add Option", GUILayout.Width(140)))
        {
            BSIMaterialOptionNewOptionEditorWindow.ShowNewMaterialOptionWindow(manager);
            managerHasBeenUpdated = true;
        }
        if(manager.Options.Count > 0)
        {
            if (GUILayout.Button("Remove Active Option", GUILayout.Width(140)))
            {
                Debug.Log("Remove action option button pressed");
                //Make sure the user actually wants to delete the option.
                if(EditorUtility.DisplayDialog("Remove Option", "Are you sure you want to delete \'" + manager.Options[activeIndex].Name + "\'?\nThis action cannot be undone.", "Yes", "No"))
                {
                    optionToRemove = manager.Options[activeIndex];
                }
            }
        }

        EditorGUILayout.EndVertical();
        //End sidebar
        if (manager.Options.Count == 0) return;

        if (activeIndex < 0) activeIndex = 0;
        if (activeIndex > manager.Options.Count - 1) activeIndex = manager.Options.Count - 1;

        EditorGUILayout.BeginVertical(EditorStyles.helpBox, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));

        //Material option header.  Gives name and tell user location of option
        EditorGUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        EditorGUILayout.LabelField("Materials in: " + manager.Options[activeIndex].Name + "  (" + manager.Options[activeIndex].Materials.Count + ")", GUILayout.Width(195));
        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        EditorGUILayout.LabelField("Material location: Assets/Resources/Materials/Material Options/", GUILayout.Width(365));
        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        EditorGUILayout.LabelField(manager.Options[activeIndex].MaterialName, GUILayout.ExpandWidth(true));
        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();

        BSIEditorUtils.DrawDoubleHorizontalDivider(2);

        //Scroll view
        optionScrollViewPos = EditorGUILayout.BeginScrollView(optionScrollViewPos, GUILayout.ExpandWidth(true));

        //Draw each material in the option.
        if(manager.Options[activeIndex].Materials.Count > 0)
        {
            //if (activeIndex < 0) activeIndex = 0;
            //if (activeIndex > manager.Options.Count - 1) activeIndex = manager.Options.Count-1;

            for (int i = 0; i < manager.Options[activeIndex].Materials.Count; i++)
            {
                //Debug.Log(i);
                drawMaterial(i);
            }
        }

        EditorGUILayout.EndScrollView();

        //Add Material button
        EditorGUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("Add Material", GUILayout.Width(100)))
        {
            EditorGUIUtility.ShowObjectPicker<UnityEngine.Material>(null, false, "", 1000);
        }
        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.EndVertical();

        EditorGUILayout.EndHorizontal();
        //End Sidebar and Main Window

        if (EditorGUI.EndChangeCheck())
        {
            EditorUtility.SetDirty(manager);
            AssetDatabase.SaveAssets();
        }

        if (Event.current.commandName == "ObjectSelectorClosed")
        {
            matToAdd = EditorGUIUtility.GetObjectPickerObject() as Material;
            managerHasBeenUpdated = true;

            //Refresh the window so it displays the new material automatically
            Repaint();
        }
    }

    //Any option manipulation needs to take place here
    //to prevent ui modification errors
    private void Update()
    {
        if(matToAdd != null)
        {
            manager.Options[activeIndex].AddMaterial(matToAdd);
            matToAdd = null;
            EditorUtility.SetDirty(manager);
            managerHasBeenUpdated = true;
        }

        if(matToRemove != null)
        {
            manager.Options[activeIndex].RemoveMaterial(matToRemove);
            matToRemove = null;
            EditorUtility.SetDirty(manager);
            managerHasBeenUpdated = true;
        }

        if(newActiveindex != -1)
        {
            activeIndex = newActiveindex;
            newActiveindex = -1;
            EditorUtility.SetDirty(manager);
            managerHasBeenUpdated = true;
        }

        if(matToReorderUp != null)
        {
            try
            {
                int matIndex = manager.Options[activeIndex].GetMaterialIndex(matToReorderUp);
                manager.Options[activeIndex].reorderMaterial(matIndex, matIndex - 1);
            } catch (Exception ex)
            {
                Debug.LogError(ex.Message);
            }

            matToReorderUp = null;
            EditorUtility.SetDirty(manager);
            managerHasBeenUpdated = true;
        }

        if (matToReorderDown != null)
        {
            try
            {
                int matIndex = manager.Options[activeIndex].GetMaterialIndex(matToReorderDown);
                manager.Options[activeIndex].reorderMaterial(matIndex, matIndex + 1);
            }
            catch (Exception ex)
            {
                Debug.LogError(ex.Message);
            }

            matToReorderDown = null;
            EditorUtility.SetDirty(manager);
            managerHasBeenUpdated = true;
        }

        if(optionToRemove != null)
        {
            //I'm having a weird issue where every time the code recompiles,
            //the first option in the manager is being deleted...  No clue why.
            if (optionToRemove.Name == "") return;
            
            AssetDatabase.DeleteAsset(manager.Options[activeIndex].MaterialPath);
            manager.RemoveMaterialOption(manager.Options[activeIndex].Name);

            managerHasBeenUpdated = true;
            optionToRemove = null;

            if (activeIndex == manager.Options.Count)
            {
                activeIndex--;
            }
        }

        if (managerHasBeenUpdated)
        {
            AssetDatabase.SaveAssets();
            managerHasBeenUpdated = false;
        }
    }


    //Draw the material
    private void drawMaterial(int index)
    {
        Material curMat = manager.Options[activeIndex].Materials[index];
        EditorGUILayout.BeginHorizontal();

        EditorGUILayout.BeginHorizontal(EditorStyles.helpBox, GUILayout.ExpandWidth(true), GUILayout.Height(64));

        GUILayout.Box(AssetPreview.GetAssetPreview(curMat), GUILayout.Width(75), GUILayout.Height(75));

        EditorGUILayout.BeginVertical();
        EditorGUILayout.LabelField(curMat.name);
        GUILayout.FlexibleSpace();
        if(GUILayout.Button("Set as Active", GUILayout.Width(100))){
            //Debug.Log("Setting " + curMat.name + " as active material in option: " + manager.Options[activeIndex].Name);
            manager.Options[activeIndex].updateMaterial(curMat);
            //Without this SetDirty call, the material will revert to its basic state after
            //the editor is closed.
            EditorUtility.SetDirty(manager.Options[activeIndex].Material);
        }
        if (GUILayout.Button("Remove", GUILayout.Width(100))){
            //Debug.Log("Removing " + curMat.name + " from option: " + manager.Options[activeIndex].Name);
            matToRemove = curMat;
        }
        EditorGUILayout.EndVertical();

        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal(EditorStyles.helpBox, GUILayout.Width(80), GUILayout.Height(80));
        EditorGUILayout.BeginVertical();
        GUILayout.FlexibleSpace();

        int matIndex = manager.Options[activeIndex].GetMaterialIndex(curMat);
        if(matIndex > 0)
        {
            if (GUILayout.Button("Move Up", GUILayout.Width(77)))
            {
                matToReorderUp = curMat;
            }
        }
        if(matIndex < manager.Options[activeIndex].Materials.Count-1)
        {
            if (GUILayout.Button("Move Down", GUILayout.Width(77)))
            {
                matToReorderDown = curMat;
            }
        }
        GUILayout.FlexibleSpace();
        EditorGUILayout.EndVertical();
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.EndHorizontal();
    }
}
