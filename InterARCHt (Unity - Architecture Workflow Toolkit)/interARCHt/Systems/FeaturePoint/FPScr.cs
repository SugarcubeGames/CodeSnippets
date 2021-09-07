using System;
using UnityEngine;

namespace interARCHt.Systems.FeaturePoint
{
	/// <summary>
	/// This script is attached to the Feature Point prefab,
	/// and stores the name of the feature point, as well as any
	/// information to be displayed about the feature point when
	/// the player teleports to it.
	/// </summary>

	[Serializable]
	public class FPScr : MonoBehaviour
	{
		//A reference to the game object this script is attached to, 
		//Used to gather position informations, and to make sure that the
		//Arrow object doesn't show in runtime.
		[SerializeField]
		private GameObject obj;
		public GameObject Obj{get{return obj;}}

		// Display name at Runtime.  Public for linq sorting
		[SerializeField]
		private string fpName;
		//public accessor property for the name
		public string Name{get{return fpName;} set{fpName = value;}}

		//Does this point have a narrative attached to it?
		[SerializeField]
		private bool hasNarrative;
		public bool HasNarrative{get{return hasNarrative;} set{hasNarrative = value;}}

		//The Narrative
		[Multiline][SerializeField]
		private string narrative;
		public string Narrative{get{return narrative;} set{narrative = value;}}

		//Track the feature point's fold status
		[SerializeField]
		private bool fold;
		public bool Fold{get{return fold;} set{fold = value;}}

		//Initialize the Feature Point system when the presentation starts

		void Start(){
			obj = this.gameObject;

			//We don't want the object this is attached to showing during
			//the presentation.  To achieve this easily, we just disable
			//the attached game object.
			obj.gameObject.SetActive (false);

		}

		//initialize the script
		public void init(){
			obj = this.gameObject;

			//Get the object the script is attached to.  This will be called
			//whenever the player loads the feature point menu so that the
			//list of feature points will update.  We only need to get the
			//reference when obj is null, so if it's not we can skip this.
			obj = this.gameObject;
		}

		//We need to be able to modify the information in the
		//manager, so each instance will draw it's own gui and
		//reference its variables directly.
		public void drawGUI(){

		}

	}
}

