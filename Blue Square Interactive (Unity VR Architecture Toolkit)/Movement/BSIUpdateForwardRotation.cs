using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BSIUpdateForwardRotation : MonoBehaviour {

    /// <summary>
    /// Forward Direction Game object (What this script is attached to)
    /// </summary>
    public Transform forwardDirection;
    
    /// <summary>
    /// Camera Center Eye object
    /// </summary>
    public Transform centerEyeAnchor;

    /// <summary>
    /// Player rigid body (for angular velocity)
    /// </summary>
    public Rigidbody playerRigidBody;

    /// <summary>
    /// Maximum head rotation angle before the camera adjusts to match.
    /// This angle is negative or positive from 0 (forward direction)
    /// </summary>
    public float maxHeadAngleRotation = 35.0f;

    public Rigidbody testRB;

	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
        if (forwardDirection == null || centerEyeAnchor == null) return;

        //Debug.Log(playerRigidBody.angularVelocity);

        //If the center eye anchor's angular rotation is greater than or less than
        //the maximum head angle rotation, rotate the forward direction to
        //compensate for bodily rotation.
        //forwardDirection.localRotation.eulerAngles.y - centerEyeAnchor.right +
        //Debug.Log("Forward: " + forwardDirection.rotation.eulerAngles.y + "\nCenter: " + centerEyeAnchor.rotation.eulerAngles.y);
        //Debug.Log("Forward: " + forwardDirection.rotation.y + "\nCenter: " + centerEyeAnchor.rotation.y);
        //Debug.Log("Forward: " + forwardDirection.localRotation.y + "\nCenter: " + centerEyeAnchor.localRotation.y);
        //Debug.Log("Forward: " + forwardDirection.localRotation.eulerAngles.y + "\nCenter: " + centerEyeAnchor.localRotation.eulerAngles.y);
        
        float forwardY = forwardDirection.localRotation.eulerAngles.y;
        float eyeY = centerEyeAnchor.localRotation.eulerAngles.y;

        //Debug.Log(Mathf.Abs((eyeY - (360 - forwardY))));

        float curAngle = 0.0f;

        if((forwardY - eyeY) <= 180.0f)
        {
            curAngle = (eyeY - forwardY);
        } else
        {
            //curAngle = Mathf.Abs((eyeY - (360 - forwardY)));
            //curAngle = eyeY - (360 - forwardY);
            //curAngle = Mathf.Abs(forwardY - 360) - eyeY;
            curAngle = Mathf.Abs(forwardY - eyeY);
        }

        //Debug.Log("curAngle:  " + curAngle + " || eyeY: " + eyeY + " || forwaydY: " + forwardY + "  ||  " + (forwardY - 360));


        /*
        if((eyeY - forwardY) > maxHeadAngleRotation || Mathf.Abs((eyeY - (360-forwardY))) > maxHeadAngleRotation)
        {
            //Debug.Log("Greater than 35 deg: " + (eyeY - forwardY));
        }
        */
        if (Mathf.Abs(curAngle) > maxHeadAngleRotation)
        {
            //Debug.Log("curAngle:  " + curAngle + " || eyeY: " + eyeY + " || forwaydY: " + forwardY);
            //forwardDirection.localRotation.SetEulerAngles(0,eyeY,0);
            //forwardDirection.eulerAngles.Set(0, eyeY, 0);
            //forwardDirection.localRotation.SetEulerAngles(0.0f, eyeY, 0.0f);
            //forwardDirection.localRotation.eulerAngles.Set(0.0f, eyeY, 0.0f);
            forwardDirection.localRotation = Quaternion.Euler(new Vector3(0.0f,eyeY,0.0f));
        }

        if(testRB != null)
        {
            Debug.Log(testRB.angularVelocity);
        }
    }

    public void ResetForwardDirection()
    {
        float eyeY = centerEyeAnchor.localRotation.eulerAngles.y;
        forwardDirection.localRotation = Quaternion.Euler(new Vector3(0.0f, eyeY, 0.0f));
    }
}
