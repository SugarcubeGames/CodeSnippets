using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DoorSide
{
    TOP,
    BOTTOM,
    LEFT,
    RIGHT
}

public class Door
{
    /// <summary>
    /// Position of the door in the room. (From the top left corner (0,0)
    /// </summary>
    public Vector2 pos;

    /// <summary>
    /// Which side of the room is the door on?
    /// </summary>
    public DoorSide side;

    /// <summary>
    /// Which room is this door's room placed off of?
    /// </summary>
    public RoomTree linkedRoom;

    /// <summary>
    /// Which room does this door belong to?
    /// </summary>
    public RoomTree room;

    /// <summary>
    /// How deep into the dungeon is this door?  (used for special room placement and goal room placement)
    /// </summary>
    public int depthValue;

    /// <summary>
    /// Is this door linked to another room?
    /// </summary>
    public bool IsLinked;

    /// <summary>
    /// Is this vertical?
    /// </summary>
    public bool isVertical { get { return (side == DoorSide.LEFT || side == DoorSide.RIGHT); }
                            private set { } }

    public Door(Vector2 p, DoorSide s, RoomTree r1)
    {
        this.pos = p;
        this.side = s;
        this.room = r1;

    }
}
