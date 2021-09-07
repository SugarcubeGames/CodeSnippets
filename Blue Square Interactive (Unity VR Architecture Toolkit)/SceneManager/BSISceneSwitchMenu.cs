using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BSISceneSwitchMenu : MonoBehaviour {

    [Header("Inputs")]
    /// <summary>
    /// Button to toggle the scene selection menu
    /// </summary>
    public OVRInput.RawButton showMenuButton = OVRInput.RawButton.Y;

    /// <summary>
    /// list of scenes that need to be linked to
    /// </summary>
    private List<string> sceneNames;

    /// <summary>
    /// Prefab of the scene selection button
    /// </summary>
    public GameObject SceneMenuPrefab;

    /// <summary>
    /// Reference to the camera's center eye (for height)
    /// </summary>
    public GameObject playerCenterEyeAnchor;

    /// <summary>
    /// Is the scene selection menu currently active?
    /// </summary>
    private bool isActive;
    /// <summary>
    /// Reference to all buttons created by this script.
    /// </summary>
    private List<GameObject> buttons;

    private void Awake()
    {
        buttons = new List<GameObject>();

        SceneManager.activeSceneChanged += buildSceneList;

        isActive = false;
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        /*
        if (Input.GetKeyDown(KeyCode.F))
        {
            ShowSceneSelector();
        }
        */

        if (OVRInput.GetUp(showMenuButton))
        {
            if (!isActive)
            {
                ShowSceneSelector();
            } else
            {
                if (buttons != null && buttons.Count > 0)
                {
                    foreach (GameObject go in buttons)
                    {
                        GameObject.DestroyImmediate(go);
                    }

                    buttons.Clear();
                }

                isActive = false;
            }
        }
	}

    
    public void buildSceneList(Scene oldScene, Scene newScene)
    {
        //Debug.Log("Old: " + oldScene.name + "\nNew: " + newScene.name);

        if (newScene.name.Equals("Player"))
        {
            //Debug.Log("Don't generate list for Player scene");
            return;
        }
        sceneNames = new List<string>();

        //Delete any existing scene transition buttons
        if (buttons != null && buttons.Count > 0)
        {
            foreach(GameObject go in buttons)
            {
                GameObject.DestroyImmediate(go);
            }

            buttons.Clear();
        }

        //Determine how many scenes are in the list
        //Debug.Log("Total Scenes: " + SceneManager.sceneCountInBuildSettings);

        for(int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            //Debug.Log(SceneManager.GetSceneByBuildIndex(i).name + "\n" + SceneUtility.GetScenePathByBuildIndex(i));

            string path = SceneUtility.GetScenePathByBuildIndex(i);

            int charIndex = path.LastIndexOf("/");
            //Get the name of the scene.  Do this by getting everyhting after the last '/' 
            //character, and by removing the '.unity' at the end of the path string.
            string name = path.Substring(charIndex + 1, path.Length - charIndex - 7);
            //Debug.Log(name);

            //If the scene's name is "Player", don't add it.
            //Or, if the scene's name is the name of hte active scene, don't add it.
            if(!name.Equals("Player") && !name.Equals(SceneManager.GetActiveScene().name))
            {
                sceneNames.Add(name);
            }

        }

        //Debug.Log("Total linkable scenes: " + sceneNames.Count);

        isActive = false;
    }

    public void ShowSceneSelector()
    {
        //Position the Material Option display point relative to the
        //camera center eye position.
        //this.transform.localPosition = new Vector3(-0.05f,
                    //playerCenterEyeAnchor.transform.localPosition.y - 0.15f,
                    //1.0f);

        this.transform.localPosition = new Vector3(-0.025f,
                    playerCenterEyeAnchor.transform.localPosition.y - 0.05f,
                    playerCenterEyeAnchor.transform.localPosition.z + .5f);

        //Generate boxes for each scene
        List<Vector3> buttonPos = GenerateButtonPosList(sceneNames.Count);

        for (int i = 0; i < sceneNames.Count; i++)
        {
            GameObject newButton = Instantiate(SceneMenuPrefab) as GameObject;
            newButton.transform.SetParent(this.transform);
            newButton.name = sceneNames[i];

            newButton.transform.localPosition = buttonPos[i];

            newButton.transform.LookAt(playerCenterEyeAnchor.transform);

            BSISceneManagerSelector selector = newButton.GetComponent<BSISceneManagerSelector>();
            selector.init(sceneNames[i]);

            buttons.Add(newButton);
        }

        isActive = true;
    }

    private List<Vector3> GenerateButtonPosList(int numScenes)
    {
        List<Vector3> pos = new List<Vector3>();

        //Button hieght variables for layout.
        float btnHt = 0.075f;
        float spacing = 0.025f;
        float totHt = btnHt + spacing;
        float totHtHalf = totHt / 2.0f;

        float halfAmnt = Mathf.Floor(numScenes / 2.0f);

        if(numScenes % 2 == 0)
        {
            for (int i = 0; i < numScenes; i++)
            {
                if (i != halfAmnt)
                {
                    float y = ((halfAmnt - i) * totHt) - totHtHalf;
                    pos.Add(new Vector3(0.0f, y, 0.0f));
                }
                else
                {
                    float y = ((halfAmnt - (i + 1)) * totHt) + totHtHalf;
                    pos.Add(new Vector3(0.0f, y, 0.0f));
                }
            }
        } else
        {
            for(int i = 0; i<numScenes; i++)
            {
                if (i != halfAmnt)
                {
                    float y = (halfAmnt - i) * totHt;
                    pos.Add(new Vector3(0.0f, y, 0.0f));
                } else
                {
                    pos.Add(Vector3.zero);
                }
            }
        }

        return pos;

    }
}
