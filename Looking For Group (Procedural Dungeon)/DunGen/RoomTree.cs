using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class HallHolder
{
    public Vector2 pos;
    public bool isVertical;

    public HallHolder(Vector2 p, bool vert)
    {
        pos = p;
        isVertical = vert;
    }
}

public class RoomTree : MonoBehaviour
{
    public static float goldenRatio = 1.618f;
    //Dimenion data
    //Position of top-left corner(-,+)
    private Vector2Int _pos;
    private Vector2Int _size;
    //Rectangle to store actual physical dims/bounds
    //of this room
    public Rect rect;
    public Vector2 pos {
        get { return rect.position; }
        set { rect.position = new Vector2(value.x, value.y); }
    }

    public Vector2 size { get { return rect.size; } private set { } }
    //Track if this room has been placed/is visible for drawing gizmos
    public bool isPlaced = false;

    //The likelihood that any given side will have a door
    private static int doorLikelihood = 100;
    public List<Door> doors;//List of doors in the room

    //How deep into the dungeon is this room (how many rooms deep)
    public int depth;

    //Quick reference tools
    public Vector2 TopCorner
    {
        get { return pos + size; }
        private set { }
    }
    public Vector3Int TopCorner3D
    {
        get { return new Vector3Int(_pos.x + _size.x, _pos.y + _size.y, 0); }
        private set { }
    }

    //Hall element that holds the connector hallway between this room and the room
    //it's placed off of.  Should only ever need one of these per room, as it's based
    //on where this room attaches to the previous room.
    public HallHolder hall;

    public void init()
    {
        _size = new Vector2Int();
        _pos = new Vector2Int(0, 0);
        //Refer to DunGenTest for notations

        _size.x = Random.Range(5, 8);

        //Use golden ratio to calculate other size randomly
        int typ = Random.Range(0, 3);
        switch (typ)
        {
            case 0:
                _size.y = _size.x;
                break;
            case 1:
                _size.y = Mathf.RoundToInt(_size.x * goldenRatio);
                break;
            case 2:
                _size.y = Mathf.RoundToInt(_size.x / goldenRatio);
                break;
        }

        rect = new Rect(_pos, _size);

        //Build doors.  Door pos is relative to the top corner (pos) of the
        //room rectangle
        doors = new List<Door>();

        int rnd;
        int n = 0;
        while (doors.Count < 1 && n < 10)
        {
            for (int i = 0; i < 4; i++)
            {
                rnd = Random.Range(0, 100);
                if (rnd < doorLikelihood)
                {
                    AddDoor(i);
                }
                n++;
            }
        }
    }

    private void AddDoor(int side)
    {
        //Sides: 1 = left, 2 = top, 3 = right, 4 = bottom
        if (side == 1 || side == 3)
        {
            //Select a random point along the vertical edge of the room to
            //place a door.  
            int rndY = (int)Random.Range(1, size.y - 1);
            if (side == 1)
            {
                doors.Add(new Door(new Vector2(0, rndY), DoorSide.LEFT, this));
            }
            else
            {
                doors.Add(new Door(new Vector2((int)size.x, rndY), DoorSide.RIGHT, this));
            }
        }
        else
        {
            int rndX = (int)Random.Range(1, size.x - 1);
            if (side == 2)
            {
                doors.Add(new Door(new Vector2(rndX, (int)size.y), DoorSide.TOP, this));
            }
            else
            {
                doors.Add(new Door(new Vector2(rndX, 0), DoorSide.BOTTOM, this));
            }
        }
    }

    public void SetPlaced(bool b)
    {
        isPlaced = b;
    }

    public void SetDepth(int d)
    {
        depth = d;
    }

    /// <summary>
    /// Move the room so that the passed-in door aligns with
    /// the coordinates of it's linked door.
    /// </summary>
    /// <param name="doorToPlace">The door from the new room</param>
    /// <param name="existingDoor">The placed door we're attempting to link to</param>
    public bool MoveDoorToDoor(Door doorToPlace, Door existingDoor, List<GameObject> placedRooms)
    {
        //We should only ever get in doors that can pair based on their side.
        //Move the new door into position, then check for it overlapping
        //other existing rooms.  If it doesn't overlap, mark it as placed.

        //Get the placed room we're attempting to link to
        RoomTree existingRoom = existingDoor.room;

        //Move this room based on the dimensions of the eixsting room and the side
        //we're placing agains
        float x = 0;
        float y = 0;

        switch (doorToPlace.side)
        { 
            //if this door is on the bottom, move this room to be above the existing room
            case DoorSide.BOTTOM:
                //Debug.Log("Existing: " + existingDoor.pos.x + " | " + doorToPlace.pos.x);
                x = existingRoom.pos.x - (doorToPlace.pos.x - existingDoor.pos.x);
                y = existingRoom.pos.y+existingRoom.size.y + 1;
                hall = new HallHolder(new Vector2(x + doorToPlace.pos.x + .5f, y-.5f), true);
                break;
            //If this door is on the top of this room, move this room to the bottom
            //of the existing room.
            case DoorSide.TOP:
                x = existingRoom.pos.x - (doorToPlace.pos.x - existingDoor.pos.x);
                y = (existingRoom.pos.y - size.y) - 1;
                hall = new HallHolder(new Vector2(x + doorToPlace.pos.x +.5f, y+size.y+.5f), true);
                break;
            //If this door is on the left of this room, move it to the right of
            //the existing room.
            case DoorSide.LEFT:
                x = (existingRoom.pos.x + existingRoom.size.x) + 1;
                y = existingRoom.pos.y - (doorToPlace.pos.y - existingDoor.pos.y);
                hall= new HallHolder(new Vector2(x-.5f, y + doorToPlace.pos.y + .5f),false);
                break;

            case DoorSide.RIGHT:
                x = (existingRoom.pos.x - size.x) - 1;
                y = existingRoom.pos.y - (doorToPlace.pos.y - existingDoor.pos.y);
                hall = new HallHolder(new Vector2(x+size.x+.5f, y+doorToPlace.pos.y+.5f), false);
                break;
        }

        this.transform.position = new Vector2(x,y);

        //Update our rectangle to track the new position
        rect = new Rect(this.transform.position, size);

        //Figure out if this room overlaps with any others
        //Comapre this room to all placed rooms
        foreach(GameObject go in placedRooms)
        {
            if (Overlaps(go.GetComponent<RoomTree>()))
            {
                return false;
            }
        }
        //If this room isn't overlapping anything, place the room,
        //mark both doors as placed, and return true;  Also,
        //update it's depth value.
        this.isPlaced = true;
        doorToPlace.depthValue = existingDoor.depthValue + 1;
        doorToPlace.IsLinked = true;
        doorToPlace.linkedRoom = existingRoom;
        doorToPlace.room.depth = existingDoor.room.depth +1;
        existingDoor.IsLinked = true;
        existingDoor.linkedRoom = this;

        return true;
    }

    public bool Overlaps(RoomTree rm)
    {
        if ((this.rect.xMin < rm.rect.xMax) &&
            (this.rect.xMax > rm.rect.xMin) &&
            (this.rect.yMin < rm.rect.yMax) &&
            (this.rect.yMax > rm.rect.yMin))
        {
            return true;
        }

        return false;
    }
    /***********************************************************
     * Debug / Dev methods
     * *********************************************************/

    //Draw the room using gizmos
    public void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawSphere(Vector3.zero, .25f);
        if (isPlaced)
        {
            Gizmos.color = Color.grey;
            //Draw the grids

            Gizmos.color = Color.white;
            Gizmos.DrawWireCube(new Vector3(pos.x + size.x / 2, pos.y + size.y / 2, 0),
                                new Vector3(size.x, size.y, 0));
            Gizmos.DrawWireCube(new Vector3(pos.x + size.x / 2, pos.y + size.y / 2, 0),
                                new Vector3(size.x - .2f, size.y - .2f, 0));

            if (hall != null)
            {
                Vector3 p1 = new Vector3(hall.pos.x, hall.pos.y, 0);
                Gizmos.DrawWireCube(p1, Vector3.one);
                if (hall.isVertical)
                {
                    Gizmos.DrawWireCube(p1, new Vector3(.8f, 1, 1));
                }
                else
                {
                    Gizmos.DrawWireCube(p1, new Vector3(1, .8f, 1));
                }
            }

            Gizmos.color = Color.blue;

            if (doors == null || doors.Count < 1) return;
            foreach (Door d in doors)
            {
                if (!d.IsLinked) continue;
                if (d.isVertical)
                {
                    Vector3 p1 = new Vector3(pos.x + d.pos.x, pos.y + d.pos.y + .5f, 0);
                    Vector3 p2 = new Vector3(.12f, 1f, 0);
                    if (d.pos.x == 0)
                    {
                        p1 += Vector3.right * .05f;
                    }
                    else
                    {
                        p1 += Vector3.left * .05f;
                    }
                    if (d.IsLinked) Gizmos.color = Color.blue; else Gizmos.color = Color.white;
                    Gizmos.DrawCube(p1, p2);
                }
                else
                {
                    Vector3 p1 = new Vector3(pos.x + d.pos.x + .5f, pos.y + d.pos.y, 0);
                    Vector3 p2 = new Vector3(1f,.12f, 0);
                    Vector3 p3 = new Vector3(-.5f, 0, 0); //Vertical offset for andle text
                    if (d.pos.y == 0)
                    {
                        p1 += Vector3.up * .05f;
                        p3 = new Vector3(-.5f, .75f, 0);
                    }
                    else
                    {
                        p1 += Vector3.down * .05f;
                    }
                    if (d.IsLinked) Gizmos.color = Color.blue; else Gizmos.color = Color.white;
                    Gizmos.DrawCube(p1, p2);
                }
            }

            //Draw floor grids
            Gizmos.color = Color.grey;
            for(int i = 1; i<size.x; i++)
            {
                Gizmos.DrawLine(new Vector3(pos.x + i, pos.y + .1f, 0),
                                new Vector3(pos.x + i, pos.y + size.y -.1f, 0));
            }
            for (int j = 1; j < size.y; j++)
            {
                Gizmos.DrawLine(new Vector3(pos.x +.1f, pos.y + j, 0),
                                new Vector3(pos.x + size.x - .1f, pos.y + j, 0));
            }
        }
    }

    public void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireCube(new Vector3(pos.x + size.x / 2, pos.y + size.y / 2, 0),
                                new Vector3(size.x, size.y, 0));
        Gizmos.DrawWireCube(new Vector3(pos.x + size.x / 2, pos.y + size.y / 2, 0),
                                new Vector3(size.x-.2f, size.y-.2f, 0));

        if (hall != null)
        {
            Vector3 p1 = new Vector3(hall.pos.x, hall.pos.y, 0);
            Gizmos.DrawWireCube(p1, Vector3.one);
            if (hall.isVertical)
            {
                Gizmos.DrawWireCube(p1, new Vector3(.8f, 1, 1));
            }
            else
            {
                Gizmos.DrawWireCube(p1, new Vector3(1, .8f, 1));
            }
        }
    }
}
