using System;
using UnityEditor;
using UnityEngine;
using interARCHt.Systems.FeaturePoint;

namespace interARCHtEditorScripts.Systems.FeaturePoint.Editor
{
	public class FPWindow : EditorWindow
	{
		private FPManager manager;

		[MenuItem("interARCHt/Navigation/Feature Point Manager", false, 120)]
		static void init(){
			GetWindow<FPWindow> ();
		}

		void OnEnable(){
			hideFlags = HideFlags.HideAndDontSave;
			if(manager == null){
				loadManager();
			}

			manager.collectFeatures ();
		}

		//Required because features are added outside of the manager
		void OnFocus(){
			if(manager == null){
				loadManager();
			}

			manager.collectFeatures ();
		}

		void OnGUI(){
			managerGUI(manager);

			if(GUI.changed){
				EditorUtility.SetDirty (manager);
			}
		}

		private void loadManager(){

			//Find the _interARCHt gameobject, and load the MOManager from it
			GameObject go = GameObject.Find("_interARCHt");
			//If we don't find the gameobject, create one and add teh MOManager script to it
			if(go == null){
				go = new GameObject();
				go.name = "_interARCHt";
				//go.hideFlags = HideFlags.HideInHierarchy;

				go.AddComponent(typeof(FPManager));
				manager = go.GetComponent<FPManager>();
				manager.init ();
				return;
			}

			//If we found the gameobject, try to load the manager script from it
			manager = go.GetComponent<FPManager>();

			if(manager == null){
				go.AddComponent(typeof(FPManager));
				manager = go.GetComponent<FPManager>();
				manager.init ();
			}
		}

		/**********************************************
		 * GUI Methods
		 * ********************************************/

		//--------------------------------------------------------------------------
		//The Mangaer GUI
		private void managerGUI(FPManager scr){
			GUILayout.Label ("Feature Points", EditorStyles.boldLabel);
			foreach(FPScr f in scr.Features){
				featurePointGUI(f);
			}
		}

		//--------------------------------------------------------------------------
		//The Feature Point GUI
		private void featurePointGUI(FPScr scr){
			scr.Fold = EditorGUILayout.Foldout (scr.Fold, scr.Name);

			if(scr.Fold){
				EditorGUI.indentLevel++;

				scr.Name = EditorGUILayout.TextField ("Name:", scr.Name);
				EditorGUILayout.LabelField ("Object: " + scr.Obj.name);

				EditorGUILayout.BeginHorizontal ();
				GUILayout.Space (19 * EditorGUI.indentLevel);
				GUILayout.Label ("Narrative: ");
				scr.HasNarrative = EditorGUILayout.Toggle (scr.HasNarrative);
				EditorGUILayout.EndHorizontal ();

				if(scr.HasNarrative){
					scr.Narrative = EditorGUILayout.TextArea (scr.Narrative);
				}

				EditorGUILayout.BeginHorizontal ();
				GUILayout.Space(16*EditorGUI.indentLevel);
				GUILayout.Box("", GUILayout.ExpandWidth(true), GUILayout.Height(1));
				EditorGUILayout.EndHorizontal ();
				EditorGUI.indentLevel--;
			}

			if(GUI.changed){
				//set this script to dirty so changes will be saved
				EditorUtility.SetDirty (scr);
			}
		}
	}
}

