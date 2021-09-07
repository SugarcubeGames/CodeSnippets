using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//This script is attached to the material option selector spheres, and handles
//the callback to the option to change the material
public class BSIMaterialOptionSelector : MonoBehaviour {

    private BSIMaterialOption _option;
    private Material _material;

    public void init(BSIMaterialOption option, Material material)
    {
        _option = option;
        _material = material;
    }

    private void OnTriggerEnter(Collider other)
    {
        //Make sure the triggering element isn't meant to be ignored by
        //interactibles
        if(other.tag != BSITagsLayers.ignoreBSIInteractibles)
        {
            _option.updateMaterial(_material);
        }
    }
}
