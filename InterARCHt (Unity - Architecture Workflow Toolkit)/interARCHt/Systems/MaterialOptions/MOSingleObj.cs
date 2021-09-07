using System;
using UnityEngine;

namespace interARCHt.Systems.MaterialOptions
{
	[System.Serializable]
	public class MOSingleObj : MonoBehaviour {

		[SerializeField]
		private MOSingle option;

		public void init(MOSingle option){
			this.option = option;
		}

		public void cycleMaterial(){
			option.cycleMaterial();
		}
	}
}

