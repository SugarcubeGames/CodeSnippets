using System;
using System.Collections.Generic;
using UnityEngine;

namespace interARCHt.Systems.DesignOptions
{
	/// <summary>
	/// Holds information for the sub options within a design option.
	/// </summary>

	[Serializable]
	public class DOSubOption
	{
		[SerializeField] 
		private List<GameObject> objects;
		public List<GameObject> Objects{get{return objects;}}
		[SerializeField] 
		private bool fold; //is the option fold expanded?
		public bool Fold{get{return fold;} set{fold = value;}}
		[SerializeField] 
		private bool foldO; //is the object fold expanded?
		public bool FoldO{get{return foldO;} set{foldO = value;}}
		[SerializeField] 
		private string optionName; //The name of the option
		public string Name{get{return optionName;} set{optionName = value;}}

		//When the setActive button is selected, this will be set to true, and
		//a function in DOScr will change the active option.
		public bool changeActive;

		public bool isActive;

		public bool beingRemoved; //Has this option been marked for removal?

		public int objectRemoval;

		public Vector2 scrollPos; //position of scrollable window
		public int scrollHeight; //Height of the scroll window

		public DOSubOption(int seqNum, GameObject[] selection){
			objects = new List<GameObject>();

			foreach(GameObject g in selection){
					objects.Add (g);
			}

			optionName = "Option " + seqNum;
			fold = true;

			changeActive = false;
			isActive = false;

			scrollHeight = calcScrollHeight();

			objectRemoval = -1;
		}

		//called to display and modify the list
		public void DrawGUI(){

		}

		//Function for calculating the height of the scroll window
		public int calcScrollHeight(){
			return Mathf.Min (objects.Count * 21, 150);
		}

		//Turn on this option, turning on all of its objects... collision meshes might slow this down...
		public void makeActive(){
			foreach(GameObject g in objects){
				g.SetActive (true);
			}
			isActive = true;
			changeActive = false; //reset the changeActive flag
		}

		//Turn off all objects.  This is called by DOScr when changing the active option
		public void makeInactive(){
			foreach(GameObject g in objects){
				g.SetActive (false);
			}
			isActive = false;
		}


	}
}

