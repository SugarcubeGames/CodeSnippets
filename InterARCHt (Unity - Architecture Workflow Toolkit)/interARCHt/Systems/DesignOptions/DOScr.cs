#pragma warning disable 0169
#pragma warning disable 0414

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace interARCHt.Systems.DesignOptions
{
	//Design Options
	/*
	 * The design options are broken down as follows:
	 * 		Design Options //This level is handled by DOManager
	 * 			Option 1   //This level is handled by DOScr
	 * 			Option 2
	 * 			...
	 * 			Option n
	 * 				Option n - object 1		//This level is handled by DOSubOption
	 * 				Option n - object 2
	 * 				...
	 * 				option n - object n
	 * 
	 * Normally, and option will not have sub-objects (as displayed in Option n),
	 * however, there's a good chance than an option won't be able to be imported
	 * as a single file, so it's important to make sure there's a way to adjust for
	 * thos instances
	 * */

	//TODO: Make sure that whenever an option is removed, or an object is removed from
	//an option, that they/it are turned back on.

	//This script is not a monobehavior, and is not attached to any object.  A list of these
	//is stored in DOManager, which is stored on the _interARCHt object.
	[Serializable]
	public class DOScr
	{

		[SerializeField]
		private string doName; //The option's name
		//Public accessor for the name (so the manager can see it)
		public string Name {get{return doName;} set{doName = value;}}

		//Track the options within the current Design Option
		[SerializeField]
		private List<DOSubOption> options; //This correlates to Option 1, Option 2, ..., Option n
		public List<DOSubOption> Options{get{return options;}}

		[SerializeField]
		private bool fold; //Is this deisgn option folded out?
		public bool Fold{get{return fold;} set{fold = value;}}

		public bool beingRemoved; //Is this option flagged to be removed?

		public DOScr(int seqNum){
			options = new List<DOSubOption> ();

			doName = "Design Option " + seqNum;
			fold = true;
			beingRemoved = false;
		}

		//Get the total number of options in this design options.  Used by the GUI.
		public int getTotOptions(){
			return options.Count;
		}

		public List<DOSubOption> getOptions(){
			return options;
		}
	}
}

