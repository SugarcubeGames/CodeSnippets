using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

public class BSIMaterialOptionUpdateOptionsInScene : EditorWindow{

    //variables for the progress barr
    private static int totalOperations = 0;
    private static int curOperation = 0;

	[MenuItem("Blue Square Interactive/Update Option Reference in Scene", false, 42)]
    static void updateOptionReferenceInScene()
    {

        curOperation = 0;
        totalOperations = 0;

        //Delete all instances of the interaction script in the scene.
        foreach(BSIMaterialOptionInteraction o in GameObject.FindObjectsOfType<BSIMaterialOptionInteraction>())
        {
            Object.DestroyImmediate(o);
        }

        //Load the Material Option Manager
        BSIMaterialOptionManager _manager = Resources.Load("Materials/MaterialOptionManager") 
                                                as BSIMaterialOptionManager;

        GameObject[] objArray = GameObject.FindObjectsOfType<GameObject>();

        totalOperations = objArray.Length * _manager.Options.Count;

        foreach (GameObject go in objArray)
        {

            foreach(BSIMaterialOption option in _manager.Options)
            {
                try
                {
                    if(go.GetComponent<MeshRenderer>() != null)
                    {
                        if(go.GetComponent<MeshRenderer>().sharedMaterials[0].name == option.Material.name)
                        {
                            //Debug.Log("Object " + go.name + " is part of Material Option " + option.Name);
                            BSIMaterialOptionInteraction newInteraction = go.AddComponent<BSIMaterialOptionInteraction>();

                            newInteraction.setMaterialOption(option);
                            EditorUtility.SetDirty(newInteraction);

                        } 
                    }
                } catch (System.Exception ex)
                {
                    Debug.LogError(ex.Message);
                }
            }
        }

        //Let the scene know that there have been modifications to it.
        //Without this, it will not mark it as needing saved, so saving wihtout
        //changing something else will not work.
        EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
    }
}
