using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[System.Serializable]
public class TileSet : ScriptableObject {

    /// <summary>
    /// What is this TileSet's name?
    /// </summary>
    [SerializeField]
    private string _tileSetName;
    public string TileSetName
    {
        get { return _tileSetName; }
        private set { }
    }

    /// <summary>
    /// List of Tile object in thei tileset.
    /// </summary>
    [SerializeField]
    private List<Tile> _tileList;
    public List<Tile> TileList
    {
        get { return _tileList; }
        private set { }
    }

    /// <summary>
    /// Where is this tileset saved to?
    /// </summary>
    [SerializeField]
    private string _assetSavePath = "";
    public string AssetSavePath
    {
        get { return _assetSavePath; }
        private set { }
    }

    /// <summary>
    /// Which tile is currently being 
    /// </summary>
    private int activeTile;
    public Tile ActiveTile
    {
        get{ return _tileList[activeTile]; }
    }
    public int ActiveTileIndex
    {
        get { return activeTile; }
        set { activeTile = value; }
    }

    private Texture[] _tileTextures;
    public Texture[] TileTextures
    {
        get
        {
            return _tileTextures;
        }
    }

    public int Count
    {
        get { return _tileList.Count; }
        private set { }
    }

    public void Init()
    {
        if(_tileList == null)
        {
            _tileList = new List<Tile>();
        }

        activeTile = 0;
    }

    public void AddNewTile()
    {
        //Create a new tile instance
        Tile newTile = ScriptableObject.CreateInstance<Tile>();
        newTile.init();

        //Add the new tile to the tile list
        _tileList.Add(newTile);

        //Set the active tile to the new tile index
        activeTile = _tileList.IndexOf(newTile);
        newTile.TileName = "New Tile";
        //UpdateTileTextureArray();
    }

    public Tile getCurrentTile()
    {
            return _tileList[activeTile];
    }



    /**********************************************************
     * 
     * ********************************************************/
    public void setAssetSavePath(string path)
    {
        _assetSavePath = path;
    }

    public void setTilesetName(string n)
    {
        _tileSetName = n;
    }

    public void setActiveTileSprite(Sprite s)
    {
        ActiveTile.setSprite(s);
    }

    public void setActiveIndex(int i)
    {
        activeTile = i;
    }

    //Set all tiles as dirty so they save
    public void saveTiles()
    {
        for(int i = 0; i<Count; i++)
        {
#if UNITY_EDITOR
            EditorUtility.SetDirty(_tileList[i]);
#endif
        }
    }


    //Update tile texture array
    public void UpdateTileTextureArray()
    {
        Debug.Log("Updating Texture List:"  + Count);

        _tileTextures = new Texture[_tileList.Count];
        for(int i = 0; i<_tileList.Count; i++)
        {
            //_tileTextures[i] = AssetPreview.GetAssetPreview(_tileList[i].TileSprite);
        }
    }
}

