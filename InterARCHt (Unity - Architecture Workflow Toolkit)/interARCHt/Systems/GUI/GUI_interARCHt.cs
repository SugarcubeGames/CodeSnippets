using System;
using System.Collections;
using UnityEngine;
using interARCHt.Systems.DesignOptions;
using interARCHt.Systems.FeaturePoint;
using interARCHt.Systems.MaterialOptions;

namespace interARCHt.Systems.GUIs
{
	//TODO: Instead of using this one class to draw all the various gui elements, use this class to
	//initialize the GUI elements, which each have their own class.  Then, this class can call each of their
	//DrawGUI functions, so that this class can stay clean, and they can be organized better.  When initializing, 
	//pass in a integer for the window ID.  This way it'll be easy to make sure there are no duplicate
	//values for that between the windows.

	/// <summary>
	/// This class will handle all the GUI interactions
	/// </summary>
	
	public class GUI_interARCHt : MonoBehaviour
	{
		Transform cameraT;
		// Use this for initialization

		GUI_FP fpG;
		GUI_MO moG;
		GUI_DO doG;


		void Awake () {
			Transform[] allTrans = this.gameObject.GetComponentsInChildren<Transform>();
			foreach(Transform t in allTrans){
				if(t.name == "Main Camera"){
					cameraT = t.transform;
				}
			}

			Screen.showCursor = false;
			Screen.lockCursor = true;

			//Set up the individual gui element
			//GUIs with windows:
			fpG = new GUI_FP(cameraT, 0);
			doG = new GUI_DO (1);
			//Guis without windows:
			moG = new GUI_MO(cameraT);

		}

		//This method will call the individual update functions for
		//each GUI elements.  There will be checks to prevent
		//multiple GUI elements from being active at once
		void Update () {

			//Windowed GUI elements
			if(!doG.Active){
				fpG.Update ();
			}
			if(!fpG.Active){
				doG.Update ();
			}
			//NO-Window GUI elements
			if(!fpG.Active && !doG.Active){
				moG.Update (); //Material option updater
			}

			//Check for escape key press
			if(Input.GetKeyDown(KeyCode.Escape)){
				Application.Quit();
			}
		}

		void OnGUI(){
			fpG.DrawGUI ();
			doG.DrawGUI ();
		}
	}
}