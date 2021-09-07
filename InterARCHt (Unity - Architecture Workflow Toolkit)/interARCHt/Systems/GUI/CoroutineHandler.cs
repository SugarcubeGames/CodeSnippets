using System;
using System.Collections;
using UnityEngine;

namespace interARCHt.Systems.GUIs
{
	//This class is used to handle those instances when one of the child GUI elements
	//needs to run a coroutine.  Since none of them are monobehaviors, they cannot
	//create coroutines on their own.
	public class CoroutineHandler:MonoBehaviour
	{
		//Interface for when one of the GUI's needs to use a coroutine.
		public void StartChildCoroutine(IEnumerator coroutineMethod){
			StartCoroutine (coroutineMethod);
		}
	}
}

