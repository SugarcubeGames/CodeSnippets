using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * The Tile Manager will be where tiles are created, and will store the complete
 * list of tiles for access from the tileset editor, level editor, and game
 * */

[System.Serializable]
public class TileManager : ScriptableObject {

    //Public accessor:
    private static TileManager _instance;
    public static TileManager Instance
    {
        get
        {
            if(_instance == null)
            {
                try
                {
                    _instance = Resources.Load<TileManager>("TileManager.asset");
                    _instance.init();
                } catch (System.Exception ex)
                {
                    Debug.LogError(ex.Message);
                }
                 
            }


            return _instance;
        }
    }

    /// <summary>
    /// A list of all tiles in the game.
    /// </summary>
    [SerializeField]
    private List<Tile> _tiles;
    public List<Tile> Tiles
    {
        get { return _tiles; }
    }

    /// <summary>
    /// Where this manager is saved.  There should only be one, so this is going to be a const.
    /// </summary>
    [SerializeField]
    private const string _assetPath = "Assets/Resources/TileManager.asset";

    /// <summary>
    /// The default sprite used when a new tile is created
    /// </summary>
    [SerializeField]
    private Sprite defaultSprite;

    public void init()
    {
        if(_tiles == null)
        {
            _tiles = new List<Tile>();
        }

        defaultSprite = Resources.Load<Sprite>("Sprites/tex_ss_EditorIcons_Error");
        //Debug.Log("Sprite: " + defaultSprite.name);
    }
    
    public void AddTile()
    {
        //Create a new tile, and assign the defaul sprite to it
        Tile newTile = ScriptableObject.CreateInstance<Tile>();
        newTile.init();

        newTile.setSprite(defaultSprite);
        _tiles.Add(newTile);
    }

    public void DeleteTile(int index)
    {
        if(_tiles.Count == 1)
        {
            Debug.LogError("You cannot delete the only tile in the game.");
            return;
        }

        _tiles.RemoveAt(index);
    }
}
