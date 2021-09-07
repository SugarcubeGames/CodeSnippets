using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using interARCHt.Systems.MaterialOptions;

namespace interARCHtEditorScripts.Systems.MaterialOptions.Editor
{
	/*
	 * The window class will be resposnbile for drawing all elements of the GUI
	 * for the material options.  This switch is being done to accommodate the
	 * innability for attached scripts to draw their own guis and still compile
	 * for an external runtime.
	 * */
	public class MOWindow : EditorWindow {

		private MOManager manager;

		[MenuItem ("interARCHt/Material Options Manager", false, 110)]
		static void init(){
			GetWindow<MOWindow>();
		}

		void OnEnable(){
			hideFlags = HideFlags.HideAndDontSave;
			if(manager == null){
				//Debug.Log ("manager is null");
				loadManager();
			}
		}

		void OnGUI(){
			GUILayout.Label("Material Options", EditorStyles.boldLabel);
			MOGUIManager.DrawGUI (manager);

			if(GUI.changed){
				EditorUtility.SetDirty(manager);
				AssetDatabase.Refresh();
			}
		}

		void loadManager(){

			//Find the _interARCHt gameobject, and load the MOManager from it
			GameObject go = GameObject.Find("_interARCHt");
			//If we don't find the gameobject, create one and add teh MOManager script to it
			if(go == null){
				go = new GameObject();
				go.name = "_interARCHt";
				//go.hideFlags = HideFlags.HideInHierarchy;

				go.AddComponent(typeof(MOManager));
				manager = go.GetComponent<MOManager>();
				manager.init ();
				return;
			}

			//If we found the gameobject, try to load the manager script from it
			manager = go.GetComponent<MOManager>();

			if(manager == null){
				go.AddComponent(typeof(MOManager));
				manager = go.GetComponent<MOManager>();
				manager.init ();
			}
		}

		//------------------------------------------------------------------------------------

	
	
	}
}

