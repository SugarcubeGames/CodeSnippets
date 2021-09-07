using System;
using System.Collections.Generic;
using UnityEngine;

namespace interARCHt.Systems.SelectionTools
{
	//Stores the objects associated with a selection group
	[Serializable]
	public class SLGroup
	{
		[SerializeField]
		private List<GameObject> objects;
		public List<GameObject>Objects{ get{return objects;}}

		[SerializeField]
		private string name;
		public string Name{get{return name;} set{name = value;}}

		public bool fold; //Is this group folded out?
		public bool foldO; //Is this group's object list folded out?

		public Vector2 scrollPos; //Managers the posistion of the scrollable window for object display
		public int scrollHeight; //How tall is the scroll window? (max 150)

		public int objectRemoval; //Tracker for objects slated for removal

		public SLGroup (GameObject[] selected)
		{
			objects = new List<GameObject> ();
			name = "New Group";
			fold = true;
			foldO = true;

			scrollPos = new Vector2(0,0);
			scrollHeight = calcSrollHeight();

			objectRemoval = -1;

			foreach(GameObject g in selected){
				//Make sure that the current object isn't already included in the group
				if(!objects.Contains(g)){
					objects.Add (g);
				}
			}
		}

		public void addObjects(GameObject[] selected){
			foreach(GameObject g in selected){
				if(!objects.Contains(g)){
					objects.Add (g);
				}
			}
		}

		public void removeObjects(GameObject[] selected){
			foreach(GameObject g in selected){
				if(objects.Contains(g)){
					objects.Remove (g);
				}
			}
		}

		public void clearGroup(){
			objects.Clear();
		}

		public int calcSrollHeight(){
			return Mathf.Min (objects.Count * 21, 150);
		}
	}
}

