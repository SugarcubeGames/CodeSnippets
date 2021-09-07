using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Holder class for attributes
[System.Serializable]
public class Attribute
{
    /// <summary>
    /// The base value of this stat.
    /// </summary>
    private int _base;
    /// <summary>
    /// Racial modifier for this attribute
    /// </summary>
    private int _racialModifier;

    /// <summary>
    /// Combines base and modifier value for the attribute
    /// </summary>
    public int val
    {
        get
        {
            return _base + _racialModifier;
        }
    }
    /// <summary>
    /// Modifier value for this stat
    /// </summary>
    public int modifier
    {
        get
        {
            return Mathf.FloorToInt((val-10)/2);
        }
    }

    public Attribute(int b, int r)
    {
        _base = b;
        _racialModifier = r;
    }

    /// <summary>
    /// Set the base attribute value
    /// </summary>
    /// <param name="val">The new base value</param>
    private void setBase(int val)
    {
        _base = val;
    }

}
