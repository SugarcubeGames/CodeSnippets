using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

/**
 * The tree system works by receiving a collection of rooms
 * to distribute, and then aligning them from door to door.
 * 
 * To achieve this, when a room is placed, all its doors
 * will be added to a list to track along with its depth into
 * the dungeon.  When it's time to place a new room, a door will
 * be selected from the list and we'll attempt to place the room.
 * If it overlaps, pick a different door and try again.  Continue
 * until all the rooms have been placed.
 * 
 * Rooms will track their depth (distance from Room 0), and 
 * certain rooms require a certain depth.  The Goal room will
 * always be placed as far from Room 0 as possible.
 * */

#region DunTree
public class DunTree : MonoBehaviour
{
    //The number of rooms this dungeon will contain
    public int numRooms = 1;

    //the lists of placed and unplaced rooms
    private List<GameObject> r_unplaced;
    private List<GameObject> r_placed;

    //Holder for the rooms
    private GameObject r_holder;

    //All Doors that can be connected
    private List<Door> doors;
    
    // Start is called before the first frame update
    void Start()
    {
        r_unplaced = new List<GameObject>();
        r_placed = new List<GameObject>();
        doors = new List<Door>();

        //Create an empty holder for the rooms
        r_holder = new GameObject("Room Holder");

        //Create room 0, which will always start at 0,0
        GameObject r_new = new GameObject("Room 0");
        RoomTree r_tree = r_new.AddComponent(typeof(RoomTree)) as RoomTree;
        r_tree.init();
        r_placed.Add(r_new);
        r_new.transform.parent = r_holder.transform;
        r_tree.SetPlaced(true);  //Room 0 is placed by default
        r_tree.SetDepth(0);
        r_tree.depth = 0; //Mark it as 0 depth, since it's the first room

        //Add all doors from Room0 to the open door list
        foreach(Door d in r_tree.doors)
        {
            doors.Add(d);
        }

        //Build rooms
        for(int i = 1; i<numRooms; i++)
        {
            r_new = new GameObject("Room " + i);
            r_tree = r_new.AddComponent(typeof(RoomTree)) as RoomTree;
            r_tree.init();
            r_unplaced.Add(r_new);
            r_new.transform.parent = r_holder.transform;
        }

        int n = 0;
        int t = 0;
        //Place each unplaced room
        //foreach(GameObject r in r_unplaced)
        while(r_unplaced.Count > 0 && t<100)
        {
            r_tree = r_unplaced[0].GetComponent<RoomTree>() as RoomTree;

            //Pick a door from the room.
            int doorNum = Random.Range(0, r_tree.doors.Count);
            Door door = r_tree.doors[doorNum];

            //List of doors this door can be linked to
            List<Door> linkableDoors = new List<Door>();
            //Determine which side of the room that door is on and
            //build a list of attachable doors from that

            foreach (Door d in doors)
            {
                switch (door.side)
                {
                    case DoorSide.TOP:
                        if (d.side == DoorSide.BOTTOM) linkableDoors.Add(d);
                        break;
                    case DoorSide.BOTTOM:
                        if (d.side == DoorSide.TOP) linkableDoors.Add(d);
                        break;
                    case DoorSide.RIGHT:
                        if (d.side == DoorSide.LEFT) linkableDoors.Add(d);
                        break;
                    case DoorSide.LEFT:
                        if (d.side == DoorSide.RIGHT) linkableDoors.Add(d);
                        break;
                }
            }

            //If there are no linkable doors at this time, move to the next
            //room and try again later
            
            if(linkableDoors.Count == 0)
            {
                t++;
                break;
            }

            //Select one door at random from that list
            Door attachDoor = linkableDoors[Random.Range(0, linkableDoors.Count)];

            /*
            Debug.Log("Attempting to attach new door on " + door.side.ToString() +
                " side of " + door.room.gameObject.name + "(" + door.room.depth + ") to " 
                + attachDoor.side.ToString() + " side door of " 
                + attachDoor.room.gameObject.name +  "(" + attachDoor.room.depth +").") ;
            */

            //Move the room so the doors align, then check for valid placement
            //If it returns true, then the room placmeent worked.  Move the room
            //to the placed list and update the doors list. 
            if (door.room.MoveDoorToDoor(door, attachDoor, r_placed))
            {
                //Switch lists
                r_placed.Add(r_tree.gameObject);
                r_unplaced.Remove(r_tree.gameObject);

                //Add the doors of this new room to the doors list
                foreach (Door d in r_tree.doors)
                {
                    doors.Add(d);
                }

                //Remove the two linked doors from the list
                doors.Remove(door);
                doors.Remove(attachDoor);
            } else
            {
                //If the room is overlapping, move it elsewhere in the list
            }

            n++;
            if (n > 100)
            {
                Debug.Log("100 tries wasn't enough....");
                //break;
            }
        }

        if(t >= 100)
        {
            Debug.Log("Failed to find linkable doors 100 times.");
        }

    }

    private Door TryGetDoor(bool vert)
    {
        return doors[Random.Range(0, doors.Count)];
    }



    // Update is called once per frame
    void Update()
    {
        
    }
}
#endregion