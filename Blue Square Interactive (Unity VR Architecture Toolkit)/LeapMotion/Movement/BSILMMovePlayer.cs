using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BSILMMovePlayer : MonoBehaviour {

    /// <summary>
    /// Is the player currently pointer their finger for teleporting?
    /// </summary>
    private bool isPointingForMovement;
    /// <summary>
    /// Where are we casting the movement raycast from?
    /// </summary>
    public Transform raycastOrigin;
    /// <summary>
    /// The line renderer for the raycast
    /// </summary>
    public LineRenderer lineRenderer;
    /// <summary>
    /// How smooth should the curve be?
    /// </summary>
    public int smoothness;
    /// <summary>
    /// The minimum amount of movement required to update the
    /// raycast position.  This is to help with issues of jumpiness
    /// </summary>
    public float movementThreshold;
    /// <summary>
    /// Holder for previous raycast origin position.  Used with movement threshold.
    /// </summary>
    private Vector3 previousRaycastOriginPos;

    /// <summary>
    /// Called when the hand gesture is in the proper location.  Activates the movement system.
    /// </summary>
    public void ActivateMovementSystem()
    {
        isPointingForMovement = true;
        //Turn on the line renderer
        lineRenderer.enabled = true;

        previousRaycastOriginPos = raycastOrigin.position;
    }

    public void DeactivateMovementSystem()
    {
        isPointingForMovement = false;
        //Turn off the line renderer
        lineRenderer.enabled = false;
    }

    public void Update()
    {
        if (isPointingForMovement)
        {
            //Debug.Log("BSILMMovePlayer :: Update :: IsPointingForwad is True");
            RaycastHit curhit;
            List<Vector3> points;

            float delta = (previousRaycastOriginPos - raycastOrigin.position).magnitude;
            if(delta >= movementThreshold)
            {
                BSICurvedRaycast.CurveCast(raycastOrigin.position, raycastOrigin.right + (raycastOrigin.up), Vector3.down, smoothness, out curhit, 60.0f, out points);
                previousRaycastOriginPos = raycastOrigin.position;
            }
            BSICurvedRaycast.CurveCast(raycastOrigin.position,raycastOrigin.right, Vector3.down, smoothness, out curhit, 60.0f, out points);

            //Update the line renderer with the new points
            if(curhit.transform != null)
            {
                lineRenderer.enabled = true;
                lineRenderer.positionCount = points.Count;
                lineRenderer.SetPositions(points.ToArray());

            } else
            {
                lineRenderer.enabled = false;
            }
            
        }
    }
}
