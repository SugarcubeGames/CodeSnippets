using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using interARCHt.Systems.MaterialOptions;

namespace interARCHtEditorScripts
{
	public class MOGUISingle
	{
		public static void DrawGUI(MOSingle scr){
			EditorGUI.indentLevel ++;
			scr.Fold = EditorGUILayout.Foldout(scr.Fold, scr.Name);

			if(scr.Fold){
				EditorGUI.indentLevel ++;
				scr.Name = EditorGUILayout.TextField("Name:", scr.Name);
				EditorGUILayout.LabelField("Object: \t\t" + scr.Obj.name);

				//Show each material
				scr.MatFold = EditorGUILayout.Foldout(scr.MatFold, "Materials: " + scr.Materials.Count);
				if(scr.MatFold){
					EditorGUI.indentLevel++;

					for(int i = 0; i<scr.Materials.Count; i++){
						GUILayout.BeginHorizontal();
						scr.Materials[i] = (Material) EditorGUILayout.ObjectField(
							scr.Materials[i], typeof(Material), false, GUILayout.Width(300));

						if(GUILayout.Button("Active", GUILayout.Width(60))){
							scr.setActive(i);
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
						scr.RemoveFlag = true;
					}
				}
				GUILayout.EndHorizontal();

				EditorGUI.indentLevel --;
			}

			EditorGUI.indentLevel --;
		}

		public static bool removalCheck(string message){
			return EditorUtility.DisplayDialog("Are you sure you want to remove:", message, "Yes", "No");
		}

	}
}

