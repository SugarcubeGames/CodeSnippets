using interARCHt.Systems.FeaturePoint;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace interARCHt.Systems.GUIs
{
	/// <summary>
	/// GUI Element for the feature point system
	/// </summary>
	//TODO: Decide if I want to haveths window change size.
	public class GUI_FP
	{
		//TODO: Remove the camera, wit the switch to needing to hold the right mouse button
		//to move the camera, it is no longer necessary to lock it when the feature
		//point menu comes up/
		//Maintain a reference to the camera so we can lock it
		Transform camera;

		//For now, the feature point window shows whenever the player right clicks
		private FPManager manager;
		private bool useFP; //Are there any feature points in the scene?
		private bool isActive;
		private int id;
		public Rect rectOpen; //Size and position are determined on startup
		private int curFeature; //The currently selected feature
		private int height; //Based on total Feature Points
		private int width;
		private string title; //The title of the feature point window

		//isActive property for GUI checks (to prevent multiple GUI elements from
		//activating at once
		public bool Active{ get{return isActive;}}
		public GUI_FP (Transform cam, int id)
		{
			this.camera = cam;
			this.id = id;

			useFP = false; //Feature points are turned off by default, and turned on if there are any available

			GameObject go = GameObject.Find ("_interARCHt");
			if(go == null){
				Debug.Log ("ERROR: Unable to locate Feature Points." +
					"\nFeature Points will not be avaiable during this presentation.");
				return;
			}
			//Initialize feature point variables
			manager = go.GetComponent<FPManager> ();
			//If there are no feature points, we won't need to initialize any of
			//these variables
			if(manager != null && manager.getTotFeatures() > 0){
				useFP = true; //There are feature points to display
				isActive = false; //The fp window doesn't start active
				title = "Feature Points";
				id = 0; //Window ID is 0
				curFeature = -1; //No features are selected
				calcWindow ();

				//Give the rectangle a starting position.  It will be draggable
				rectOpen = new Rect(100, 100, width, height+5);
				//Debug.Log ("TOT Features: " + manager.getFeatureNames ().Length);
			}
		}

		public void Update(){

			if(!useFP){
				return;
			}

			//Right now, I'm looking at using the right mouse click for camera movement (limiting it to
			//only when the right mouse button is held down), so I'm switching the required input for this to
			//the 'E' key
			if(Input.GetKeyDown(KeyCode.E)){
				isActive = !isActive; //Right clicking will toggle the window
				if(isActive){
					//Lock the viewport TODO: NEED TO FIGURE OUT HOW TO HANDLE THIS - MOUSELOOK NOT PART OF THIS DLL.. custom mouse script?
					try{

					} catch {

					}

					Screen.showCursor = true;
					Screen.lockCursor = false;
				} else {
					//unlock camera, hide and lock cursor
					Screen.showCursor = false;
					Screen.lockCursor = true;
				}
			}
		}

		public void DrawGUI(){
			if(!useFP || !isActive){
				return;
			}

			rectOpen = GUI.Window (id, rectOpen, fpWindow, title);
		}

		//The method to dispay the window content
		private void fpWindow(int id){

			//Populate the window with all feature points
			curFeature = GUI.SelectionGrid (new Rect(10,20,width-20, height-20), curFeature, manager.getFeatureNames (), 1);

			//If curFeature != -1, that means that a feautre point button was pressed, and we need
			//to move the player.
			if(curFeature != -1){

				//In order to teleport, we need to object the FPScr we're
				//teleporting to is attached to.  This is because the script does
				//not contain any positional information, only the object it's attached
				//to do.
				GameObject topCamera = camera.gameObject.transform.parent.gameObject;
				//Debug.Log (camera + "\n" + topCamera);
				List<FPScr> features = manager.getFeatures ();
				GameObject point = features [curFeature].gameObject;
				//Debug.Log(point + " | " + point.name);
				//Get the pos of the feature point, and modify it slightly so that the
				//camera doesn't fall through the floor
				Vector3 newPos = point.transform.position;
				newPos.y += .1f; //Move the camera up slightly to prevent falling through the floor

				//Get the rotation of the feature point, and modify the camera to align with it
				Vector3 newRot = point.transform.rotation.eulerAngles;
				newRot.y += 270.0f; //Look straight forward
				newRot.x += 90.0f;  //Stay vertical

				//Set the position and rotation of the camera
				topCamera.transform.position = newPos;
				topCamera.transform.rotation = Quaternion.Euler (newRot);

				//Reset everything to prepare for the next button press
				curFeature = -1;
				//Close the window
				isActive = false;
				//Renenable viewport rotation

				//Rehide the mouse
				Screen.showCursor = false;
				Screen.lockCursor = true;
			}

			//Make the window draggable
			GUI.DragWindow ();
		}

		private void calcWindow(){
			height = 20 * (manager.getTotFeatures () + 1);
			width = title.Length * 8;
			foreach(FPScr f in manager.getFeatures()){
				width = Mathf.Max (width, f.Name.Length * 8);
			}
			width += 5;
		}
	}
}

