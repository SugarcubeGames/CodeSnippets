using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/******************************************************************
 * This class handles spawning the types of monsters that
 * aren't placed at specific points in the scene.  These
 * include:
 *      Zombies
 *      Mermen
 *      Medusas
 *      Bats (Fly across scene, not the ones that hang and attack)
 *      Eagles
 *      Eagles with Fleamen
 *      Ghosts
 *      
 * Many of these spawn in different ways.
 *      Zombies: Spawn just off screen and move towards hte player.
 *                  They spawn in groups, until there are three in
 *                  the room.  They will spawn while the user is
 *                  inside of a designated trigger.
 ******************************************************************/

public class EnemySpawnManager : MonoBehaviour {
    /// <summary>
    /// Do Zombies spawn in this room?
    /// </summary>
    private bool spawnZombies;
    /// <summary>
    /// Do Mermen spawn in this room?
    /// </summary>
    private bool spawnMermen;
    /// <summary>
    /// Do Medusa Heads spawn in this room?
    /// </summary>
    private bool spawnMedusaHeads;
    /// <summary>
    /// Do Bats spawn in this room?
    /// </summary>
    private bool spawnBats;
    /// <summary>
    /// Do Eagles spawn in this room?
    /// </summary>
    //private bool spawnEagles; //There are never just eagle spawns, only eagles with fleamen
    /// <summary>
    /// Do Eagles with Fleamen spawn in this room?
    /// </summary>
    private bool spawnEagleFleamen;
    /// <summary>
    /// Do Ghosts spawn in this room?
    /// </summary>
    private bool spawnGhosts;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    /// <summary>
    /// Update the Enemy Spawner settings to correspond to the newly loaded room.
    /// </summary>
    /// <return>Returns true if all settings are updated correctly.</return>
    public bool updateSpawnSettings()
    {
        return true;
    }

    void FixedUpdate()
    {

    }
}
