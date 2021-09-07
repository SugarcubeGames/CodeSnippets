using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum Alignments
{
    CHAOTIC_EVIL,
    CHAOTIC_GOOD,
    CHAOTIC_NEUTRAL,
    LAWFUL_EVIL,
    LAWFUL_GOOD,
    LAWFUL_NEUTRAL,
    NEUTRAL_EVIL,
    NETURAL_GOOD,
    NEUTRAL_NEUTRAL
}

[System.Serializable]
public class Character
{
    /// <summary>
    /// Character's name
    /// </summary>
    public string _name;

    /// <summary>
    /// Character's Race
    /// </summary>
    public Race _race;

    /// <summary>
    /// Character's guild
    /// </summary>
    public Guild _guild;

    //Attributes
    /// <summary>
    /// Attribute: Measures physical power and carrying capacity.
    /// </summary>
    [SerializeField]
    public Attribute _strength;
    /// <summary>
    /// Attribute: Measures endurance, stamina, and good health.
    /// </summary>
    [SerializeField]
    public Attribute _constitution;
    /// <summary>
    /// Attribute: Measures agility, balance, coordination, and reflexes.
    /// </summary>
    [SerializeField]
    public Attribute _dexterity;
    /// <summary>
    /// Attribute: Measures deductive reasoning, knowledge, memory, logic, and rationality
    /// </summary>
    [SerializeField]
    public Attribute _intelligence;
    /// <summary>
    /// Attribute: Measures self-awareness, common sense, restrain, perception, and insight.
    /// </summary>
    [SerializeField]
    public Attribute _wisdom;
    /// <summary>
    /// Attribute: measuring force of personality, persuasiveness, leadership, and successful planning.
    /// </summary>
    [SerializeField]
    public Attribute _charisma;

    /// <summary>
    /// Character's age.
    /// </summary>
    public int _age;

    /// <summary>
    /// Character's Alignment
    /// </summary>
    public Alignments _alignment;

    public Character(Race r, Guild g)
    {
        _race = r;
        _guild = g;

        //The name will be generated using name banks unique to each race.  For now
        //just set the name to the race for easy testing
        _name = $"({_race._name}) ({_guild._name})";

        //TODO: Select a class

        rollStats();
    }

    private void rollStats()
    {
        /*
        Debug.Log("Rolling stats for " + this._name);
        List<int> rolls = new List<int>();

        List<int> d = new List<int>();

        for(int i = 0; i<6; i++)
        {
            d.Clear(); //Reset list for now rolls
            for(int j = 0; j<4; j++)
            {
                d.Add(Random.Range(1, 7));
                Debug.Log($"{i}:{j} : " + d[d.Count - 1]);
            }
            d.Sort();//Sort the list from smallest to least
            d.RemoveAt(0); //Remove the lowest roll

            rolls.Add(d.Sum());
            Debug.Log($"Total {i}: + {rolls[rolls.Count-1]}");
        }
        */
        
        /****
         * Attribute set methods
         * ****/

    }
}
