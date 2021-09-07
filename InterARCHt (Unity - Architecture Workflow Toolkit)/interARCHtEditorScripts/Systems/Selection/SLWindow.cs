using System;
using UnityEditor;
using UnityEngine;

namespace interARCHt.Systems.SelectionTools
{
	/// <summary>
	/// The selection group manager (this window) will control the
	/// creation of selection groups, as well as the selection
	/// of the specified group in the editor.
	/// </summary>
	public class SLWindow : EditorWindow
	{
		private SLManager manager;

		[MenuItem("interARCHt/Selection Tools/Selection Group Manager", false, 14)]
		static void init(){
			GetWindow<SLWindow> ();
		}

		void OnEnable(){
			hideFlags = HideFlags.HideAndDontSave;
			if(manager == null){
				//Debug.Log ("manager is null");
				loadManager();
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

				go.AddComponent(typeof(SLManager));
				manager = go.GetComponent<SLManager>();
				manager.init ();
				return;
			}

			//If we found the gameobject, try to load the manager script from it
			manager = go.GetComponent<SLManager>();

			if(manager == null){
				go.AddComponent(typeof(SLManager));
				manager = go.GetComponent<SLManager>();
				manager.init ();
			}
		}

		void OnGUI(){
			GUILayout.Label("Selection Group Manager", EditorStyles.boldLabel);
			foreach(SLGroup g in manager.getGroups()){
				GUILayout.BeginHorizontal ();
				g.fold = EditorGUILayout.Foldout (g.fold, g.Name);
				//Select the group
				if(GUILayout.Button("Select", GUILayout.Width(70))){
					Selection.objects = g.Objects.ToArray ();
				}

				//Clear the group
				if(GUILayout.Button("Clear", GUILayout.Width(70))){
					if(clearGroupCheck()){
						g.clearGroup ();
					}
				}

				//Delete the group
				if(GUILayout.Button("Delete", GUILayout.Width(70))){
					if(deleteGroupCheck()){
						manager.removalGroup = g;
					}
				}
				GUILayout.EndHorizontal ();

				if(g.fold){
					EditorGUI.indentLevel++;
					g.Name = EditorGUILayout.TextField (g.Name);

					g.foldO = EditorGUILayout.Foldout (g.foldO, "Objects: " + g.Objects.Count);
					if(g.foldO){
						EditorGUI.indentLevel++;
						//Make a scrollable window for the objects
						g.scrollPos = EditorGUILayout.BeginScrollView (g.scrollPos, GUILayout.Width (300),
																		GUILayout.Height (g.scrollHeight));
						for (int i = 0; i < g.Objects.Count; i++) {
							GUILayout.BeginHorizontal ();
							EditorGUILayout.LabelField ("Object: \t\t" + g.Objects [i].name);
							//Removal Button
							if(GUILayout.Button("Remove", GUILayout.Width(60))){
								if(removalCheck(g.Objects[i].name)){
										g.objectRemoval = i;
								}
							}
							GUILayout.EndHorizontal ();

							//If an object was set to be removed, remove it then reset the removal flag
							if(g.objectRemoval != -1){
								g.Objects.RemoveAt (g.objectRemoval);
								g.objectRemoval = -1;
								g.scrollHeight = g.calcSrollHeight ();
							}
						}

						EditorGUILayout.EndScrollView ();

						//Add new objects
						EditorGUILayout.BeginHorizontal ();
						GUILayout.Space (16 * EditorGUI.indentLevel);
						if(GUILayout.Button("Add Object(s)", GUILayout.Width(100))){
							if(selectionIsValid()){
								g.addObjects (Selection.gameObjects);
							}
						}
						EditorGUILayout.EndHorizontal ();

						GUILayout.BeginHorizontal();
						GUILayout.Space (EditorGUI.indentLevel * 16);
						GUILayout.Box("", GUILayout.ExpandWidth(true), GUILayout.Height(1));
						GUILayout.EndHorizontal();

						EditorGUI.indentLevel--;
					}
						
					EditorGUI.indentLevel--;
				}
			}

			//The button to add a new group
			if (GUILayout.Button ("Add Group", GUILayout.Width (100))){
				if(selectionIsValid()){
					manager.addGroup (Selection.gameObjects);

				}
			}

			if(GUI.changed){
				if(manager.removalGroup != null){
					manager.removeGroup ();
				}
			}
		}

		private bool selectionIsValid(){
			//The user needs to have at least one object selected in order to create
			//a new group.
			if(Selection.gameObjects.Length==0){
				EditorUtility.DisplayDialog("Error: Too few objects selected",
					"Please select one object  to complete this actoin.",
					"Okay");
				return false;
			}

			return true;
		}

		//Double check for removal dialouges
		private bool removalCheck(string message){
			return EditorUtility.DisplayDialog("Are you sure you want to remove:", message, "Yes", "No");
		}

		//Double check before clearing a group
		private bool clearGroupCheck(){
			return EditorUtility.DisplayDialog ("Are you sure you want to clear this group?",
				"This action cannot be undone.", "Yes", "No");
		}

		//Double check before deleting a group
		private bool deleteGroupCheck(){
			return EditorUtility.DisplayDialog ("Are you sure you want to delete this group?",
				"This action cannot be undone.", "Yes", "No");
		}
	}
}

