using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class TileManagerEditorWindow : EditorWindow {

    private TileManager tm;

    /// <summary>
    /// The index of the tile currently being edited.
    /// </summary>
    private int activeTile;
    /// <summary>
    /// Scrollview for scrollable tile list.
    /// </summary>
    private Vector2 listSV;

    /// <summary>
    /// Active tile's sprite
    /// </summary>
    private Sprite atSprite;
    /// <summary>
    /// Active tile's name
    /// </summary>
    private string atName;
    /// <summary>
    /// Active tile's description
    /// </summary>
    private string atDescription;

    /// <summary>
    /// Are we going to delete the active tile?
    /// </summary>
    private bool isDeletingActiveTile = false; //Don't want to do this mid UI, so use a toggle.


    [MenuItem("Infinivania/Tile Manager")]
    static void Init()
    {
        //Get existing open window or, if none, create a new one
        TileManagerEditorWindow window = (TileManagerEditorWindow)
            EditorWindow.GetWindow(typeof(TileManagerEditorWindow));

        window.minSize = new Vector2(100.0f, 100.0f);

        window.init();
    }

    public void init()
    {
        //Get the TileManager isntance
        tm = TileManager.Instance;
        //If this returns null, that means no TileManager isntance exists.
        //Create and save one.
        if(tm == null)
        {
            tm = ScriptableObject.CreateInstance<TileManager>();
            tm.init();
            //Add a tile by default, purely to prevent idnex issues
            tm.AddTile();
            activeTile = 0;
            //Call the save function
            atSprite = tm.Tiles[activeTile].TileSprite;
        }

        listSV = new Vector2(0.0f, 0.0f);
    }

    private void OnGUI()
    {
        //Rather than waiting to save data, this manager will just save continuously, and,
        //since there can only be one copy of the Tile Manager, we don't
        //need a new/open/save toolbar at top.

        //Watch for changes
        EditorGUI.BeginChangeCheck();

        //Screen is divided in two, a scrollview list with all the different tiles, and
        //the information of the active tile.
        EditorGUILayout.BeginHorizontal(GUILayout.ExpandWidth(true));

        //Vertical for all button list functions
        EditorGUILayout.BeginVertical();

        //Vertical for tile Total
        EditorGUILayout.BeginVertical(EditorStyles.helpBox, GUILayout.Width(200));
        GUIUtils.DrawCenteredText("Total Tiles: " + tm.Tiles.Count);
        EditorGUILayout.EndVertical();
        //End vertical for tile Total

        //Vertical for add / delete tiles
        EditorGUILayout.BeginVertical(EditorStyles.helpBox, GUILayout.Width(200));
        if(GUIUtils.DrawCenteredButtonHorz("Add Tile"))
        {
            tm.AddTile();
        }
        //TODO: Add delete functions
        if (GUIUtils.DrawCenteredButtonHorz("Delete Tile"))
        {
            isDeletingActiveTile = true;
        }
        EditorGUILayout.EndVertical();
        //EndVertical for add / delete tiles

        //Scrollview for tile list
        EditorGUILayout.BeginVertical(GUILayout.Width(200), GUILayout.ExpandHeight(true));
        listSV = EditorGUILayout.BeginScrollView(listSV,EditorStyles.helpBox, GUILayout.Width(200));
        for(int i = 0; i<tm.Tiles.Count; i++)
        {
            if (i == activeTile)
            {
                drawActiveTile(tm.Tiles[i]);
            } else
            {
                if (drawInactiveTile(tm.Tiles[i]))
                {
                    //Update the current acitve tile's info
                    UpdateTileInfo(tm.Tiles[activeTile]);
                    activeTile = i;
                    //Get the info for the new active tile
                    UpdateEditorInfo();
                }
            }
        }
        EditorGUILayout.EndScrollView();
        EditorGUILayout.EndVertical();
        //End scrollview for tile list

        EditorGUILayout.EndVertical();
        //EndVertical for all button list functions

        //Vertical for tile data
        EditorGUILayout.BeginVertical();

        //Show the object picker
        atSprite = EditorGUILayout.ObjectField(atSprite, typeof(Sprite), false) as Sprite;
        //Show the icon

        //Name
        atName = EditorGUILayout.TextField(atName);


        EditorGUILayout.EndVertical();
        //End Vertical for Tile Data


        EditorGUILayout.EndHorizontal();
        //End horizontal for tiles list and tile data

        if (EditorGUI.EndChangeCheck())
        {
            UpdateTileInfo(tm.Tiles[activeTile]);
            EditorUtility.SetDirty(tm.Tiles[activeTile]);
        }
        //End watching for changes (need to make this an if statement which calls the save function

        //This is done at the end of the GUI seuqence to prevent an Aout of Bound exception.
        if (isDeletingActiveTile)
        {
            tm.DeleteTile(activeTile);
            isDeletingActiveTile = false;

            //If the tile we just deleted was the last in the list,
            //move the index down by one.
            if(activeTile == tm.Tiles.Count)
            {
                activeTile--;
            }

            //Update Editor infor to match new active tile
            UpdateEditorInfo();
        }
    }

    private void UpdateTileInfo(Tile t)
    {
        t.setSprite(atSprite);
        t.setName(atName);
    }

    private void UpdateEditorInfo()
    {
        atSprite = tm.Tiles[activeTile].TileSprite;
        atName = tm.Tiles[activeTile].TileName;
    }


    /*********************************************************************
     * GUI Functions
     * *******************************************************************/
    //Simple gui function to draw tile buttons
    private bool drawInactiveTile(Tile t)
    {
        bool hasClicked = false;

        EditorGUILayout.BeginHorizontal(EditorStyles.helpBox);
        //GUILayout.Label(t.TileTexture);
        if (GUILayout.Button(t.TileTexture, EditorStyles.label))
        {
            hasClicked = true;
        }
        GUILayout.Space(16);
        //GUILayout.Label(t.TileName);
        if(GUILayout.Button(t.TileName, EditorStyles.label, GUILayout.ExpandWidth(true)))
        {
            hasClicked = true;
        }
        EditorGUILayout.EndHorizontal();

        return hasClicked;
    }

    private void drawActiveTile(Tile t)
    {
        EditorGUILayout.BeginHorizontal(EditorStyles.textField);
        GUILayout.Label(t.TileTexture);
        GUILayout.Space(16);
        GUILayout.Label(t.TileName, GUILayout.ExpandWidth(true));
        EditorGUILayout.EndHorizontal();
    }


}
