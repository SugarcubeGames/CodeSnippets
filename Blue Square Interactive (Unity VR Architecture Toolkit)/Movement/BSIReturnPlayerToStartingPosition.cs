using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BSIReturnPlayerToStartingPosition : MonoBehaviour {

    private Vector3 startPosition;

    /// <summary>
    /// Player contorller object
    /// </summary>
    public GameObject playerController;

    public KeyCode playerResetKey = KeyCode.M;

    public void Awake()
    {
        startPosition = playerController.transform.position;

        //Add updateStartPos to the scene loaded event so the player
        //will return to the proper position
        SceneManager.activeSceneChanged += updateStartPos;
    }

    public void OnDestroy()
    {
        SceneManager.activeSceneChanged -= updateStartPos;
    }

    public void Update()
    {
        if (Input.GetKeyDown(playerResetKey))
        {
            playerController.transform.position = startPosition;
        }
    }

    public void updateStartPos(Scene oldScene, Scene NewScene)
    {
        if (NewScene.name == "Player") return;

        //Look for an object in the scene called "BSIStartPos"  This is a prefab.
        GameObject start = GameObject.Find("BSIStartPos");

        startPosition = start.transform.position;
    }
}
