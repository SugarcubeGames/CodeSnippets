using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    /// <summary>
    /// How many characters to generate?
    /// </summary>
    public int _numToGenerate = 4;

    /// <summary>
    /// List of all races in the game
    /// </summary>
    [SerializeField]
    public List<Race> _races;

    /// <summary>
    /// Luist of all generated characters
    /// </summary>
    [SerializeField]
    public List<Character> _characters;
    //Use this method to genrate all the class and race structs
    private void Awake()
    {
        genRaces();
    }

    // Start is called before the first frame update
    void Start()
    {
        _characters = new List<Character>();

        for(int i = 0; i<_numToGenerate; i++)
        {
            //select a random race;
            Race r = Races.races[Random.Range(0, Races.Count)];
            Guild g = Guilds.guilds[Random.Range(0, Guilds.Count)];
            _characters.Add(new Character(r, g));
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /************************************************************
     * initial generation classses
     * **********************************************************/
     private void genRaces()
    {
        Debug.Log("Generating racial structs");

        _races = new List<Race>();

        _races.Add(new Race("Human", 1, 1, 1, 1, 1, 1));
        _races.Add(new Race("Wood Elf", 0, 0, 2, 0, 0, 0));
        _races.Add(new Race("Sea Elf", 0, 1, 2, 0, 0, 0));
        _races.Add(new Race("Mountain Dwarf", 2, 2, 0, 0, 0, 0));
        _races.Add(new Race("Hill Dwarf", 0, 2, 0, 0, 1, 0));
    }
}
