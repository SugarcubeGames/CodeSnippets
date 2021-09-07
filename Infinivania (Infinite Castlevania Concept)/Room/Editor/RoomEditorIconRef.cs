using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[System.Serializable]
public class RoomEditorIconRef : ScriptableObject {
    
    /// <summary>
    /// Sprite for the New Room button
    /// </summary>
    [SerializeField]
    private Sprite editorButtonNewRoomSprite;
    public Texture editorButtonNewRoom
    {
        get
        {
            return AssetPreview.GetAssetPreview(editorButtonNewRoomSprite);
        }
        private set { }
    }

    /// <summary>
    /// Sprite for the Open Room button
    /// </summary>
    [SerializeField]
    private Sprite editorButtonOpenRoomSprite;
    public Texture editorButtonOpenRoom
    {
        get
        {
            return AssetPreview.GetAssetPreview(editorButtonOpenRoomSprite);
        }
        private set { }
    }

    /// <summary>
    /// Sprite for the Save Room button
    /// </summary>
    [SerializeField]
    private Sprite editorButtonSaveRoomSprite;
    public Texture editorButtonSaveRoom
    {
        get
        {
            return AssetPreview.GetAssetPreview(editorButtonSaveRoomSprite);
        }
        private set { }
    }

    
}
