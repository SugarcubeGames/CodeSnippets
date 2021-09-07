using System;
using System.Collections.Generic;
using UnityEngine;

namespace interARCHt.Systems.MaterialOptions
{
	[System.Serializable]
	public class MOGroup {

		[SerializeField]
		private string name; //The option's name
		public string Name{get{return name;} set{name = value;}}

		[SerializeField]
		private List<GameObject> objects; //The objects in the option
		public List<GameObject> Objects{get{return objects;}}

		public int objectRemoval; //Track any objects that are going to be removed

		[SerializeField]
		private List<Material> materials; //Material choices
		public List<Material> Materials{get{return materials;}}

		[SerializeField]
		private bool fold; //option fold tracker
		public bool Fold{get{return fold;} set{fold = value;}}
		[SerializeField]
		private bool foldO; //Objects fold tracker
		public bool FoldO{get{return foldO;} set{foldO = value;}}
		[SerializeField]
		private bool foldM; //Materials fold tracker
		public bool FoldM{get{return foldM;} set{foldM = value;}}

		[SerializeField]
		private int activeMat;
		public int ActiveMat{get{return activeMat;} set{activeMat = value;}}

		public bool removeFlag; //Is this option being removed?


		public MOGroup(GameObject obj){
			name = "New Option";

			objects = new List<GameObject>();
			objects.Add(obj);

			materials = new List<Material>();
			materials.Add(obj.GetComponent<MeshRenderer>().renderer.sharedMaterial);

			//Add the pointer script to the object
			obj.AddComponent<MOGroupObj>();
			MOGroupObj scr = (MOGroupObj)obj.GetComponent(typeof(MOGroupObj));
			scr.init(this);

			fold = false;
			foldO = false;
			foldM = false;

			activeMat = 0;

			objectRemoval = -1;

			removeFlag = false;
		}

		public MOGroup(List<GameObject> objs){
			name = "New Option";

			objects = new List<GameObject>();
			foreach(GameObject obj in objs){
				objects.Add(obj);

				obj.AddComponent<MOGroupObj>();
				MOGroupObj scr = (MOGroupObj)obj.GetComponent(typeof(MOGroupObj));
				scr.init (this);
			}

			materials = new List<Material>();
			materials.Add(objs[0].GetComponent<MeshRenderer>().renderer.sharedMaterial);

			foldO = false;
			foldM = false;

			activeMat = 0;

			objectRemoval = -1;

			removeFlag = false;
		}

		public void setMaterial(int index){
			if(index < materials.Count && index > -1){
				activeMat = index;
				foreach(GameObject obj in objects){
					obj.GetComponent<MeshRenderer>().renderer.sharedMaterial = materials[activeMat];
				}
			}
		}

		public void cycleMaterial(){
			activeMat++;
			if(activeMat >= materials.Count){
				activeMat = 0;
			}

			foreach(GameObject obj in objects){
				obj.GetComponent<MeshRenderer>().renderer.sharedMaterial = materials[activeMat];
			}
		}

		public bool checkDelete(){
			return removeFlag;
		}

		public void removeOptionIdentifier(){
			foreach(GameObject obj in objects){
				UnityEngine.Object.DestroyImmediate(obj.GetComponent<MOGroupObj>());
			}
		}
	}
}

