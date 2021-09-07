using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/*
 * The executable should always begin with Player as the first scene to load
 * Immediately upon load, subscribe to various events
 * Once subscribed, begin loading new scene (the second scene in the scene build list)
 * 
 * By using the DontDestroyOnLoad function on objects in the Player scene, there is no
 * longer a need to load scenes additively.  There is also no longer a neww to
 * unload scenes once the scene is loaded.  This significantly simplifies
 * the entire process!
 * */
public class BSISceneManager : MonoBehaviour {

    /// <summary>
    /// Load level operation
    /// </summary>
    private AsyncOperation asyncLoad;
    /// <summary>
    /// Unload level operation
    /// </summary>
    private AsyncOperation asynUnload;

    private void Awake()
    {
        //Debug.Log("BSISceneManager :: Awake()");

        BSIEvents.BeginSceneChange += LoadLevelByName;
        BSIEvents.BeginSceneChangeByIndex += LoadLevelByIndex;
    }

    private void Start()
    {
        //Begin loading the new scene.  Get teh second scene in the build list.
        if (SceneManager.sceneCountInBuildSettings == 1)
        {
            Debug.LogError("ADD MORE THAN ONE SCENE TO BUILD LIST!");
        }
        else
        {
            //SceneManager.GetSceneByBuildIndex(); returns null if the scene is not actually loaded, making it completely useless...
            //string path = SceneUtility.GetScenePathByBuildIndex(1);
            //string sceneName = path.Substring(0, path.Length - 6).Substring(path.LastIndexOf('/') + 1);

            //TEST: Find OvrAvatarSDKManager and add don't destroy onload to it
            GameObject go = GameObject.Find("OvrAvatarSDKManager");
            if (go.GetComponent<BSIDontDestroyOnLoad>() == null)
            {
                go.AddComponent<BSIDontDestroyOnLoad>();
            }


            BSIEvents.beginSceneChangeByIndex(1);
        }
    }

    private void OnDestroy()
    {
        //Debug.Log("BSISceneManager :: OnDestroy()");

        BSIEvents.BeginSceneChange -= LoadLevelByName;
    }

    public void LoadLevelByName(string newSceneName)
    {
        //Debug.Log("BSISceneManager :: LoadLevelByName()");
        StartCoroutine(LoadLevel(newSceneName));
    }

    private IEnumerator LoadLevel(string sceneName)
    {
        //Debug.Log("BSISceneManager :: LoadLevel()");

        if (sceneName == "")
        {
            yield break;
        }

        asyncLoad = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
        asyncLoad.allowSceneActivation = false;

        while(asyncLoad.progress < .9f)
        {
            yield return null;
        }

        asyncLoad.allowSceneActivation = true;

        yield break;
    }

    public void LoadLevelByIndex(int newSceneIndex)
    {
        //Debug.Log("BSISceneManager :: LoadLevelByName()");
        StartCoroutine(LoadLevel(newSceneIndex));
    }

    private IEnumerator LoadLevel(int index)
    {
        //Debug.Log("BSISceneManager :: LoadLevel()");

        asyncLoad = SceneManager.LoadSceneAsync(index, LoadSceneMode.Single);
        asyncLoad.allowSceneActivation = false;

        while (asyncLoad.progress < .9f)
        {
            yield return null;
        }

        //Allow the newly-loaded scene to be activated, removing the previously
        //loaded scene.
        asyncLoad.allowSceneActivation = true;

        yield break;
    }
}
