using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BSIMaterialOption {

    /// <summary>
    /// This Maerial Option's name
    /// </summary>
    [SerializeField]
    private string _name;
    public string Name
    {
        get { return _name; }
        private set { }
    }

    /// <summary>
    /// List of all materials in the option
    /// </summary>
    [SerializeField]
    private List<Material> _materials;
    public List<Material> Materials
    {
        get { return _materials; }
        private set { }
    }

    /// <summary>
    /// The name of the material associated with this option
    /// </summary>
    [SerializeField]
    private string _materialName;
    public string MaterialName
    {
        get { return _materialName; }
        private set { }
    }

    //Path to the material object in the project browser
    [SerializeField]
    private string _materialPath;
    public string MaterialPath
    {
        get { return _materialPath; }
        private set { }
    }

    /// <summary>
    /// The material associated with this option
    /// </summary>
    [SerializeField]
    private Material _optionMaterial;
    public Material Material
    {
        get{ return _optionMaterial; }
        private set { }
    }

    [SerializeField]
    private int activeIndex;


	public void init(string optionName, string matName, Material mat)
    {
        _materials = new List<Material>();
        _name = optionName;
        _materialName = matName;
        //_materialPath = BSIMaterialOptionManager.materialLocationPath + "mat_BSI_MO_" + matName.Replace(" ", "_");
        _materialPath = BSIMaterialOptionManager.materialLocationPath + matName.Replace(" ", "_");
        _optionMaterial = mat;

        activeIndex = 0;
    }

    public void setName(string name)
    {
        _name = name;
    }

    /**********************************************
     * Add and remove materials
     * ********************************************/
    
    public void AddMaterial(Material m)
    {
        if (!_materials.Contains(m))
        {
            _materials.Add(m);
        }
    }

    public void RemoveMaterial(Material m)
    {
        if (_materials.Contains(m))
        {
            _materials.Remove(m);
        }
    }

    /*******************************************
     * Cycle through material options in runtime
     * *****************************************/
    public void showNextMaterial()
    {
        if (activeIndex == _materials.Count - 1)
        {
            activeIndex = 0;
        }
        else
        {
            activeIndex++;
        }

        updateMaterial(_materials[activeIndex]);
    }

    /***********
     * 
     * *********/
     public int GetMaterialIndex(Material mat)
    {
        return _materials.IndexOf(mat);
    }

    /**********
     * Reorder
     * ********/

    public void reorderMaterial(int oldIndex, int newindex)
    {
        Material m = _materials[oldIndex];

        _materials.RemoveAt(oldIndex);
        _materials.Insert(newindex, m);
    }

    /*****************
     * Update Material
     * ***************/
    public void updateMaterial(Material newMat)
    {
        _optionMaterial.CopyPropertiesFromMaterial(newMat);
    

        activeIndex = _materials.IndexOf(newMat);
    }
}
