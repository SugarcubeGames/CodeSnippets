using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BSIOTInteraction : MonoBehaviour {

    [Header("Object References:")]
    /// <summary>
    /// Object from which the interaction raycast is cast
    /// </summary>
    public Transform interactionPointerOrigin;
    /// <summary>
    /// Line renderer for interaction raycast
    /// </summary>
    public LineRenderer interactionLineRenderer;

    [Header("Interaction Arc variables:")]
    /// <summary>
    /// the smoothness of the arc
    /// </summary>
    public int interactionArcSmoothness = 40;
    /// <summary>
    /// Color of the arc when not on an teleportable surface
    /// </summary>
    public Color invalidInteractionColor = Color.red;
    /// <summary>
    /// The last object the interactoin raycast hit
    /// </summary>
    private GameObject lastHitObj;
    /// <summary>
    /// Is the player currently attempting to interact with the scene?
    /// </summary>
    private bool isAttemptingInteraction;


    [Header("Material Option Variables:")]
    /// <summary>
    /// Color of the arc when on an teleportable surface
    /// </summary>
    public Color validMaterialOptionColor = Color.green;
    /// <summary>
    /// Is teh currently-hit object a Material Option Object?
    /// </summary>
    private bool isMaterialOptionObject;
    /// <summary>
    /// Reference to the interaction 
    /// </summary>
    public BSIMaterialOptionSelectorGenerator optionSelectionGenerator;

    // Use this for initialization
    void Start () {
        lastHitObj = null;
        isMaterialOptionObject = false;
	}
	
	// Update is called once per frame
	void Update () {
		
        if(OVRInput.Get(OVRInput.RawAxis1D.LIndexTrigger) > 0.0f && !isAttemptingInteraction)
        {
            isAttemptingInteraction = true;
        }

        if(OVRInput.Get(OVRInput.RawAxis1D.LIndexTrigger) <= 0.0f && isAttemptingInteraction)
        {
            isAttemptingInteraction = false;
            interactionLineRenderer.enabled = false;
        }

        if (isAttemptingInteraction)
        {
            RaycastHit curHit;
            List<Vector3> points;

            BSICurvedRaycast.CurveCast(interactionPointerOrigin.position, interactionPointerOrigin.forward, 
                                       Vector3.down * 2.0f, interactionArcSmoothness, out curHit, 30f, out points);

            if(curHit.transform != null)
            {
                interactionLineRenderer.enabled = true;
                interactionLineRenderer.positionCount = points.Count;
                interactionLineRenderer.SetPositions(points.ToArray());

                if(curHit.transform.gameObject != lastHitObj)
                {
                    lastHitObj = curHit.transform.gameObject;

                    //Determine if the object being hit has a material option on it
                    isMaterialOptionObject = checkIsMaterialOptionObject(curHit.transform.gameObject);

                    if (!isMaterialOptionObject)
                    {
                        interactionLineRenderer.material.color = invalidInteractionColor;
                        interactionLineRenderer.material.SetColor("_EmissionColor", invalidInteractionColor);
                    }
                    else
                    {
                        interactionLineRenderer.material.color = validMaterialOptionColor;
                        interactionLineRenderer.material.SetColor("_EmissionColor", validMaterialOptionColor);
                    }
                }

                if (OVRInput.GetUp(OVRInput.Button.Three) && isMaterialOptionObject)
                {
                    //curHit.transform.gameObject.GetComponent<BSIMaterialOptionInteraction>().showNextMaterial();
                    optionSelectionGenerator.ShowMaterialSelectors(curHit.transform.gameObject.GetComponent<BSIMaterialOptionInteraction>().Option);
                }
            } else
            {
                interactionLineRenderer.enabled = false;
            }
        }
	}


    private bool checkIsMaterialOptionObject(GameObject obj)
    {
        //return obj.GetComponent<BSIMaterialOptionInteraction>() != null;
        if (obj.GetComponent<BSIMaterialOptionInteraction>() != null) return true;
        return false;
    }
}
