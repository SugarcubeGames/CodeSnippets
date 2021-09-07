using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BSIMoveToPlayerStartOnSceneLoad : MonoBehaviour {

    private void Awake()
    {
        //Debug.Log("BSIMoveToPlayerStartOnSceneLoad :: Awake()");
        SceneManager.activeSceneChanged += MovePlayerToStartPosition;
    }

    private void OnDestroy()
    {
        //Debug.Log("BSIMoveToPlayerStartOnSceneLoad :: OnDestroy()");
        SceneManager.activeSceneChanged -= MovePlayerToStartPosition;
    }

    public void MovePlayerToStartPosition(Scene oldScene, Scene NewScene)
    {
        //Debug.Log("BSIMoveToPlayerStartOnSceneLoad :: MovePlayerToStartPosition(" + oldScene.name + ", " + NewScene.name + ")");

        //We don't need to do anyhting if we are in the Player scene
        if (NewScene.name == "Player") return;

        //Look for an object in the scene called "BSIStartPos"  This is a prefab.
        GameObject start = GameObject.Find("BSIStartPos");

        if(start == null)
        {
            Debug.LogError("The scene " + NewScene.name + " does not have a start position object.  Please add one:\nBlue Square Interactive/Prefabs/BSIStartPos");
        } else
        {
            this.transform.position = start.transform.position;
            this.transform.rotation = start.transform.rotation;
        }
    }
}
