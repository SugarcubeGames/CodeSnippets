using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BSIMaterialOptionCloseSelector : MonoBehaviour {

    BSIMaterialOptionSelectorGenerator _generator;

    public void init(BSIMaterialOptionSelectorGenerator gen)
    {
        _generator = gen;
    }

    private void OnTriggerEnter(Collider other)
    {
        /*
        if(other.tag != BSITagsLayers.ignoreCloseMenuTrigger && other.tag != BSITagsLayers.ignoreBSIInteractibles)
        {
            _generator.clearMaterialDisplay();
        }
        */

        if(other.name.Equals(BSITagsLayers.OculusRightIndexName) || other.name.Equals(BSITagsLayers.OculusLeftIndexName))
        {
            _generator.clearMaterialDisplay();
        }
    }


}
