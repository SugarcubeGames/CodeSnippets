using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

using interARCHt.Systems.MaterialOptions;

namespace interARCHtEditorScripts
{
	public class MOGUIManager
	{
		public static void DrawGUI(MOManager scr){
			scr.FoldS = EditorGUILayout.Foldout(scr.FoldS, "Single-Object Options: " 
			                                    + scr.SingleObjects.Count);
			if(scr.FoldS){
				foreach(MOSingle mo in scr.SingleObjects){
					MOGUISingle.DrawGUI(mo);
				}

				//GUILayout.Box("", GUILayout.ExpandWidth(true), GUILayout.Height(1));

				if(GUILayout.Button("Add", GUILayout.Width(75))){
					addSingleObjectOption(scr);
				}

				GUILayout.Box("", GUILayout.ExpandWidth(true), GUILayout.Height(1));
			}

			scr.FoldG = EditorGUILayout.Foldout(scr.FoldG, "Grouped-Object Options: " + scr.GroupObjects.Count);
			if(scr.FoldG){

				foreach(MOGroup mo in scr.GroupObjects){
					MOGUIGroup.DrawGUI(mo);
				}

				if(GUILayout.Button("Add", GUILayout.Width(75))){
					addGroupObjectOption(scr);
				}
			}

			if(GUI.changed){
				//Debug.Log ("Gui.changed");
				//Check for options being marked for removal
				List<MOSingle> singleRemove = new List<MOSingle>();

				foreach(MOSingle mo in scr.SingleObjects){
					if(mo.checkDelete()){
						singleRemove.Add(mo);
					}
				}

				if(singleRemove.Count>0){
					foreach(MOSingle mo in singleRemove){
						mo.removeOptionIdentifier();
						scr.SingleObjects.Remove(mo);
					}
				}

				//Do the same thing for group options
				List<MOGroup> groupRemove = new List<MOGroup> ();

				foreach(MOGroup mo in scr.GroupObjects){
					if(mo.checkDelete()){
						groupRemove.Add (mo);
					}
				}

				if(groupRemove.Count>0){
					foreach(MOGroup mo in groupRemove){
						mo.removeOptionIdentifier ();
						scr.GroupObjects.Remove (mo);
					}
				}
			}
		}

		/*************************
		 * Add and remove check functions
		 * ************/
		public static void addSingleObjectOption(MOManager scr){
			//Make sure the proper number of objects is selected
			if(Selection.gameObjects.Length != 1){
				if(Selection.gameObjects.Length == 0){
					EditorUtility.DisplayDialog("Error: Too few objects selected",
					                            "Please select one object to create a new Design Option.",
					                            "Okay");
					return;
				} else if(Selection.gameObjects.Length > 1){
					EditorUtility.DisplayDialog("Error: Too many objects selected",
					                            "Please select no more than one object.",
					                            "Okay");
					return;
				}
			}

			//Now make sure that the selected object has a mesh renderer
			if(Selection.activeGameObject.GetComponent(typeof(MeshRenderer)) == null){
				EditorUtility.DisplayDialog("Error: Object has no Mesh Renderer",
				                            "The selected object does not have a mesh renderer, and is invalid for material switching.",
				                            "Okay");
				return;
			}


			//Make sure that the selected object isn't already part of another option

			if(Selection.activeGameObject.GetComponent(typeof(MOSingleObj)) != null
			   ||Selection.activeGameObject.GetComponent(typeof(MOSingleObj)) != null){
				EditorUtility.DisplayDialog("Error: Object already has Material Option",
				                            "The selected object already has a material option.",
				                            "Okay");
				return;
			}

			scr.SingleObjects.Add (new MOSingle(Selection.gameObjects[0]));
		}

		public static void addGroupObjectOption(MOManager scr){
			List<GameObject> objs = checkIsValidGroupObject();

			if(objs.Count == 0){
				return;
			}

			scr.GroupObjects.Add(new MOGroup(objs));
		}

		//Set to public and static so that MOGroup can access it.
		public static List<GameObject> checkIsValidGroupObject(){

			List<GameObject> objs = new List<GameObject>();
			List<GameObject> invalidObjsRenderer = new List<GameObject>();
			List<GameObject> invalidObjsOption = new List<GameObject>();

			//Make sure the proper number of objects is selected
			if(Selection.gameObjects.Length == 0){
				EditorUtility.DisplayDialog("Error: Too few objects selected",
				                            "Please select at least one object to create a new Material Option.",
				                            "Okay");
				return objs;
			} 

			foreach(GameObject obj in Selection.gameObjects){
				if(obj.GetComponent(typeof(MeshRenderer)) == null){
					invalidObjsRenderer.Add(obj);
					continue;
				}

				if(obj.GetComponent(typeof(MOSingleObj)) != null
				   || obj.GetComponent(typeof(MOGroupObj)) != null){
					invalidObjsOption.Add(obj);
					continue;
				}

				objs.Add(obj);
			}

			//Error message, if necessary:
			if(invalidObjsOption.Count > 0 || invalidObjsRenderer.Count > 0){
				string error = "";

				if(invalidObjsOption.Count > 0){
					error += "These objects are aready part of another material option:\n";

					foreach(GameObject obj in invalidObjsOption){
						error += "\t" + obj.gameObject.name + "\n";
					}

					error += "\n";
				}

				if(invalidObjsRenderer.Count >0){
					error += "These objects do not have mesh renderers, and are invalid for inclusion in a material option:\n";

					foreach(GameObject obj in invalidObjsRenderer){
						error += "\t" + obj.gameObject.name + "\n";
					}
				}

				EditorUtility.DisplayDialog("Error: Certain objects could not be added:",
				                            error, "Okay");

			}

			return objs;
		}
	}
}

