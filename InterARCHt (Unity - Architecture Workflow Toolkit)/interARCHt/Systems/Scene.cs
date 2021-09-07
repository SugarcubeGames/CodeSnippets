using System;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

using interARCHt.Systems.DesignOptions;
using interARCHt.Systems.Elevator;

// Scene will be used to replace _interARCHt, and will store all manager scripts.
// There will be a static reference back to the current scene so that it can be
// easily accessed from any class.  This will also remove the reliance on
// the GetComponent method for _interARCHt.
namespace interARCHt
{
	/// <summary>
	/// Stores scene data related to the _interARCHt system.
	/// 
	/// Use Scene.scene to access the current scene
	/// </summary>
	public class Scene
	{
		public static Scene current; //Static accessor for current scene

		//System managers
		public ELManager elManager;
		public DOManager doManager;
#if UNITY_EDITOR
		[MenuItem("Reload Scene Data", false, 50000)]
		public static void init(){
			current = new Scene ();
		}
#endif
		//Note: This should only be called once per session, the first time one of the manager windows
		//is opened.  If it is called more than once, somthing is wrong.
		public Scene(){
			Debug.Log ("Loading Scene data.");
			//Work thorugh each manager in the scene, trying to load it.  If
			//any manager fails to load (returns false), instantiate a new one and save it.

			//Feature Point:

			//Elevator:
#if UNITY_EDITOR
			elManager = SaveLoad.LoadELManager ();
			if(elManager == null){
				Debug.Log ("Elevator Manager failed to create.");
			}

			//Design Options:
			doManager = SaveLoad.LoadDOManager();
			if(doManager == null){
				Debug.Log ("Design Option manager failed to create.");
			}
#endif
		}

	}
}

