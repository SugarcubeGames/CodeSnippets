#pragma warning disable 0414
#pragma warning disable 0169

using System;
using System.Collections;
using UnityEngine;
using interARCHt.Systems.DesignOptions;
using interARCHt.Systems.GUIs;

namespace interARCHt.Systems.GUIs
{
	/// <summary>
	/// GUI script for design options
	/// </summary>
	//The design option GUI will display a list of all the design options in the project.
	//When the user selects one of these optoins, the menu will resize and show all
	//suboptions within that design option.  To achieve this, the GUI will be handled with
	//layers, with layer -1 beingthe list of Design Options, and any higher number corresponding
	//to the index of the selected D.O. in the designOptions list stored in manager.
	public class GUI_DO
	{
		//Holder for the deisgn option manager
		private DOManager manager;
		private CoroutineHandler ch; //Handler for the box shape change coroutine

		private bool useDO; //Will we be using the design options for this presentation?
		private bool isActive; //Is the window active?
		//public acessor for isActive, used by the main GUI to prevent overlaps
		public bool Active{get{return isActive;}}
		private int id;

		//Rectangles for holding the size of the window.  Multiple are necessary due to
		private Rect windowRect;
		private Rect windowRectOld;

		//The title of the window
		private string title;

		//Is the window currently in the process of changing size?
		private bool changingSize;
		//float value used for size change interpolation
		private float t;

		//Window dimensions
		private int height;
		private int width;
		private int curLayer; //-1 = the design option selection layer.
		public GUI_DO (int id)
		{
			//Find _interARCHt and collect the manager from it
			useDO = false; //Design Options are turned on by default, and turned on if there are any avaiable.
			GameObject go = GameObject.Find ("_interARCHt");
			if(go == null){
				Debug.Log ("ERROR: Unable to locate Design Options." +
				           "\nDeisgn will not be avaiable during this presentation.");
				return;
			}

			ch = go.GetComponent<CoroutineHandler> ();
			if(ch == null){
				go.AddComponent (typeof(CoroutineHandler));
				ch = go.GetComponent<CoroutineHandler> ();
			}
			//manager = go.GetComponent<DOManager> ();

			if(manager == null){
				Debug.Log ("There was an error loading the Design Options manager." +
					"Design Options will not be avaiable this session.");
				return;
			}

			if(manager != null && manager.getTotDesignOptions() > 0){
				isActive = false;
				this.id = id;
				useDO = true;

				title = "Design Options";

				windowRect = new Rect (Screen.width/2.0f, Screen.height/2.0f, 0, 0);
				curLayer = -1;
				calcWindowDims (curLayer);

				windowRectOld = windowRect;

				changingSize = false;
				t = 0.0f;
			}
		}

		//Update method called from the main GUI.
		public void Update(){
			//Check for the appropriate input to active the Design Options menu
			//TODO: Set up key definitions so they can be changed by the use
			if(Input.GetKeyDown(KeyCode.Q)){
				isActive = !isActive;
				if(isActive){
					//If the menu is now active, we need to make sure that we start at the topmost layer
					curLayer = -1;
					calcWindowDims (curLayer); //calculate the window dimension for the layer
					title = "Design Options"; //Reset the window title

					Screen.showCursor = true;
					Screen.lockCursor = false;
				} else {
					//If the menu is now inactive, reset the cursot
					Screen.showCursor = false;
					Screen.lockCursor = false;
				}
			}
		}

		//The GUI display, called from the main GUI.
		public void DrawGUI(){
			if(!useDO || !isActive){
				return;
			}

			windowRect = GUI.Window (id, windowRect, doWindow, title);

			//If a button is pressed, check to see if a new option has been selected
			//We don't need to do anything on the top level
			if(curLayer != -1){
				//Check to see if any of them are set to be changes
				bool change = false;
				foreach (DOSubOption d in manager.designOptions[curLayer].Options) {
					if(d.changeActive){
						change = true;
					}
				}
				if(change){
					foreach(DOSubOption d in manager.designOptions[curLayer].Options){
						if(d.changeActive){
							d.makeActive ();
						} else {
							d.makeInactive ();
						}
					}
				}
			}
		}

		private void doWindow(int id){
			GUILayout.BeginVertical ();
			//Draw the appropriate buttons based on the layer
			if(curLayer == -1){
				for(int i = 0; i<manager.getTotDesignOptions(); i++){
					DOScr curOption = manager.getDesignOptions () [i];
					if (GUILayout.Button (curOption.Name, GUILayout.Height (20))){
						title = curOption.Name;
						curLayer = i;
						calcWindowDims (curLayer);

						ch.StartChildCoroutine (changeSize (0.25f));
					}
				}
				//The close button
				if(GUILayout.Button("Close")){
					//hide and lock the cursor, turn off the window
					Screen.showCursor = false;
					Screen.lockCursor = true;

					isActive = false;
				}
			} else { //Options within the selected design option
				//Draw all the avaiable options in the currently-selected option
				for(int i = 0; i<manager.designOptions[curLayer].Options.Count; i++){
					if (manager.designOptions [curLayer].Options[i].isActive) {
						if (GUILayout.Button ("*" + 
						                      manager.designOptions[curLayer].Options[i].Name
						                      + "*")) {
							//If this button is pressed, we don't need to do anyhting becuase
							//it's already active
							//manager.DesignOptions [curLayer].Options [i].changeActive = true;
						}
					} else {
						if(GUILayout.Button(manager.designOptions[curLayer].Options[i].Name)){
							//If this button is pressed, marked the current option to be changed to
							//the active option
							manager.designOptions [curLayer].Options [i].changeActive = true;
						}
					}
				}

				//Back button
				if(GUILayout.Button("Back")){
					title = "Design Options";
					curLayer = -1;
					calcWindowDims (curLayer);

					ch.StartChildCoroutine (changeSize (0.25f));
				}

			}

			GUILayout.EndVertical ();

			if(!changingSize){
				GUI.DragWindow ();
			}
		}

		//Calculate the size of the window based on the layer of the window we're on
		private void calcWindowDims(int layer){
			if(layer == -1){ //Highest layer, calc height by number of design options
				height = 24 * (manager.getTotDesignOptions ()+2); //+1 to account for the Title and back button
				//calculate the width based on the longest design option name
				int sl = title.Length * 8;
				foreach(DOScr d in manager.getDesignOptions()){
					int tc = d.Name.Length + 2; //+3 to account for the '* *'annotation on the active option 
					sl = Mathf.Max (sl, (tc * 8));
				}
				width = sl + 28;

				windowRect.height = height;
				windowRect.width = width;
			} else {
				height = 24 * (manager.getDesignOptions () [layer].getTotOptions () + 2);

				//calculate the width based on the lonest option name
				int sl = manager.getDesignOptions () [layer].Name.Length * 8;
				foreach(DOSubOption d in manager.getDesignOptions() [layer].getOptions()){
					int tc = d.Name.Length+2;
					sl = Mathf.Max (sl, (tc * 8));
				}
				width = sl + 28;

				windowRect.height = height;
				windowRect.width = width;
			}
		}

		//Coroutine for expanding the window
		private IEnumerator changeSize(float rate){
			windowRectOld.x = windowRect.x;
			windowRectOld.y = windowRect.y;

			//Calculate the new window position. Multiply by cx / cy to make positive or negative
			float newPosX = windowRectOld.x - (width - windowRect.width) / 2;
			float newPosY = windowRectOld.y - (height - windowRect.height) / 2;

			changingSize = true;

			t = 0.0f;

			while (t<1.0f){
				//Increment t based on the passed in rate
				t += rate;

				//Expand / shrink through interpolation
				windowRect.width = Mathf.SmoothStep (windowRectOld.width, width, t);
				windowRect.x = Mathf.SmoothStep (windowRectOld.x, newPosX, t);

				windowRect.height = Mathf.SmoothStep (windowRectOld.height, height, t);
				windowRect.y = Mathf.SmoothStep (windowRectOld.y, newPosY, t);

				//Yield (wait) the code execution for a short time so that the transition will be noticable
				yield return new WaitForSeconds (0.01f);
			}

			//Change the changing variable to false so we can drag the window again
			changingSize = false;
		}
	}
}

