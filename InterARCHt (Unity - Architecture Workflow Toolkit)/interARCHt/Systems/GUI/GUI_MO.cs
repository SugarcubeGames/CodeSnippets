using interARCHt.Systems.MaterialOptions;
using System;
using UnityEngine;

namespace interARCHt.Systems.GUIs
{
	/// <summary>
	/// Gui element for the Material Options System
	/// </summary>
	public class GUI_MO
	{
		Transform camera;

		public GUI_MO(Transform cam){
			this.camera = cam;
		}

		//Called from GUI_interARCHt
		public void Update(){
			// Material Options Raycaster, checks for material options
			// on clicked-on objects

			if(Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.E)){
				//Vector2 fwd = this.gameObject.transform.TransformDirection(Vector3.forward);

				RaycastHit target = new RaycastHit();

				//See if our ray hits an object with a material option
				if(Physics.Raycast(camera.position, camera.forward, out target, 10.0f)){
					if(target.transform.gameObject.GetComponent(typeof(MOSingleObj)) != null){
						MOSingleObj scr = (MOSingleObj) target.transform.gameObject.
							GetComponent(typeof(MOSingleObj));

						scr.cycleMaterial();
					}
				}
				if(Physics.Raycast(camera.position, camera.forward, out target, 10.0f)){
					if(target.transform.gameObject.GetComponent(typeof(MOGroupObj)) != null){
						MOGroupObj scr = (MOGroupObj) target.transform.gameObject.
							GetComponent(typeof(MOGroupObj));

						scr.cycleMaterial();
					}
				}
			}
		}
	}
}

