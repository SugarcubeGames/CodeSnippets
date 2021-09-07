using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[System.Serializable]
public class ScriptDisplayTesting : EditorWindow {

    [MenuItem("Infinivania/Sprite Render Editor")]
    static void Init()
    {
        //Get existing open window or, if none, create a new one
        ScriptDisplayTesting window = (ScriptDisplayTesting)
            EditorWindow.GetWindow(typeof(ScriptDisplayTesting));
        window.Show();

        window.minSize = new Vector2(100.0f, 100.0f);

        window.setup();
    }

    [SerializeField]
    private Sprite spriteToRender;

    [SerializeField]
    private Sprite[] spritesFromSpriteSheet;

    [SerializeField]
    private Texture2D spriteTex;

    [SerializeField]
    private List<IVBlock> blocks;

    public void setup()
    {
        //Get the teexture of the selected sprite
        spriteTex = spriteToRender.texture;

        //Load that sprite sheet from resources
        spritesFromSpriteSheet = Resources.LoadAll<Sprite>("Sprites/"+spriteTex.name);

        Debug.Log(spriteTex.name + "\nNum Sprites: " + spritesFromSpriteSheet.Length);

        blocks = new List<IVBlock>();

        foreach (Sprite s in spritesFromSpriteSheet)
        {
            IVBlock newBlock = ScriptableObject.CreateInstance<IVBlock>();
            newBlock.init(s);

            blocks.Add(newBlock);
        }
    }


    private void OnGUI()
    {
        EditorGUI.BeginChangeCheck();
        EditorGUILayout.BeginHorizontal(EditorStyles.helpBox, GUILayout.Height(32));
        spriteToRender = EditorGUILayout.ObjectField(spriteToRender, typeof(Sprite), false) as Sprite;
        EditorGUILayout.EndHorizontal();
        if (EditorGUI.EndChangeCheck())
        {
            setup();
        }

        EditorGUILayout.BeginHorizontal();
        foreach(IVBlock b in blocks)
        {
            GUILayout.Label(b.BlockTexture, GUILayout.Width(b.width+2), GUILayout.Height(b.height+2));
            GUILayout.Label(b.BlockTexture2X, GUILayout.Width(b.width * 2 + 2), GUILayout.Height(b.height * 2 + 2));
            GUILayout.Label(b.BlockTexture4X, GUILayout.Width(b.width * 4 + 2), GUILayout.Height(b.height * 4 + 2));
        }
        EditorGUILayout.EndHorizontal();
    }

}

//This is a test iteration of a script that will contain a sprite, as well
//as a texture image for that sprite.  This attempt is because
//trying to use sprites in the editore window is proving.... difficult.
//This class will receive a sprite and automatically generate a texture2d for it
[System.Serializable]
public class IVBlock : ScriptableObject
{

    [SerializeField]
    private Sprite blockSprite;

    [SerializeField]
    private Texture2D blockTexture;
    public Texture BlockTexture
    {
        get { return blockTexture as Texture; }
    }

    [SerializeField]
    private Texture2D blockTexture2X;
    public Texture BlockTexture2X
    {
        get { return blockTexture2X; }
    }

    [SerializeField]
    private Texture2D blockTexture4X;
    public Texture BlockTexture4X
    {
        get { return blockTexture4X; }
    }

    public int width;
    public int height;

    public void init(Sprite s)
    {
        blockSprite = s;
        generateTextureFromSprite();

        width = blockTexture.width;
        height = blockTexture.height;
    }

    private void generateTextureFromSprite()
    {
        if (blockSprite == null) return;

        blockTexture = new Texture2D((int)blockSprite.textureRect.width, 
                                        (int)blockSprite.textureRect.height);
        blockTexture2X = new Texture2D((int)blockSprite.textureRect.width,
                                        (int)blockSprite.textureRect.height);
        blockTexture4X = new Texture2D((int)blockSprite.textureRect.width,
                                        (int)blockSprite.textureRect.height);

        Color[] px = blockSprite.texture.GetPixels((int)blockSprite.rect.x,
                                                    (int)blockSprite.rect.y,
                                                    (int)blockSprite.rect.width,
                                                    (int)blockSprite.rect.height);

        blockTexture.SetPixels(px);
        blockTexture.Apply();

        blockTexture2X.SetPixels(px);
        blockTexture2X.Apply();

        //Generate the 2X texture
        TextureScale.Bilinear(blockTexture2X, blockTexture.width * 2, blockTexture.height * 2);

        blockTexture4X.SetPixels(px);
        blockTexture4X.Apply();
        TextureScale.Bilinear(blockTexture4X, blockTexture.width * 4, blockTexture.height * 4);

    }
}
