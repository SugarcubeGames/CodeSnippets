using System;
using System.Collections.Generic;
using UnityEngine;

namespace interARCHt.Systems.MaterialOptions
{
	[System.Serializable]
	public class MOManager : MonoBehaviour {

		//Since GUI operations are handled in the static class MOEditorGUI, it's necessary
		//to be able to access all of this information from outside of the script, as a result
		//I'll be including properties for almost all of this.
		[SerializeField]
		private bool foldS;
		public bool FoldS{get{return foldS;} set{foldS = value;}}
		[SerializeField]
		private bool foldG;
		public bool FoldG{get{return foldG;} set{foldG = value;}}

		[SerializeField]
		private List<MOSingle> singleObjects;
		public List<MOSingle> SingleObjects{get{return singleObjects;}}

		[SerializeField]
		private List<MOGroup> groupObjects;
		public List<MOGroup> GroupObjects{get{return groupObjects;}}

		public void init(){
			foldS = false;
			foldG = false;

			singleObjects = new List<MOSingle>();
			groupObjects = new List<MOGroup>();
		}

	}
}

