using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class TileSetEditorWindow : EditorWindow {

    /***********************
     * Top toolbar variables
     * *********************/
    /// <summary>
    /// Texture for the New Tileset button.
    /// </summary>
    private Texture buttonNew;
    /// <summary>
    /// Texture for the Save Tileset button.
    /// </summary>
    private Texture buttonSave;
    /// <summary>
    /// Texture for the Open Tileset buttons.
    /// </summary>
    private Texture buttonOpen;



    /// <summary>
    /// The Tileset that is currently being edited
    /// </summary>
    private TileSet tileset;

    //Temp tileset data, used for checking for changes
    /// <summary>
    /// Temp tileset name
    /// </summary>
    private string tsName;

    //Active tile temp information
    /// <summary>
    /// Active tile's name
    /// </summary>
    private string atName;
    /// <summary>
    /// Active tile's sprite
    /// </summary>
    private Sprite atSprite;
    /// <summary>
    /// What is the current active index in the editor?
    /// </summary>
    private int atActiveIndex;

    /// <summary>
    /// Rect for showing object picker for sprite
    /// </summary>
    private Rect atRect;


    private bool hasBeenModified = false;


    /// <summary>
    /// Reference to the scriptable object which contains all the editor icons (Same icons as room editor)
    /// </summary>
    private EditorIconRef iconRef;

    [MenuItem("Infinivania/Tileset Editor")]
    static void Init()
    {
        //Get existing open window or, if none, create a new one
        TileSetEditorWindow window = (TileSetEditorWindow)
            EditorWindow.GetWindow(typeof(TileSetEditorWindow));
        window.Show();

        window.minSize = new Vector2(100.0f, 100.0f);

        window.init();
    }

    /// <summary>
    /// Initialize all window variables
    /// </summary>
    public void init()
    {
        //Instantiate the icon ref
        iconRef = EditorIconRef.CreateInstance<EditorIconRef>();
        buttonNew = EditorIconRef.Instance.NewTextureLG;
        buttonOpen = EditorIconRef.Instance.OpenTextureLG;
        buttonSave = EditorIconRef.Instance.SaveTextureLG;
    }

    public void OnGUI()
    {

        /************************************************************
         * Top toolbar: New, Save, Open
         * **********************************************************/
        EditorGUILayout.BeginHorizontal(EditorStyles.helpBox);

        //EditorGUILayout.LabelField("Test");
        if (GUILayout.Button(buttonNew, EditorStyles.label, GUILayout.Width(32), GUILayout.Height(32)))
        {
            createNewTileset();
        }

        if (GUILayout.Button(buttonOpen, EditorStyles.label, GUILayout.Width(32), GUILayout.Height(32)))
        {
            openTileset();
        }

        if (GUILayout.Button(buttonSave, EditorStyles.label, GUILayout.Width(32), GUILayout.Height(32)))
        {
            saveTileset();
        }
        EditorGUILayout.EndHorizontal();
        //End top toolbar

        //If we do not have a tileset loaded, we don't want to show any more
        if(tileset == null)
        {
            return;
        }


        //Horizontal layouts for the sidebar and tile view
        EditorGUILayout.BeginHorizontal();

        /******************************************************************
         * Vertical sidebar - shows add/remove tile buttons, as well as all
         * tiles in the tileset
         * ****************************************************************/
        EditorGUILayout.BeginVertical(EditorStyles.helpBox, GUILayout.Width(200), GUILayout.ExpandHeight(true));

        GUIUtils.DrawCenteredText("Tileset Name:");
        //Tilset name:
        tsName = EditorGUILayout.TextField(tsName, GUILayout.ExpandWidth(true));

        //Add new tile button
        //Show add tile button
        if (GUIUtils.DrawCenteredButtonHorz("Add Tile", GUILayout.Width(100))){
            tileset.AddNewTile();
            //UpdateActiveTileData();
            SetEditorTilesetData(tileset);
        }


        //If there are no tiles in this tileset, we do not want to draw any more in the column
        if (tileset.Count != 0)
        {
            GUILayout.Label("Active Tile: " + tileset.ActiveTileIndex);
            //Delete current tile button

            //tileset.ActiveTileIndex = GUILayout.SelectionGrid(tileset.ActiveTileIndex, tileset.TileTextures, 4);

            for(int i = 0; i<tileset.TileList.Count; i++)
            {
                if(GUILayout.Button(tileset.TileList[i].TileTexture, EditorStyles.helpBox)){
                    //tileset.setActiveIndex(i);
                    atActiveIndex = i;
                }
            }
        }

        EditorGUILayout.EndVertical();
        //End vertical sidebar

        //Tile information
        EditorGUILayout.BeginVertical();

        //If there are no tiles, we do not want to draw any of the tile information
        if(tileset.Count > 0)
        {

            atSprite = EditorGUILayout.ObjectField(atSprite, typeof(Sprite), false) as Sprite;
            
            GUIUtils.DrawTilesetSpritePreview(atSprite, (int)atSprite.rect.width, (int)atSprite.rect.height);
        }

        EditorGUILayout.EndVertical();
        //End tile view


        EditorGUILayout.EndHorizontal();
        //End vertical sidebar/tile horizontal grouping

        //Check for changes
        if(tsName != tileset.TileSetName)
        {
            hasBeenModified = true;
        }
        //Checking for changes to the tile
        if (tileset.Count > 0)
        {
            if(atActiveIndex != tileset.ActiveTileIndex)
            {
                tileset.setActiveIndex(atActiveIndex);
                //Update the editor's tileset data
                SetEditorTilesetData(tileset);
            }
            
            if(atSprite != tileset.ActiveTile.TileSprite)
            {
                UpdateActiveTileData();
            }
        }
    }

    /*********************************************************************
     * Calcualtion Methods
     * *******************************************************************/
            private void calcTileSpriteRect()
    {

    }



    /*********************************************************************
     * Create, Save, and Open methods
     * *******************************************************************/
    private void createNewTileset()
    {
        if (hasBeenModified)
        {
            //Prompt to save

            hasBeenModified = false;
        }

        tileset = ScriptableObject.CreateInstance<TileSet>();
        tileset.Init();

        //Set editor data to matchnew tileset.
        SetEditorTilesetData(tileset);

        hasBeenModified = true;
    }

    private void saveTileset()
    {
        Debug.Log(tileset + " | " + hasBeenModified);
        if(tileset == null || !hasBeenModified)
        {
            return;
        }

        //Update all tileset data prior to saving
        UpdateTilesetData();
        UpdateActiveTileData();

        if(tileset.AssetSavePath == "")
        {
            string pathToSave = EditorUtility.SaveFilePanel("Save Tileset to:", "", "", "");

            //Reduce the returned path down to a realtive application path
            if (pathToSave.StartsWith(Application.dataPath))
            {
                pathToSave = "Assets" + pathToSave.Substring(Application.dataPath.Length);
            }

            //Assign the asset save path
            tileset.setAssetSavePath(pathToSave);

            if (tileset.AssetSavePath.EndsWith(".asset")){
                AssetDatabase.CreateAsset(tileset, pathToSave);
            } else
            {
                AssetDatabase.CreateAsset(tileset, pathToSave + ".asset");
            }
            AssetDatabase.SaveAssets();
        }
        else
        {
            AssetDatabase.SaveAssets();
        }

        hasBeenModified = false;

        //Set the tileset to dirty so changes will be saved
        EditorUtility.SetDirty(tileset);
        tileset.saveTiles(); //Set each tile as dirty so they save as well.

    }

    private void openTileset()
    {
        string pathToRoom = EditorUtility.OpenFilePanel("Open Tileset:", "", "");
        if (pathToRoom.StartsWith(Application.dataPath))
        {
            string relPath = "Assets" + pathToRoom.Substring(Application.dataPath.Length);
            tileset = AssetDatabase.LoadAssetAtPath(relPath, typeof(TileSet)) as TileSet;

        }

        SetEditorTilesetData(tileset);
        if(tileset.Count > 0)
        {
            tileset.setActiveIndex(0);
        }
    }


    /*********************************************************
     * Methods to update Tileset daat
     * *******************************************************/
    
    private void UpdateTilesetData()
    {
        tileset.setTilesetName(tsName);
        tileset.UpdateTileTextureArray();
        tileset.saveTiles();
    }
    
    private void UpdateActiveTileData()
    {
        //atSprite = tileset.ActiveTile.TileSprite;
        tileset.setActiveTileSprite(atSprite);
        tileset.saveTiles();
    }

    private void SetEditorTilesetData(TileSet t)
    {
        tsName = t.TileSetName;
        if (tileset.Count > 0)
        {
            atSprite = t.ActiveTile.TileSprite;
            atActiveIndex = tileset.ActiveTileIndex;
        }
    }
}
