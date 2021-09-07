using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Which castle segment does this room belong to?  (Defines tileset)
/// </summary>
public enum LEVELTYPES
{
    ENTRANCE = 1,
    MAINHALL = 2,
    CRUMBLINGTOWER = 3,
    UNDERGROUNDCAVERNS = 4,
    DUNGEON = 5,
    CLOCKTOWER = 6 
}

public enum ROOMENTRYEXITLOCATIONS
{
    RIGHT,
    LEFT,
    UPPERRIGHT,
    UPPERLEFT,
    LOWERRIGHT,
    LOWERLEFT
}

[System.Serializable]
public class Room : ScriptableObject {

    /// <summary>
    /// The room's name.
    /// </summary>
    [SerializeField]
    private string _name;
    /// <summary>
    /// The room's name
    /// </summary>
    public string Name
    {
        get { return _name; }
        private set { }
    }

    /// <summary>
    /// Which castle segment does this room belong to?
    /// </summary>
    [SerializeField]
    private LEVELTYPES _roomType;
    /// <summary>
    /// Which castle segment does this room belong to?
    /// </summary>
    public LEVELTYPES RoomType
    {
        get { return _roomType; }
        private set { }
    }

    //Where is this room saved to?
    [SerializeField]
    private string _assetSavePath = "";
    /// <summary>
    /// Where is this asset saved to?
    /// </summary>
    public string AssetSavePath
    {
        get { return _assetSavePath; }
        private set { }
    }

    //Room entrance and exit
    [SerializeField]
    private ROOMENTRYEXITLOCATIONS _roomEntrance;
    /// <summary>
    /// Where does the player enter the room from?
    /// </summary>
    public ROOMENTRYEXITLOCATIONS RoomEntrance
    {
        get { return _roomEntrance; }
        private set { }
    }

    [SerializeField]
    private ROOMENTRYEXITLOCATIONS _roomExit;
    /// <summary>
    /// Where does the player enter the room from?
    /// </summary>
    public ROOMENTRYEXITLOCATIONS RoomExit
    {
        get { return _roomExit; }
        private set { }
    }

    /*********************************************************************
     * Monster Spawn Bools
     * 
     * These bools are checked during saving, and modified automatically.
     * These include zmobies, medusa heads, etc.  If one of their spawn
     * blocks is in the rooom, then they are set to true.
     * *******************************************************************/

    /// <summary>
    /// Is there a bat spawn trigger in this room?
    /// </summary>
    [SerializeField]
    private bool spawnBats;
    /// <summary>
    /// Is there an Eagle spawn trigger in this room?
    /// </summary>
    [SerializeField]
    private bool spawnEagles;
    /// <summary>
    /// Is there a Ghost spawn trigger in this room?
    /// </summary>
    [SerializeField]
    private bool spawnGhosts;
    /// <summary>
    /// Is there a Medusa Head spawn trigger in this room?
    /// </summary>
    [SerializeField]
    private bool spawnMedusaHeads;
    /// <summary>
    /// Is there a Merman spawn trigger in this room? (Also requires a merman spawn point marker)
    /// </summary>
    [SerializeField]
    private bool spawnMermen;
    /// <summary>
    /// Is there a Zombie spawn trigger in this room?
    /// </summary>
    [SerializeField]
    private bool spawnZombies;
    

    /****************************************************
     * Apply changes to room variables
     * **************************************************/
    public void setAssetSavePath(string path)
    {
        _assetSavePath = path;
    }

    public void setName(string name)
    {
        _name = name;
    }

    public void setRoomType(LEVELTYPES type)
    {
        _roomType = type;
    }

    public void setRoomEntrance(ROOMENTRYEXITLOCATIONS entrance)
    {
        _roomEntrance = entrance;
    }

    public void setRoomExit(ROOMENTRYEXITLOCATIONS exit)
    {
        _roomExit = exit;
    }

}
