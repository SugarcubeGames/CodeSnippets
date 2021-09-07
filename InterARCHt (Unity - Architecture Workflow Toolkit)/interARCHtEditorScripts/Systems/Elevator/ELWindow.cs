using System;
using UnityEditor;
using UnityEngine;

using interARCHt;
using interARCHt.Systems.Elevator;

namespace interARCHtEditorScripts
{
	[Serializable]
	public class ELWindow : EditorWindow
	{
		[SerializeField]
		private ELManager manager;

		[MenuItem("interARCHt/Navigation/Elevator Manager", false, 130)]
		static void init(){
			GetWindow<ELWindow> ();
		}

		void OnEnable(){
			//try to access the scene, if it is null, call the scene load function.
			if(Scene.current == null){
				Scene.current = new Scene ();
			}

			manager = Scene.current.elManager;
		}

		void OnGUI(){
			EditorGUILayout.LabelField ("Feature Points", EditorStyles.boldLabel);

			//Draw the manager gui, and the guis for each individual ELScr
			if(manager.elevators == null){
				return;
			}
			foreach(ELScr e in manager.elevators){
				EditorGUILayout.LabelField (e.name + " | " + e.numLevels);
				e.name = EditorGUILayout.TextField ("Name:", e.name);
			}

			if(GUILayout.Button("Add", GUILayout.Width(70))){
				//Create the new elevator (Note: This is not how this will work, elevators will
				///be created by adding an elevator pointer to the scene.
				ELScr newElevator = (ELScr)ScriptableObject.CreateInstance<ELScr> ();
				newElevator.init (1);

				//Add the newly-created elevator to the elevator manager asset so that it will
				//be stored properly
#if UNITY_EDITOR
				AssetManager.AddSubObject (Scene.current.elManager, newElevator);
				//Add the newly-created elevator's path to the path list for later loading
				manager.paths.Add (AssetManager.GetAssetPath(newElevator));
				Debug.Log(manager.paths[manager.paths.Count-1]);
#endif
				manager.elevators.Add (newElevator);
			}
			if(GUILayout.Button("Save", GUILayout.Width(70))){
				saveManager ();
			}
		}

		private void saveManager(){
			//This method is used to set manager and its sub-assets to dirty so
			//their data will be saved
			EditorUtility.SetDirty (manager);
			foreach (ELScr e in manager.elevators) {
				EditorUtility.SetDirty (e);
			}
			//Now thet they've been marked as durty, save them
#if UNITY_EDITOR
			SaveLoad.Save ();
#endif
		}
	}
}

