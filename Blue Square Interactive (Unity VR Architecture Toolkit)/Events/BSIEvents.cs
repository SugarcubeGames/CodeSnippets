using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BSIEvents{

    /**************************************************************
     * Custom Scene change events
     * ************************************************************/

    public delegate void BSILoadSceneByNameEventHandler(string sceneName);
    public static event BSILoadSceneByNameEventHandler BeginSceneChange;
    /// <summary>
    /// Call event when loading a new scene.  This begins the process.
    /// </summary>
    /// <param name="newSceneName"></param>
    public static void beginSceneChange(string newSceneName)
    {
        //Debug.Log("BSIEvents :: beginSceneChange(" + newSceneName + ")");
        if (newSceneName != null && newSceneName != "")
        {
            BeginSceneChange(newSceneName);
        }
    }

    public delegate void BSILoadSceneByIndexEventHandler(int sceneIndex);
    public static event BSILoadSceneByIndexEventHandler BeginSceneChangeByIndex;
    /// <summary>
    /// Call event when loading a new scene.  This begins the process.
    /// </summary>
    /// <param name="newSceneName"></param>
    public static void beginSceneChangeByIndex(int newSceneIndex)
    {
        //Debug.Log("BSIEvents :: beginSceneChange(" + newSceneName + ")");
        BeginSceneChangeByIndex(newSceneIndex);
    }


    /*************************************************************
     * Player controller modificaiton events
     * ***********************************************************/

    //Event called whenever the player moves.  Used to update active reflection probe
    public delegate void BSIPlayerHasMovedEventHandler();
    public static event BSIPlayerHasMovedEventHandler OnPlayerHasMoved;
    public static void playerHasMoved()
    {
        //Debug.Log("Player has moved");
        if (OnPlayerHasMoved != null)
        {
            OnPlayerHasMoved();
        }
    }

    //Event called whenever we modify the player's height
    public delegate void BSIModifiedPlayerHeightEventHandler(float amnt);
    public static event BSIModifiedPlayerHeightEventHandler OnModifiedPlayerHeight;
    public static void playerHeightModified(float amnt)
    {
        //Debug.Log("Player has moved");
        if (OnModifiedPlayerHeight != null)
        {
            OnModifiedPlayerHeight(amnt);
        }
    }
}
