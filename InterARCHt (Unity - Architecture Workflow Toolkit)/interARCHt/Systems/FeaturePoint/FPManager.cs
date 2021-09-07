using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace interARCHt.Systems.FeaturePoint
{
	/// <summary>
	/// This script keeps track of all the feature points in a scene.
	/// It also compiles a list of them at runtime to procedurally generate
	/// the selection menu.  This allows feature points to be easily
	/// added to the scene, and requires no additional code for
	/// each one added.
	/// </summary>
	public class FPManager : MonoBehaviour
	{
		/// <summary>
		/// List of feature points in the scene, populated at runtime
		/// </summary>
		[SerializeField]
		private List<FPScr> features;
		public List<FPScr> Features{get{return features;}}
		[SerializeField]
		private List<string> featureNames;

		public void init(){
			features = new List<FPScr> ();
			featureNames = new List<string> ();
		}

		/// <summary>
		/// Get the feature points present in the scene
		/// </summary>
		public void collectFeatures (){
			//Make sure that features exists, if not, reinitialie it
			if(features == null){
				init ();
			} else {
				//if it does exist, we need to clear the list so we're not dealing with duplicates
				features.Clear ();
				featureNames.Clear ();
			}

			//Find all instances of FPScr in the scene and add them to the list.
			foreach(UnityEngine.Object o in UnityEngine.Object.FindObjectsOfType<FPScr>()){
				features.Add ((FPScr)o);
			}

			//If there are no features, we don't need to do anything else
			if(features.Count < 1){
				return;
			}

			foreach(FPScr f in features){
				f.init ();
			}

			//Build the name list
			foreach(FPScr f in features){
				featureNames.Add (f.Name);
			}

			//List<FPScr> temp = features.OrderBy (fp => fp.fpName).ToList ();
		}

		/// <summary>
		/// This function is called immedately when the presentation starts, and gathers a list
		/// of all the feature points in the scene
		/// </summary>
		void Awake(){
			collectFeatures ();
		}

		//Used while initializing the GUI
		public int getTotFeatures(){
			return features.Count;
		}

		public List<FPScr> getFeatures(){
			return features;
		}

		private void buildFeatureNameList(){

		}

		public string[] getFeatureNames(){
			return featureNames.ToArray();
		}
	}

}
