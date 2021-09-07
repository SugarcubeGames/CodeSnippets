using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace interARCHt.Systems.DesignOptions
{
	/// <summary>
	/// The manager script keeps track of the different Design Options, as well
	/// as the individual choices within each option
	/// </summary>

	[Serializable]
	public class DOManager : ScriptableObject
	{
		[SerializeField]
		public List<DOScr> designOptions;

		public void OnEnable(){
			if(designOptions == null){
				designOptions = new List<DOScr> ();
			}
		}

		public void drawGUI(){

		}

		//Add a new option to the list
		public void addOption(){
			//TODO: Add checks to make sure that enough objects are selected
			designOptions.Add (new DOScr(designOptions.Count+1));
		}

		//Get total number of design options, used by the GUI
		public int getTotDesignOptions(){
			return designOptions.Count;
		}

		//Get the list of design options
		public List<DOScr> getDesignOptions(){
			return designOptions;
		}
	}
}

