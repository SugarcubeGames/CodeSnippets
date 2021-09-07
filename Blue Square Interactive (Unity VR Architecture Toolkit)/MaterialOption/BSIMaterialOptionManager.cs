using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BSIMaterialOptionManager : ScriptableObject {

    private static BSIMaterialOptionManager _instance;
    public static BSIMaterialOptionManager Instance
    {
        get
        {
            if (_instance == null)
            {
                try
                {
                    _instance = Resources.Load(managerPathRelative) as BSIMaterialOptionManager;
                    _instance.init();

                } catch (Exception ex)
                {
                    Debug.LogError(ex.Message);
                }
                
            }
            return _instance;
        }

        private set { }
    }
    /// <summary>
    /// The list of Material Options in the project.
    /// </summary>
    [SerializeField]
    private List<BSIMaterialOption> options;
    public List<BSIMaterialOption> Options
    {
        get { return options; }
        private set { }
    }

    /// <summary>
    /// The path of the manager.  There is only one manager, it should always be stored at this location.
    /// </summary>
    public static string managerPath = "Assets/Resources/Materials/MaterialOptionManager.asset";
    public static string managerPathRelative = "Materials/MaterialOptionManager";
    public static string materialLocationPath = "Assets/Resources/Materials/Material Options/";

    public void init()
    {
        if(options == null)
        {
            options = new List<BSIMaterialOption>();
        }
    }

    /***************************************************************************
     * Add / Remove Methods
     * *************************************************************************/
    public void AddMaterialOption(string name, string matPath, Material mat)
    {
        //Debug.Log(matPath);
        BSIMaterialOption newOption = new BSIMaterialOption();
        newOption.init(name, matPath, mat);

        options.Add(newOption);
    }

    public void RemoveMaterialOption(string name)
    {
        foreach (BSIMaterialOption opt in options)
        {
            if (opt.Name.Equals(name))
            {
                options.Remove(opt);

                break;
            }
        }
    }
	
}
