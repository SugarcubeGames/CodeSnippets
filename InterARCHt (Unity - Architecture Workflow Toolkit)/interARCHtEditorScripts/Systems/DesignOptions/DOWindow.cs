using System;
using UnityEditor;
using UnityEngine;
using interARCHt.Systems.DesignOptions;
using interARCHt;

namespace interARCHtEditorScripts.Systems.DesignOptions
{
	public class DOWindow : EditorWindow
	{
		private DOManager manager;

		[MenuItem("interARCHt/Design Option Manager", false, 1000)]
		static void init(){
			GetWindow<DOWindow> ();
		}

		void OnEnable(){
			//try to access the scene, if it is null, call the scene load function.
			if(Scene.current == null){
				Scene.current = new Scene ();
			}

			manager = Scene.current.doManager;
		}

		void onFocus(){
			hideFlags = HideFlags.HideAndDontSave;
			if(manager == null){
				loadManager ();
			}
		}

		void OnGUI(){
			if(manager == null){
				return;
			}
			managerGUI (manager);

			if(GUI.changed){
				EditorUtility.SetDirty (this);
			}
		}

		//This method is defunct, since manager will no longer stored on an object
		//in the scene.
		private void loadManager(){

			//Find the _interARCHt gameobject, and load the MOManager from it
			GameObject go = GameObject.Find("_interARCHt");
			//If we don't find the gameobject, create one and add teh MOManager script to it
			if(go == null){
				Debug.Log ("interARCHt doesn't exist");
				go = new GameObject();
				go.name = "_interARCHt";
				//go.hideFlags = HideFlags.HideInHierarchy;

				go.AddComponent(typeof(DOManager));
				//manager = go.GetComponent<DOManager>();
				//manager.init ();
				return;
			}

			//If we found the gameobject, try to load the manager script from it
			//manager = go.GetComponent<DOManager>();

			if(manager == null){
				go.AddComponent(typeof(DOManager));
				//manager = go.GetComponent<DOManager>();
				//manager.init ();
			}
		}

		/************************************
		 * GUIS
		 * **********************************/
		//--------------------------------------------------------------------------
		//Manager GUI
		private void managerGUI(DOManager scr){
			GUILayout.Label ("Design Options");
			foreach(DOScr d in scr.designOptions){
				optionGUI (d);
			}

			if(GUILayout.Button("Add Design Option", GUILayout.Width(150))){
				scr.addOption ();
			}

			if(GUI.changed){
				DOScr scrToRemove = null; //holder for the option being removed.\

				foreach(DOScr d in scr.designOptions){
					if(d.beingRemoved){
						scrToRemove = d;
					}
				}

				if(scrToRemove != null){
					scr.designOptions.Remove (scrToRemove);
				}

				EditorUtility.SetDirty (scr);
			}
		}

		//------------------------------------------------------------------------------
		//Option GUI
		private void optionGUI(DOScr scr){
			EditorGUILayout.BeginHorizontal ();
			scr.Fold = EditorGUILayout.Foldout (scr.Fold, scr.Name);
			if(GUILayout.Button("Remove", GUILayout.Width(75))){
				if (removalCheck (scr.Name)){
					//Set the removal flag to true
					scr.beingRemoved = true;
				}
			}

			EditorGUILayout.EndHorizontal ();
			if(scr.Fold){
				EditorGUI.indentLevel ++;
				scr.Name = EditorGUILayout.TextField ("Name: ", scr.Name);

				//Show each of the options within this design Option
				foreach(DOSubOption d in scr.Options){
					subObjectGUI (d);
				}

				//Add an option comprised of one or more objects
				EditorGUILayout.BeginHorizontal ();
				GUILayout.Space (19 * EditorGUI.indentLevel);
				if(GUILayout.Button("Add Option", GUILayout.Width(150))){
					if(Selection.gameObjects.Length == 0){
						EditorUtility.DisplayDialog("Error: Too few objects selected",
						                            "Please select at least one object to create a new Option.",
						                            "Okay");

					} else {
						scr.Options.Add (new DOSubOption (scr.Options.Count+1, Selection.gameObjects));
					}
				}
				EditorGUILayout.EndHorizontal ();

				GUILayout.BeginHorizontal();
				GUILayout.Space (EditorGUI.indentLevel * 16);
				GUILayout.Box("", GUILayout.ExpandWidth(true), GUILayout.Height(1));
				GUILayout.EndHorizontal();

				EditorGUI.indentLevel--;
			}

			if(GUI.changed){
				bool changing = false;
				DOSubOption scrForRemoval = null; //Holder for an option being removed
				//Check to see if any of the options has had their changeActive flag set to true
				foreach(DOSubOption d in scr.Options){
					if(d.changeActive){
						changing = true;
					}
					if(d.beingRemoved){
						scrForRemoval = d;
					}
				}
				//If one is changing, turn it on and the others off
				if(changing){
					foreach(DOSubOption d in scr.Options){
						if(d.changeActive){
							d.makeActive ();
						} else {
							d.makeInactive();
						}
					}
				}

				//Remove any option that's been marked for removal
				if(scrForRemoval != null){
					scr.Options.Remove (scrForRemoval);
				}
			}
		}

		//------------------------------------------------------------------------------
		//Sub-Option GUI
		private void subObjectGUI(DOSubOption scr){
			EditorGUILayout.BeginHorizontal ();
			if(scr.isActive == true){
				scr.Fold = EditorGUILayout.Foldout (scr.Fold, scr.Name+ ": Active");
			} else {
				scr.Fold = EditorGUILayout.Foldout (scr.Fold, scr.Name);
			}
			if(GUILayout.Button("Set Active", GUILayout.Width(75))){
				scr.changeActive = true; //Mark this option to be set as the active option
			}
			if(GUILayout.Button("Remove", GUILayout.Width(75))){
				scr.beingRemoved = true; //Mark this option for removal, handled by DOScr
			}
			EditorGUILayout.EndHorizontal ();

			if(scr.Fold){
				EditorGUI.indentLevel++;
				scr.Name = EditorGUILayout.TextField ("Name: ", scr.Name);
				scr.FoldO = EditorGUILayout.Foldout (scr.FoldO, "Objects: " + scr.Objects.Count);

				if(scr.FoldO){
					EditorGUI.indentLevel++;
					//Make the window scrollable
					//TODO: If scrollHeight >150, draw a box around the area
					scr.scrollPos = EditorGUILayout.BeginScrollView (scr.scrollPos, GUILayout.Width (300), 
					                                             GUILayout.Height(scr.scrollHeight));
					for(int i = 0; i<scr.Objects.Count; i++){
						GUILayout.BeginHorizontal();
						EditorGUILayout.LabelField("Object: \t\t" + scr.Objects[i].name);
						//Remove button
						if(GUILayout.Button("Remove", GUILayout.Width(60))){
							if(removalCheck(scr.Objects[i].name)){
								scr.objectRemoval = i;
							}
						}
						GUILayout.EndHorizontal();

						//If an object was set to be removed, remove it then teset objectremoval
						if(scr.objectRemoval != -1){
							scr.Objects.RemoveAt(scr.objectRemoval);
							scr.objectRemoval = -1;
							scr.scrollHeight = scr.calcScrollHeight ();
						}
					}

					EditorGUILayout.EndScrollView ();

					//Add new Objects
					EditorGUILayout.BeginHorizontal ();
					GUILayout.Space (16 * EditorGUI.indentLevel);
					if(GUILayout.Button("Add Objects", GUILayout.Width(100))){
						//Add selected obbjects to the option after making sure they are
						//not already a part of the option.
						addObjectsWithCheck (scr);
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
		//Double check for removal dialouges
		private bool removalCheck(string message){
			return EditorUtility.DisplayDialog("Are you sure you want to remove:", message, "Yes", "No");
		}

		/*********************
		 * Modify suboptions
		 * *******************/

		private void addObjectsWithCheck(DOSubOption scr){
			bool duplicateCheck = false;
			foreach(GameObject g in Selection.gameObjects){
				if(!scr.Objects.Contains(g)){
					scr.Objects.Add (g);
				} else {
					duplicateCheck = true;
				}
			}

			if(duplicateCheck){
				EditorUtility.DisplayDialog ("Warning:", 
				                             "Certain objects were excluded because they are already included in this option.", "Okay");
			}

			//Recalculate the scroll window height
			scr.scrollHeight = scr.calcScrollHeight ();
		}
	}
}

