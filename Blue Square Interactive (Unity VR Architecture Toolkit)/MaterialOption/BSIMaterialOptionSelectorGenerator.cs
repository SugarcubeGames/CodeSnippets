using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script creates the material selection spheres that
//appear whenever the player interacts with a Material Option object.
//It receives a material option for easy access
public class BSIMaterialOptionSelectorGenerator : MonoBehaviour {

    public GameObject playerController;
    public GameObject playerCenterEyeAnchor;

    /// <summary>
    /// The option being selected
    /// </summary>
    private BSIMaterialOption _option;
    /// <summary>
    /// The sphere being used to select materials
    /// </summary>
    private List<GameObject> spheres;
    /// <summary>
    /// Are we currently showing the selectors in the secne?
    /// </summary>
    private bool isShowingSelectors = false;

    private float radius = 0.2f;

    public Material closeButtonMaterial;

	public void ShowMaterialSelectors(BSIMaterialOption option)
    {
        if(_option != null)
        {
            //Debug.Log("Old: " + _option.Name + "\nNew: " + option.Name);
        }
        //Trying to open the same option twice causes problems
        if (isShowingSelectors && _option.Equals(option)) return;

        _option = option;

        //Position the Material Option display point relative to the
        //camera center eye position.
        this.transform.localPosition = new Vector3(-0.025f, 
                    playerCenterEyeAnchor.transform.localPosition.y - 0.25f, 
                    playerCenterEyeAnchor.transform.localPosition.z + 0.5f);

        if(spheres != null)
        {
            foreach(GameObject g in spheres)
            {
                Destroy(g);
            }
            spheres.Clear();
        } else
        {
            spheres = new List<GameObject>();
        }

        generateSphere();

        isShowingSelectors = true;
    }

    public void clearMaterialDisplay()
    {
        foreach(GameObject go in spheres)
        {
            Destroy(go);
        }
        spheres.Clear();

        isShowingSelectors = false;
    }

    private void generateSphere()
    {
        int numPoints = _option.Materials.Count;

        radius = 0.0375f + 0.0375f + (.011f * (float)numPoints);

        float angle = 2 * Mathf.PI / numPoints;

        for(int i = 0; i<numPoints; i++)
        {
            float x = radius * Mathf.Sin(i * angle);
            float y = radius * Mathf.Cos(i * angle);

            GameObject go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            go.name = _option.Materials[i].name;
            go.transform.parent = this.gameObject.transform;
            go.transform.localPosition = new Vector3(x, y, 0);
            go.transform.localScale = new Vector3(0.075f, 0.075f, 0.075f);
            //Make sure that the sphere is always facing the player
            go.transform.localRotation = Quaternion.Euler(.025f, playerController.transform.localRotation.y + 90.0f, 0.0f);
            go.GetComponent<SphereCollider>().isTrigger = true;

            go.GetComponent<MeshRenderer>().material = _option.Materials[i];

            Rigidbody rb = go.AddComponent<Rigidbody>();
            rb.isKinematic = true;

            BSIMaterialOptionSelector selector = go.AddComponent<BSIMaterialOptionSelector>();
            selector.init(_option, _option.Materials[i]);

            spheres.Add(go);
        }

        //create the close sphere
        GameObject closeObj = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        closeObj.name = "Close Sphere";
        closeObj.transform.parent = this.gameObject.transform;
        closeObj.transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
        closeObj.transform.localScale = new Vector3(0.075f, 0.075f, 0.075f);
        //Make sure that the sphere is always facing the player
        closeObj.transform.localRotation = Quaternion.Euler(.025f, playerController.transform.localRotation.y + 90.0f, 0.0f);
        closeObj.GetComponent<SphereCollider>().isTrigger = true;

        closeObj.GetComponent<MeshRenderer>().material = closeButtonMaterial;

        Rigidbody rb2 = closeObj.AddComponent<Rigidbody>();
        rb2.isKinematic = true;

        BSIMaterialOptionCloseSelector closeSelector =  
            closeObj.AddComponent<BSIMaterialOptionCloseSelector>();
        closeSelector.init(this);

        spheres.Add(closeObj);
    }
}
