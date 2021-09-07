using System;
using System.Collections.Generic;
using UnityEngine;

namespace interARCHt.Systems.SelectionTools
{
	/// <summary>
	/// SL Manager keeps tracks of selections groups, 
	/// as well as which objects have been hidden in the
	/// scene.
	/// </summary>

	[Serializable]
	public class SLManager : MonoBehaviour
	{
		//Variables for the selection tools
		[SerializeField]
		List<MeshRenderer> hiddenObjects; //Objects who's mesh renderers have been turned off
		[SerializeField]
		List<GameObject> hiddenInHierarchy; //Objects that have been hidden in the hierarchy

		//Varaibles for the selection group system
		[SerializeField]
		List<SLGroup> selectionGroups; //A list of selection groups
		[SerializeField]
		public SLGroup removalGroup; //Is there a group slated for removal?

		public void init(){
			hiddenObjects = new List<MeshRenderer> ();
			hiddenInHierarchy = new List<GameObject> ();
			selectionGroups = new List<SLGroup> ();
			removalGroup = null;
		}

		public void isolateSelection(GameObject[] selected){
			if(selected.Length == 0){ //Minimum of one obect required for isolation
				Debug.Log ("There are no objects selected, cancelling isolation");
			}

			//Generate an array of all objects in the scene
			GameObject[] allObj =(GameObject[]) GameObject.FindObjectsOfType (typeof(GameObject));

			//Work through the array, generating an array of the root objects (highest hierarchy level)
			List<GameObject> curLevel = new List<GameObject> (); //curlevel = current level of the hierarchy we're working through
			foreach (GameObject g in allObj) {
				//use g.transform.root to get the topmost parent object of g; if that obj is not already in curLevel, add it
				GameObject root = g.transform.root.gameObject;
				if(!curLevel.Contains(root)){
					curLevel.Add (root);
				}
			}

			//Now that we have a list of the topmost hierarchy levels, we need to turn everything off that is not related
			//to the select object(s).  This will be done with a recursive function.
			hideLevel (curLevel, selected);
		}

		//Hide the selected objects
		public void hideSelection(GameObject[] selected){
			foreach(GameObject g in selected){

				if (g.name == "_interARCHt" || g.tag == "MainCamera") {
					continue;
				}

				MeshRenderer mr = g.GetComponent<MeshRenderer> ();
				if(mr != null){
					mr.enabled = false;
					hiddenObjects.Add (mr);
				}
				g.hideFlags = HideFlags.HideInHierarchy;
				hiddenInHierarchy.Add (g);
			}
		}

		//Unhide everything that's been hidden by the Hide or Isolate functions
		public void resetIsolation(){
			if(hiddenObjects == null || hiddenObjects.Count == 0){
				Debug.Log("There are no objects to restore.");
				return;
			}

			foreach(MeshRenderer m in hiddenObjects){
				if(m == null){ //Make sure hte object hasn't been deleted
					continue;
				}
				m.enabled = true;
			}

			foreach(GameObject g in hiddenInHierarchy){
				if(g == null){
					continue;
				}
				g.hideFlags = HideFlags.None;
			}

			hiddenObjects.Clear ();
			hiddenInHierarchy.Clear();

		}

		//Recursive method which will work through each level of the hierarchy, turning off objects which 
		//Do not have the selected objects as a child
		private void hideLevel(List<GameObject> curLevel, GameObject[] selected){
			//Cycle through each object on this level, checking to see if it has a selected object as
			//a child.If it doesn't, turn it off and add it to hidden objects.  If it does, add each
			//of its children to the new curLevel list.  Once we're done on this level, check to see if there
			//are any more child objects, if there are, start the process over with the new curLevel list.

			List<GameObject> nextLevel = new List<GameObject> ();

			foreach(GameObject g in curLevel){
				//Step 0: Make sure we don't add _interARCHt to the list, we don't want to modify it.
				//We also don't want to turn off the camera
				if(g.name == "_interARCHt" || g.tag=="MainCamera"){
					continue;
				}

				//Make sure that the current object isn't one of our child objects
				if(checkIsSelected(selected, g)){
					continue;
				}

				//If the current object is not a selected object, and has no children, we will check to see if
				//it has a mesh renderer.If it does, we turn it off, hide it in the hierarchy, and move on
				if(g.transform.childCount == 0){
					MeshRenderer mr = g.GetComponent<MeshRenderer> ();
					if(mr != null){
						mr.enabled = false;
						hiddenObjects.Add (mr);
					}
					g.hideFlags = HideFlags.HideInHierarchy;
					hiddenInHierarchy.Add (g);
					continue;
				}

				//If the current object is not a selected object and none of it's children are a selected object
				//then we will turn off all the mesh renders of it's child objects and hide it in the hierarchy.
				if(!checkIsChild(g, selected)){
					//We want to turn off every child in the hierarchy, so we have to get all transforms, 
					//and then check for mesh renderers.
					foreach(Transform t in g.transform){
						MeshRenderer mr = t.gameObject.GetComponent<MeshRenderer> ();
						if(mr != null){
							mr.enabled = false;
							hiddenObjects.Add (mr);
						}
						//Now check for mesh renderers in it's children
						MeshRenderer[] mrs = t.gameObject.GetComponentsInChildren<MeshRenderer> ();
						if(mrs.Length > 0){
							foreach(MeshRenderer m in mrs){
								m.enabled = false;
								hiddenObjects.Add (m);
							}
						}
						t.hideFlags = HideFlags.HideInHierarchy;
						hiddenInHierarchy.Add (t.gameObject);
					}
					g.hideFlags = HideFlags.HideInHierarchy;
					hiddenInHierarchy.Add (g);
					continue;
				}

				//If none of the previous conditions are true, that means that one of the current object's
				//children is a selected object, so we need to pass it to the next level.
				foreach(Transform t in g.transform){
					nextLevel.Add (t.gameObject);
				}

			}

			//Now that we've run through each object ont his level, we need to check to see if there
			//are any more objects to check.  If there are, start over with the new list of objects.
			if(nextLevel.Count > 0){
				hideLevel (nextLevel, selected);
			}

			//Once we've run through all the levels, all that's left is to exist the recursion.
			return;
		}

		//Check to see if the given object is one of our selected objects
		private bool checkIsSelected(GameObject selection, GameObject curObj){
			if(selection == curObj){
				return true;
			}
			return false;
		}

		//Check to see if the given object is contained in a list of selected objects
		private bool checkIsSelected(GameObject[] selection, GameObject curObj){
			foreach(GameObject g in selection){
				if(g == curObj){
					return true;
				}
			}
			return false;
		}

		//Check a single parent-child relationship
		private bool checkIsChild(GameObject parent, GameObject child){
			if(child.transform.IsChildOf(parent.transform)){
				return true;
			}
			return false;
		}

		//Check a child against multiple potential parents
		private bool checkIsChild(GameObject[] parents, GameObject child){
			foreach(GameObject g in parents){
				if(child.transform.IsChildOf(g.transform)){
					return true;
				}
			}
			return false;
		}

		//Check a group of children against a single potential parent
		private bool checkIsChild(GameObject parent, GameObject[] children){
			foreach(GameObject g in children){
				if(g.transform.IsChildOf(parent.transform)){
					return true;
				}
			}
			return false;
		}


		/*
		 * Selection  Group Manager Methods
		 * */

		public List<SLGroup> getGroups(){
			return selectionGroups;
		}

		//Create a new group
		public void addGroup(GameObject[] selected){
			selectionGroups.Add (new SLGroup (selected));
		}

		//Remove a group from the list
		public void removeGroup(){
			if(removalGroup != null){
				if (selectionGroups.Contains (removalGroup)) {
					selectionGroups.Remove (removalGroup);
				}
			}
		}
			
	}
}

