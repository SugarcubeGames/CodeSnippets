using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewRace", menuName = "lfp/Race", order = 1)]
public class RaceSO : ScriptableObject
{
    public string raceName;

    /// <summary>
    /// Race's bonus to Strength Attribute
    /// </summary>
    public int strBonus;
    /// <summary>
    /// Race's bonus to Constitution Attribute
    /// </summary>
    public int conBonus;
    /// <summary>
    /// Race's bonus to Dexterity Attribute
    /// </summary>
    public int dexBonus;
    /// <summary>
    /// Race's bonus to Intelligence Attribute
    /// </summary>
    public int intBonus;
    /// <summary>
    /// Race's bonus to Wisdom Attribute
    /// </summary>
    public int wisBonus;
    /// <summary>
    /// Race's bonus to Bonus Attribute
    /// </summary>
    public int chaBonus;

}
