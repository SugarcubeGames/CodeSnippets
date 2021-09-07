using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Tile : ScriptableObject{

    /// <summary>
    /// What sprite does this Tile use?
    /// </summary>
    [SerializeField]
    private Sprite _tileSprite;
    public Sprite TileSprite
    {
        get { return _tileSprite; }
        set { _tileSprite = value; }
    }

    /// <summary>
    /// This Tile's Texture.
    /// </summary>
    [SerializeField]
    private Texture2D _tileTexture;
    public Texture TileTexture
    {
        get { return _tileTexture as Texture; }
        private set { }
    }

    /// <summary>
    /// Scaled version of this Tile's Texture.
    /// </summary>
    [SerializeField]
    private Texture2D _tileTextureScaled;
    public Texture TileTextureScaled
    {
        get { return _tileTextureScaled as Texture; }
        private set { }
    }

    /// <summary>
    /// What is this tile's name?
    /// </summary>
    [SerializeField]
    private string _tileName;
    public string TileName
    {
        get { return _tileName; }
        set { _tileName = value;  }
    }

    /// <summary>
    /// Description of this tiles
    /// </summary>
    [SerializeField]
    private string _description;
    public string Description
    {
        get{ return _description;}
        private set{_description = value;}
    }

    /// <summary>
    /// Is this tile solid?
    /// </summary>
    [SerializeField]
    private bool _isSolid;
    public bool IsSolid
    {
        get { return IsSolid; }
        private set { _isSolid = value; }
    }

    public void init()
    {
#if UNITY_EDITOR
        _tileSprite = EditorIconRef.Instance.ErrorSpritSM;
#endif
        generateTextureImages();
    }

    public void setSprite(Sprite s)
    {
        _tileSprite = s;
        generateTextureImages();
    }

    public void setName(string s)
    {
        _tileName = s;
    }

    private void generateTextureImages()
    {
        //Make sure the sprite has data
        if (_tileSprite == null) return;

        _tileTexture = new Texture2D((int)_tileSprite.rect.width,
                                        (int)_tileSprite.rect.height);
        _tileTextureScaled = new Texture2D((int)_tileSprite.rect.width,
                                            (int)_tileSprite.rect.height);

        Color[] px = _tileSprite.texture.GetPixels((int)_tileSprite.rect.x,
                                                    (int)_tileSprite.rect.y,
                                                    (int)_tileSprite.rect.width,
                                                    (int)_tileSprite.rect.height);

        _tileTexture.SetPixels(px);
        _tileTextureScaled.SetPixels(px);

        _tileTexture.Apply();
        _tileTextureScaled.Apply();
    }

    public void scaleTexture(int scaleFactor)
    {
        //reset the texture whenever scaling happens to prevent distortion
        _tileTextureScaled = new Texture2D((int)_tileSprite.rect.width,
                                            (int)_tileSprite.rect.height);

        Color[] px = _tileSprite.texture.GetPixels((int)_tileSprite.rect.x,
                                                    (int)_tileSprite.rect.y,
                                                    (int)_tileSprite.rect.width,
                                                    (int)_tileSprite.rect.height);

        _tileTextureScaled.SetPixels(px);

        if(scaleFactor != 1)
        {
            TextureScale.Bilinear(_tileTextureScaled, (int)_tileSprite.rect.width * scaleFactor,
                                    (int)_tileSprite.rect.height * scaleFactor);
        }
    }

    //Todo: Add animation.  Some tiles have animations, I need to incorporate that
}
