using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace interARCHt.Systems.MaterialOptions
{
	[System.Serializable]
	public class MOSingle {

		[SerializeField]
		private string name;
		public string Name{get{return name;} set{name = value;}}

		[SerializeField]
		private GameObject obj;
		public GameObject Obj {get{return obj;}}

		[SerializeField]
		private bool fold; //Is the object folded out? 
		public bool Fold{get{return fold;} set{fold = value;}}
		[SerializeField]
		private bool matFold; //Are the materials folded out?
		public bool MatFold{get{return matFold;} set{matFold = value;}}

		[SerializeField]
		private List<Material> materials;
		public List<Material> Materials{get{return materials;}}
		[SerializeField]
		private int activeMat;
		public int ActiveMat{get{return activeMat;} set{activeMat = value;}}

		private bool removeFlag; //is this option being removed from the list?
		public bool RemoveFlag{get{return removeFlag;} set{removeFlag = value;}}

		public MOSingle(GameObject go){
			name = "New Material Option";
			obj = go;
			removeFlag = false;

			fold = false;
			matFold = false;

			materials = new List<Material>();
			activeMat = 0;

			//Grab the current material(s) on the object
			materials.Add(obj.GetComponent<MeshRenderer>().renderer.sharedMaterial);

			//Add the pointer script to the object
			obj.AddComponent<MOSingleObj>();
			MOSingleObj scr = (MOSingleObj)obj.GetComponent(typeof(MOSingleObj));
			scr.init (this);
		}

		//No longer needed, will be removed
		public void DrawGUI(){

		}

		public void setActive(int index){
			if(index < materials.Count && index > -1){
				activeMat = index;
				obj.GetComponent<MeshRenderer>().renderer.sharedMaterial = materials[activeMat];
			}
		}

		public void cycleMaterial(){
			activeMat++;
			if(activeMat >= materials.Count){
				activeMat = 0;
			}

			obj.GetComponent<MeshRenderer>().renderer.sharedMaterial = materials[activeMat];
		}

		public bool checkDelete(){
			return removeFlag;
		}

		public void removeOptionIdentifier(){
			UnityEngine.Object.DestroyImmediate(obj.GetComponent<MOSingleObj>());
		}

	}
}

