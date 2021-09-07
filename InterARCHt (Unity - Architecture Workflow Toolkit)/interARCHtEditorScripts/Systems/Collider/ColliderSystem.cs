using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace interARCHtEditorScripts.Systems.Colliders.Editor
{
	/// <summary>
	/// The collider system will automatially apply or remove
	/// Mesh colliders to all selected Gameobjects and their
	/// child objects.
	/// </summary>
	public class ColliderSystem
	{
		[MenuItem("interARCHt/Colliders/Add Mesh Collider(s)", false, 100)]
		static void ApplyMeshCollider(){

			//This method will generate a list of selected elements and their children which have meshes.
			//If an object does not have a mesh, we can reasonably assume that it does not need a collider.

			List<Transform> meshedChildren = new List<Transform> (); //A list of all children that have meshes

			//Generate a list of selected elements
			GameObject[] selected = Selection.gameObjects;

			//Work through each of the selected objects, gathering all the children that have meshes
			foreach(GameObject g in selected){
				Transform[] children = g.GetComponentsInChildren<Transform> ();
				foreach(Transform t in children){
					//see if it has a mesh, if it does, add it to the list
					if(t.gameObject.GetComponent(typeof(MeshFilter)) != null){
						meshedChildren.Add (t);
					}
				}
			}

			//Once we have the list of meshed children, iterate through them to see if they already
			//have a collider on them.  If they don't add one.
			foreach(Transform t in meshedChildren){
				//Check for an existing collider, if none exist, add one
				if(t.gameObject.GetComponent(typeof(Collider)) != null){
					continue;
				}
				t.gameObject.AddComponent<MeshCollider> ();
			}
		}

		//Remove all colliders from the selected GameObjects and
		//their children
		[MenuItem("interARCHt/Colliders/Remove Colliders", false, 111)]
		static void ApplyCollider(){

			//Generate a list of the selected elements
			GameObject[] selected = Selection.gameObjects;

			//Work through each of the selected objects, gathering all the children that have meshes
			foreach(GameObject g in selected){
				Transform[] children = g.GetComponentsInChildren<Transform> ();
				foreach(Transform t in children){
					//If it has a collider, remove it
					if(t.gameObject.GetComponent(typeof(Collider)) != null){
						GameObject.DestroyImmediate (t.gameObject.GetComponent (typeof(Collider)));
					}
				}
			}
		}
	}
}

