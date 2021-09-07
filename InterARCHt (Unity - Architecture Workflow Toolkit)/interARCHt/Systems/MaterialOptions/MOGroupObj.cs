using System;
using UnityEngine;

namespace interARCHt.Systems.MaterialOptions
{
	[System.Serializable]
	public class MOGroupObj : MonoBehaviour {

		[SerializeField]
		private MOGroup option;

		public void init(MOGroup option){
			this.option = option;
		}

		public void cycleMaterial(){
			option.cycleMaterial();
		}
	}
}

