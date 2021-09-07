using System;
using System.Collections.Generic;
using UnityEngine;

namespace interARCHt.Systems.Elevator
{
	/// <summary>
	/// This script is used to define an elevator element, and manage
	/// the levels in the elevator, as well as alternate 'output' options
	/// when an elevator level is selected (in the case of 2-sided elevators)
	/// </summary>
	/// 
	/// There are multiple ways of activating an elevator - either through a trigger field
	/// or by attaching and "ELButton" script to an object.  The trigger field will activate
	/// as soon as the field is entered, the button script will activate when the player
	/// presses 'f' when looking at the object the script is attached to
	/// 
	/// TODO: Develop elevator class, need to do more important classes right now
	[Serializable]
	public class ELScr : ScriptableObject
	{
		public string eName;
		public int numLevels;
		public List<int> levelHeights;
		public GameObject testObj;

		public void OnEnable()
		{
		}

		public void init(int numLevels){
			eName = "Elevator";
			this.numLevels = numLevels;
			levelHeights = new List<int> ();
			//Add a level height for each level
			for(int i = 0; i< numLevels; i++){
				levelHeights.Add (10); //default to ten ft. per level.
			}
		}
	}
}

