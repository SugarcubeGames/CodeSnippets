using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[System.Serializable]
public class EditorIconRef : ScriptableObject
{
#if UNITY_EDITOR
    private static EditorIconRef _instance;
    public static EditorIconRef Instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = ScriptableObject.CreateInstance<EditorIconRef>();
            }

            return _instance;
        }

        private set { }
    }
    

    /// <summary>
    /// Sprite for the New Room button (Large)
    /// </summary>
    [SerializeField]
    private Sprite newSpriteLG;
    public Texture NewTextureLG
    {
        get
        {
            return AssetPreview.GetAssetPreview(newSpriteLG);
        }
        private set { }
    }
    public Sprite NewSpriteLG
    {
        get { return newSpriteLG; }
    }
    /// <summary>
    /// Sprite for the New Room button (Small)
    /// </summary>
    [SerializeField]
    private Sprite newSpriteSM;
    public Texture NewTextureSM
    {
        get
        {
            return AssetPreview.GetAssetPreview(newSpriteSM);
        }
        private set { }
    }
    public Sprite NewSpriteSM
    {
        get { return newSpriteSM; }
    }

    /// <summary>
    /// Sprite for the Open Room button (Large)
    /// </summary>
    [SerializeField]
    private Sprite openSpriteLG;
    public Texture OpenTextureLG
    {
        get
        {
            return AssetPreview.GetAssetPreview(openSpriteLG);
        }
        private set { }
    }
    public Sprite OpenSpriteLG
    {
        get { return openSpriteLG; }
    }
    /// <summary>
    /// Sprite for the Open Room button (Small)
    /// </summary>
    [SerializeField]
    private Sprite openSpriteSM;
    public Texture OpenTextureSM
    {
        get
        {
            return AssetPreview.GetAssetPreview(openSpriteSM);
        }
        private set { }
    }
    public Sprite OpenSpriteSM
    {
        get { return openSpriteSM; }
    }

    /// <summary>
    /// Sprite for the Save Room button (Large)
    /// </summary>
    [SerializeField]
    private Sprite saveSpriteLG;
    public Texture SaveTextureLG
    {
        get
        {
            return AssetPreview.GetAssetPreview(saveSpriteLG);
        }
        private set { }
    }
    public Sprite SaveSpriteLG
    {
        get { return saveSpriteLG; }
    }
    /// <summary>
    /// Sprite for the Save Room button (Small)
    /// </summary>
    [SerializeField]
    private Sprite saveSpriteSM;
    public Texture SaveTextureSM
    {
        get
        {
            return AssetPreview.GetAssetPreview(saveSpriteSM);
        }
        private set { }
    }
    public Sprite SaveSpriteSM
    {
        get { return saveSpriteSM; }
    }

    /// <summary>
    /// Error sprite.  Used as default for new / missing tiles.(Large)
    /// </summary>
    [SerializeField]
    private Sprite errorSpriteLG;
    public Texture ErrorTextureLG
    {
        get
        {
            return AssetPreview.GetAssetPreview(errorSpriteLG);
        }
        private set { }
    }
    public Sprite ErrorSpriteLG
    {
        get { return errorSpriteLG; }
    }
    /// <summary>
    /// Error sprite.  Used as default for new / missing tiles. (Small)
    /// </summary>
    [SerializeField]
    private Sprite errorSpriteSM;
    public Texture ErrorTextureSM
    {
        get
        {
            return AssetPreview.GetAssetPreview(errorSpriteSM);
        }
        private set { }
    }
    public Sprite ErrorSpritSM
    {
        get { return errorSpriteSM; }
    }
#endif
}