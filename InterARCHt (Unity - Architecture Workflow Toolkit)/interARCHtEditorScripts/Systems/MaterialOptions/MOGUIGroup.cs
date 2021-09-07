using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using interARCHt.Systems.MaterialOptions;

namespace interARCHtEditorScripts
{
	public class MOGUIGroup
	{
		public static void DrawGUI(MOGroup scr){
			EditorGUI.indentLevel++;

			scr.Fold = EditorGUILayout.Foldout(scr.Fold, scr.Name);

			if(scr.Fold){
				EditorGUI.indentLevel++;
				scr.Name = EditorGUILayout.TextField("Name:", scr.Name);

				scr.FoldO = EditorGUILayout.Foldout(scr.FoldO, "Objects: " + scr.Objects.Count);
				if(scr.FoldO){
					EditorGUI.indentLevel++;
					for(int i = 0; i< scr.Objects.Count; i++){
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
							UnityEngine.Object.DestroyImmediate(
								scr.Objects[scr.objectRemoval].GetComponent<MOGroupObj>());
							scr.Objects.RemoveAt(scr.objectRemoval);
							scr.objectRemoval = -1;
						}
					}

					//Button to add new objects
					GUILayout.BeginHorizontal ();
					GUILayout.Space (16 * EditorGUI.indentLevel);
					if(GUILayout.Button("Add Object(s)", GUILayout.Width (100))){
						List<GameObject> objs = MOGUIManager.checkIsValidGroupObject();

						if(objs.Count == 0){
							return;
						}

						foreach(GameObject obj in objs){
							scr.Objects.Add(obj);

							obj.AddComponent<MOGroupObj>();
							MOGroupObj scrG = (MOGroupObj)obj.GetComponent(typeof(MOGroupObj));
							scrG.init (scr);
						}
					}
					GUILayout.EndHorizontal ();
					EditorGUI.indentLevel--;
				}

				scr.FoldM = EditorGUILayout.Foldout(scr.FoldM, "Materials: " + scr.Materials.Count);
				if(scr.FoldM){
					EditorGUI.indentLevel++;
					for(int i = 0; i<scr.Materials.Count; i++){
						GUILayout.BeginHorizontal();
						scr.Materials[i] = (Material) EditorGUILayout.ObjectField(
							scr.Materials[i], typeof(Material), false, GUILayout.Width(300));

						if(GUILayout.Button("Active", GUILayout.Width(60))){
							scr.setMaterial(i);
						}
						if(GUILayout.Button("Remove", GUILayout.Width (60))){
							if(scr.Materials[i] == null){
								if(removalCheck("null")){
									scr.Materials.RemoveAt(i);
								}
							} else {
								if(removalCheck(scr.Materials[i].name)){
									scr.Materials.RemoveAt(i);
								}
							}
						}
						GUILayout.EndHorizontal();
					}

					GUILayout.BeginHorizontal();
					GUILayout.Space(EditorGUI.indentLevel * 16);
					if(GUILayout.Button ("Add Material", GUILayout.Width(100))){
						scr.Materials.Add(null);
					}
					GUILayout.EndHorizontal();

					GUILayout.BeginHorizontal();
					GUILayout.Space (EditorGUI.indentLevel * 16);
					GUILayout.Box("", GUILayout.ExpandWidth(true), GUILayout.Height(1));
					GUILayout.EndHorizontal();

					EditorGUI.indentLevel--;
				}

				GUILayout.BeginHorizontal();
				GUILayout.Space(16*EditorGUI.indentLevel);
				if(GUILayout.Button("Remove", GUILayout.Width(75))){
					if(removalCheck(scr.Name)){
						scr.removeFlag = true;
					}
				}
				GUILayout.EndHorizontal();

				EditorGUI.indentLevel--;
			}
			EditorGUI.indentLevel--;
		}

		public static bool removalCheck(string message){
			return EditorUtility.DisplayDialog("Are you sure you want to remove:", message, "Yes", "No");
		}


	}
}

