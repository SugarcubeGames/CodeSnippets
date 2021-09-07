using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BSIRealtimeReflectionProbe : MonoBehaviour {

    [Header("Object References")]
    ///<summary>
    /// Reference to the VR Camera's CenterEyeAnchor
    ///</summary>
    [Tooltip("Reference to the VR Camera's CenterEyeAnchor (Do not fill, this is found when the scene loads!)")]
    public GameObject centerEyeAnchor;
    /// <summary>
    /// The Trigger used to determine when the player enters and
    /// exists the bounds of this reflection probe.
    /// </summary>
    public BoxCollider trigger;

    [Header("Constraints")]
    [Tooltip("Will the Reflection Probe follow the user's X-axis movements?")]
    public bool lockX = true;
    [Tooltip("Will the Reflection Probe follow the user's Y-axis movements?")]
    public bool lockY = true;
    [Tooltip("Will the Reflection Probe follow the user's Z-axis movements?")]
    public bool lockZ = true;

    /// <summary>
    /// Reflction Probe reference
    /// </summary>
    private ReflectionProbe probe;
    /// <summary>
    /// The last position of the Reflection Probe
    /// </summary>
    private Vector3 lastPost;
    /// <summary>
    /// Is this probe active?
    /// </summary>
    private bool isProbeActive;

    /// <summary>
    /// Ovverride to prevent constantly seeking out CenterEyeAnchor
    /// </summary>
    private bool failedToFindCenterEyeAnchor = false;

    // The roiginal extents of the probe
    private Vector3 originalExtents;
    private Vector3 originalProbePosition;
    private Vector3 originalProbeCenter;

    [Header("Debug")]
    [Tooltip("Show debug center spheres?")]
    public bool showDebug = false;
    GameObject debugProbeCenter;
    GameObject debugTriggerCenter;

    public bool isPlayerWithinTrigger
    {
        get
        {
            if(centerEyeAnchor == null)
            {
                return false;
            }
            return trigger.bounds.Contains(centerEyeAnchor.transform.position);
        }

        private set { }
    }

    // Use this for initialization
    void Start () {

        probe = this.gameObject.GetComponent<ReflectionProbe>();

        //Collect all the original data of this reflection probe
        originalExtents = probe.bounds.extents;
        originalProbeCenter = probe.center;
        originalProbePosition = this.gameObject.transform.position;

        //Modify trigger data to match reflection probe extents data
        trigger.center = probe.center;
        trigger.size = probe.size;

        lastPost = this.gameObject.transform.position;
        isProbeActive = isPlayerWithinTrigger;

        if (showDebug)
        {
            debugProbeCenter = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            debugTriggerCenter = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            debugProbeCenter.name = "Probe Center";
            debugTriggerCenter.name = "Trigger Center";
        }


	}
	
	// Update is called once per frame
	void Update () {

        if (failedToFindCenterEyeAnchor) return;
        
        if(centerEyeAnchor == null)
        {
            //Try to locate the center eye anchor
            centerEyeAnchor = GameObject.Find("CenterEyeAnchor");

            //If we don't find one, we don't want to keep trying to use the
            //realtime reflection probe.  Mark the failure checker to true
            //so Update will be prevented from running int he future.
            if(centerEyeAnchor == null)
            {
                failedToFindCenterEyeAnchor = true;
                return;
            }
        }

        if (isPlayerWithinTrigger)
        {
            //If we just moved from outside the trigger to insides of it
            //make this reflection probe active
            if (!isProbeActive)
            {
                probe.refreshMode = UnityEngine.Rendering.ReflectionProbeRefreshMode.EveryFrame;
                isProbeActive = true;
            }

            float newX = this.gameObject.transform.position.x;
            if (lockX) newX = centerEyeAnchor.transform.position.x;

            float newY = this.gameObject.transform.position.y;
            if (lockY) newY = centerEyeAnchor.transform.position.y;

            float newZ = this.gameObject.transform.position.z;
            if (lockZ) newZ = centerEyeAnchor.transform.position.z;

            Vector3 newPos = new Vector3(newX, newY, newZ);
            //The probe's center needs to move opposite player movement within the trigger
            //to prevent the extents from changing location as the player moves.
            Vector3 newProbeCenter = trigger.bounds.center + (trigger.center - newPos) - originalProbeCenter - (Vector3.up * originalProbePosition.y);
            

            probe.center = newProbeCenter;

            this.gameObject.transform.position = newPos;

            if (showDebug)
            {
                debugProbeCenter.transform.position = probe.center;
                debugTriggerCenter.transform.position = trigger.center;
            }

        } else
        {
            //If we just moved form inside the probe bounds to outside of them,
            //deactivate the probe and reset it
            if (isProbeActive)
            {
                this.gameObject.transform.position = originalProbePosition - (Vector3.up * originalProbePosition.y);
                probe.center = trigger.center;
                probe.refreshMode = UnityEngine.Rendering.ReflectionProbeRefreshMode.OnAwake;
                isProbeActive = false;
            }
        }
	}
}
