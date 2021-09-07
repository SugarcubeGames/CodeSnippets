using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace interARCHt
{
	public static class AssetManager
	{
#if UNITY_EDITOR
		//Create an asset from an existing scriptable object
		public static ScriptableObject Create<T>(string path) where T: ScriptableObject
		{
			if(!Directory.Exists(SaveLoad.folderName)){
				Directory.CreateDirectory (SaveLoad.folderName);
			}
			T asset = ScriptableObject.CreateInstance<T> ();

			if (path == "") {
				path = "Assets/Resources/data/";
			}

			AssetDatabase.CreateAsset (asset, path);
			AssetDatabase.SaveAssets ();

			return asset;
		}

		public static void AddSubObject(UnityEngine.Object asset, UnityEngine.Object subObject){
			if(asset == null){
				Debug.Log ("Error: Asset does not exist.");
				return;
			}
			AssetDatabase.AddObjectToAsset (subObject, asset);
			AssetDatabase.SaveAssets ();
		}

		public static List<T> GetSubObjectOfType<T>(UnityEngine.Object asset) where T:UnityEngine.Object {
			UnityEngine.Object[] objs = AssetDatabase.LoadAllAssetsAtPath (AssetManager.GetAssetPath (asset));
			List<T> ofType = new List<T> ();

			foreach (UnityEngine.Object o in objs) {
				if (o is T) {
					ofType.Add (o as T);
				}
			}
			return ofType;
		}

		public static string GetAssetPath(UnityEngine.Object asset){
			return AssetDatabase.GetAssetPath (asset);
		}
#endif
	}
}

