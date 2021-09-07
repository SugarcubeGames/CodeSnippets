using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Add this class to any object which needs to survive
/// between scenes.  All objects in the Player scene
/// should have this script on them.
/// </summary>
public class BSIDontDestroyOnLoad : MonoBehaviour {

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }
}
