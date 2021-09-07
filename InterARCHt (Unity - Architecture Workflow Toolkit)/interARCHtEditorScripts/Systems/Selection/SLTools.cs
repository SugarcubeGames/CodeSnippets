using System;
using UnityEditor;
using UnityEngine;
using interARCHt.Systems.SelectionTools;

namespace interARCHtEditorScripts.Systems.SelectionTools
{
	//The selections tools will handle the hiding, isolation, and unhiding
	//of objects in the editor.  None of these will create a window, so it
	//will be necessary to find SLManager each time.
	public class SLTools
	{
		[MenuItem("interARCHt/Selection Tools/Isolate Selection", false, 0)]
		static void isolateSel(){
			//Get the SLManager from _interARCHt
			SLManager manager = loadManager();

			manager.isolateSelection (Selection.gameObjects);

			//Once it's done everyhting in isolateSelection, we need to make sure that
			//the hierarchy window redraws
			EditorApplication.RepaintHierarchyWindow ();
		}

		[MenuItem("interARCHt/Selection Tools/Hide Selected", false, 1)]
		static void hidSelected(){
			SLManager manager = loadManager ();
			manager.hideSelection (Selection.gameObjects);
			EditorApplication.RepaintHierarchyWindow ();
		}

		[MenuItem("interARCHt/Selection Tools/Restore Hidden", false, 2)]
		static void resetHid(){
			SLManager manager = loadManager ();

			manager.resetIsolation ();

			EditorApplication.RepaintHierarchyWindow ();
		}


		private static SLManager loadManager(){
			//Find the _interARCHt gameobject, and load the MOManager from it
			GameObject go = GameObject.Find("_interARCHt");
			SLManager manager;
			//If we don't find the gameobject, create one and add teh MOManager script to it
			if(go == null){
				go = new GameObject();
				go.name = "_interARCHt";
				//go.hideFlags = HideFlags.HideInHierarchy;

				go.AddComponent(typeof(SLManager));
				manager = go.GetComponent<SLManager>();
				manager.init ();
				return manager;
			}

			//If we found the gameobject, try to load the manager script from it
			manager = go.GetComponent<SLManager>();

			if(manager == null){
				go.AddComponent(typeof(SLManager));
				manager = go.GetComponent<SLManager>();
				manager.init ();
			}

			return manager;
		}
	}
}

