using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Race
{
    /// <summary>
    /// This race's name
    /// </summary>
    public string _name;

    /// <summary>
    /// Race's bonus to Strength Attribute
    /// </summary>
    public int _strBonus;
    /// <summary>
    /// Race's bonus to Constitution Attribute
    /// </summary>
    public int _conBonus;
    /// <summary>
    /// Race's bonus to Dexterity Attribute
    /// </summary>
    public int _dexBonus;
    /// <summary>
    /// Race's bonus to Intelligence Attribute
    /// </summary>
    public int _intBonus;
    /// <summary>
    /// Race's bonus to Wisdom Attribute
    /// </summary>
    public int _wisBonus;
    /// <summary>
    /// Race's bonus to Bonus Attribute
    /// </summary>
    public int _chaBonus;

    /*
     * TODO:
     * 
     * Impliment languages and racial languages
     * Impliment Skills.  Character race and class both influence default skills
     * Impliment Items.  Characters will start with gear appropriate to their class,
     *                      and to the prosperity level of the city.
     * */

    public Race(string name, int str, int con, int dex, int intel, int wis, int cha)
    {
        _name = name;
        _strBonus = str;
        _conBonus = con;
        _dexBonus = dex;
        _intBonus = intel;
        _wisBonus = wis;
        _chaBonus = cha;
    }
}

public class Races
{
    public static List<Race> races = new List<Race>
    {
        new Race("Human", 1, 1, 1, 1, 1, 1),
        new Race("Wood Elf", 0, 0, 2, 0, 0, 0),
        new Race("Sea Elf", 0, 1, 2, 0, 0, 0),
        new Race("Mountain Dwarf", 2, 2, 0, 0, 0, 0),
        new Race("Hill Dwarf", 0, 2, 0, 0, 1, 0)
    };

    public static int Count = races.Count;
}
