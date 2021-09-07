#pragma warning disable 0414

using System;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using interARCHt.Systems.DesignOptions;
using interARCHt.Systems.Elevator;

namespace interARCHt
{
	/// <summary>
	/// This is a static class which handles the saving and loading of
	/// data for interARCHT
	/// </summary>

	//For now, we will test saving and loading the elevator script.
	public static class SaveLoad
	{

		/*
		 * Each system manager in the scene will have a different load method, this
		 * is done to save time in the loading , as well as to try and prevent issues
		 * with accidentally overwriting data.  Each save method will be called by a button in their
		 * respective editor.  The load methods will be called automatically when the associated
		 * window is opened.
		 * 
		 * All save and load functions will receive the specific manager they are saving / loading.
		 * This is mainly being done for simplicity so it's not necessary to keep track
		 * of function names for each system.  Load functoin receive a reference to the manager they
		 * are supposed to be loading, so it's not necessary to return anything.
		 * ----
		 * Looking at Assets and scriptable objects for saving data, the save function now only needs
		 * to call AssetDatabase.SaveAssets() after marking the script to be saved in the window
		 * scripte (EditorUtility.SetDirty(manager)).  As a result, there will only be one save
		 * function, and multiple load functions.
		 * */


		//Static strings for file locations and names.
		//Note: Each location begins with Application.persistentDataPath, which points to the specific
		//location save files are intended to go in Unity, and automatically adjsts for different platforms.
		public static string folderName = "Assets/Resources/data/";

		private static string FPPath = folderName + "FP.asset";
		private static string ELPath = folderName + "EL.asset";
		private static string MOPath = folderName + "MO.asset";
		private static string DOPath = folderName + "DO.asset";
#if UNITY_EDITOR
		public static void Save(){
			AssetDatabase.SaveAssets ();
		}

		/*********************************************************************
		 * Feature Point
		 * *******************************************************************/

		/*********************************************************************
		 * Elevator
		 * Thoughts:
		 * The ELManager itself does not need to save, it only exists to
		 * manipulate and store the elevator scripts during an editing
		 * session.  When it comes time to save the only thing we're needing
		 * to store are the actual ELScrs stored in the manager's list, since
		 * they contain all of the important data.  The manager will remain a
		 * scriptable object so that is can be used as the base for storing 
		 * the ELScrs associated with it.
		 * *******************************************************************/

		public static ELManager LoadELManager(){
			//Make sure that savedata exists before trying to load it
			ELManager manager = (ELManager)AssetDatabase.LoadAssetAtPath (ELPath, typeof(ELManager));
			if(manager==null){
				manager = (ELManager)AssetManager.Create<ELManager> (ELPath);

				//The newly-create ELManager has no elevators in it, so we don't need to
				//load anything here.

			} else {
				//Load each elevator from the manager into the manager's elevator list
				foreach(ScriptableObject s in AssetManager.GetSubObjectOfType<ELScr>(manager)){
					manager.elevators.Add ((ELScr)s);
				}
			}
			return manager; 
		}

		/*Design Options*/
		public static DOManager LoadDOManager(){
			DOManager manager = (DOManager)AssetDatabase.LoadAssetAtPath (DOPath, typeof(DOManager));
			if(manager == null){
				manager = (DOManager)AssetManager.Create<DOManager> (DOPath);
			} else {

			}
			return manager;
		}
#endif
	}
}

