using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BSIMaterialOptionInteraction : MonoBehaviour {

    /// <summary>
    /// Reference to the Material Option this interaction works with
    /// </summary>
    [SerializeField]
    private BSIMaterialOption _option;
    public BSIMaterialOption Option
    {
        get { return _option; }
        private set { }
    }

    public List<Material> Materials
    {
        get { return _option.Materials; }
        private set { }
    }

	public void setMaterialOption(BSIMaterialOption option)
    {
        _option = option;
    }

    public void showNextMaterial()
    {
        if(_option != null)
        {
            _option.showNextMaterial();
        }
    }
}
