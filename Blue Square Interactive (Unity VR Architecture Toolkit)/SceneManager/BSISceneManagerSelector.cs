using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BSISceneManagerSelector : MonoBehaviour {

    private string sceneName;

    public TextMesh sceneNameText;

	public void init(string sceneName)
    {
        this.sceneName = sceneName;
        sceneNameText.text = sceneName;
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.name.Equals(BSITagsLayers.OculusRightIndexName) ||
            other.name.Equals(BSITagsLayers.OculusLeftIndexName))
        {
            SceneManager.LoadSceneAsync(sceneName);
        }
    }

}
