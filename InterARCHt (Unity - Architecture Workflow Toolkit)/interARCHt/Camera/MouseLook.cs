using System;
using UnityEngine;

namespace interARCHt.Camera
{
	/// <summary>
	/// This script will replace the default mouselook script in Unity
	/// </summary>
	public class MouseLook : MonoBehaviour
	{
		public enum RotationAxes { MouseXAndY = 0, MouseX = 1, MouseY = 2 }
		public RotationAxes axes = RotationAxes.MouseXAndY;
		public float sensitivityX = 15f;
		public float sensitivityY = 15f;

		public float minimumX = -360f;
		public float maximumX = 360f;

		public float minimumY = -75f;
		public float maximumY = 75f;

		float rotationY = 0f;

		void Update(){
			//only update if the right mouse button is being held down
			if(!Input.GetMouseButton(1)){
				return;
			}

			if(axes == RotationAxes.MouseXAndY){
				float rotationX = transform.localEulerAngles.y + Input.GetAxis ("Mouse X") * sensitivityX;

				rotationY += Input.GetAxis ("Mouse Y") * sensitivityY;
				rotationY = Mathf.Clamp (rotationY, minimumY, maximumY);

				transform.localEulerAngles = new Vector3 (-rotationY, rotationX, 0);
			} else if(axes == RotationAxes.MouseX){
				transform.Rotate (0, Input.GetAxis ("Mouse X") * sensitivityX, 0);
			} else {
				rotationY += Input.GetAxis ("Mouse Y") * sensitivityY;
				rotationY = Mathf.Clamp (rotationY, minimumY, maximumY);

				transform.localEulerAngles = new Vector3 (-rotationY, transform.localEulerAngles.y, 0);
			}
		}

		void Start(){
			//Make the rigid body not change rotation
			if(rigidbody){
				rigidbody.freezeRotation = true;
			}
		}
	}
}

