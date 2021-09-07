using System;
using System.Collections.Generic;
using UnityEngine;

namespace interARCHt.Systems.Elevator
{
	[Serializable]
	public class ELManager : ScriptableObject
	{
		//A list of elevators in the scene
		//public List<ELScr> elevators { get; private set;}
		public List<ELScr> elevators ;
		//A list of paths for each ELScr added to elevators, needed for
		//loading the subobjects back into the elevators list at load.
		public List<string> paths;
 
		public ELScr this[int i] {
			get{ return elevators [i];}
			private set{}
		}
		public int Count{ get { return elevators.Count; } private set{}}

		public void OnEnable()
		{
			elevators = new List<ELScr> (); //Create a new list of elevators
			paths = new List<string> ();
		}

		public void setDirty(){

		}
	}
}

