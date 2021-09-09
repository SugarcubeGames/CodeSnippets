from random import *
from Room import *
from RoomType import RoomType
import Vec as vec
import numpy as np
import math as math

class Dungeon:
    numrooms = 12
    totrooms = 12
    size=10
    linearcutoff = 60
    dungeonarray = [[size,size]]
    rooms = [] #roomlist
    dirvals = [vec.Vector2(0,1), vec.Vector2(1,0),
               vec.Vector2(0,-1), vec.Vector2(-1,0)]
    
    def __init__(self, size, rooms, linear):
        #print("init")
        self.size = size
        self.numrooms = rooms
        self.totrooms = rooms
        self.linearcutoff = linear
    
    def build(self, size):
        #print("Build")
        da = self.dungeonarray = np.ones((size,size))
        #Pick a random starting point
        curx = randint(1,size-2)
        cury = randint(1,size-2)
        #print(f"Start Point: {curx}, {cury}")
        #Set that start point to 0 (a room)
        da[curx][cury] = 0
        #Create a room at the starting point
        self.rooms.append(Room(curx, cury, 0))
        #self.rooms[len(self.rooms)-1].settype(RoomType.ENTRY)
        
        while self.numrooms > 1:
            #Pick a random room, bias towards the last-added room
            r = len(self.rooms)-1 #assume attach to latest room
            p = randint(1,99)
            if p<self.linearcutoff: #60% probabilitiy of Adding to latest room
                r = randint(0, len(self.rooms)-1)
                #print(r)
            if self.tryaddroom(self.rooms[r]):
                self.numrooms -= 1
        self.setroomtype()
        self.printdungeon()
    
    def getroomfrompos(self, pos):
        #print(f"attempting to recover room from position: {pos.x},{pos.y}")
        for r in self.rooms:
            if r.posx == pos.x and r.posy == pos.y:
                #print(f"Retrieving Room: {r.posx},{r.posy} | {r.depth}")
                return r;
            
        #print("Did not find a matching room")
               
    def tryaddroom(self, rm):
        #rm is the room we're trying to build from
        spos = vec.Vector2(rm.posx, rm.posy)
        #pick a random direction to build in
        #0: up, 1: right, 2: down, 3: left
        d = randint(0,3)
        dval = self.dirvals[d]
        rpos = vec.Vector2(spos.x, spos.y)
        rpos += dval
        
        #Make sure the position is within bounds
        if rpos.x < 0 or rpos.x > self.size-1 or rpos.y < 0 or rpos.y > self.size -1:
            return False
        #Make sure that position isn't already ocuppied
        if self.dungeonarray[rpos.x][rpos.y] == 0:
            return False
        
        #If we're good, build a new room
        self.dungeonarray[rpos.x][rpos.y] = 0
        nr = Room(rpos.x, rpos.y, rm.depth+1)
        self.rooms.append(nr)
        return True
    
    def setroomtype(self):
        #See RoomType file for type selection
        maxdepth = 0
        startroom = Room
        bossroom = Room
        #Define the entry and boss rooms
        #Work through the room list, and the room
        #with the highest depth value is the boss
        #room.  In the event of ties, the first room
        #to have this value is marked as the boss.
        for r in self.rooms:
            if(r.depth == 0):
                r.settype(RoomType.ENTRY)
                startroom = r
            else:
                if r.depth > maxdepth:
                    maxdepth = r.depth
                    bossroom = r
        bossroom.settype(RoomType.BOSS)
        
        #Work through and figure out the rest of the rooms
        curtreasures = 0
        maxtreasures = math.floor(self.totrooms/10)
        curemptyrooms = 0
        maxemptyrooms = math.floor(self.totrooms/5)
        hassethealroom = False
        halfdepth = math.floor(maxdepth/2)
        
        #print(f"Total Room:{self.totrooms}\nMax Depth:{maxdepth}\nMax Treasure Rooms:{maxtreasures}\nMax Empty Rooms:{maxemptyrooms}\nHalf Max Depth:{halfdepth}\n")
        for r in self.rooms:
            if(r == startroom or r == bossroom):
                continue
            #The higher the depth the more likely it
            #will be an enemy room.  Empty rooms should
            #rarely occur past half depth.
            
            #Determine if this is going to be an empty room
            prob = randint(1,99)
            if (r.depth <= halfdepth and prob < 5+5*(maxemptyrooms - curemptyrooms) and curemptyrooms < maxemptyrooms):
                r.settype(RoomType.EMPTY)
                curemptyrooms += 1
                continue
            #Determine if this is health room.  Necessary in every dungeon
            if(r.depth > maxdepth-2 and hassethealroom == False):
                r.settype(RoomType.HEALTH)
                hassethealroom = True
                continue
            #Determine if this is treasure room
            if(r.depth > halfdepth and prob > 60 - 20*(maxtreasures-curtreasures) and curtreasures < maxtreasures):
                r.settype(RoomType.TREASURE)
                curtreasures += 1
                continue
            #If it's none of the other room types, it's an enemy
            r.settype(RoomType.ENEMY)
    
    
    def printdungeon(self):
        for y in range(0, self.size):
            for x in range(0, self.size):
                if self.dungeonarray[x][y]==0:
                    print("0", end = '')
                else:
                    print("1", end = '')
            #Now draw the Depth Map
            print(" |  ", end = '') #Spacer between the Dungeon Array and Depth Maps
            for x in range(0, self.size):
                if self.dungeonarray[x][y] == 1:
                    print("█", end = '')
                else:
                    pos = vec.Vector2(x, y)
                    #print(pos)
                    rm = self.getroomfrompos(pos)
                    if(rm.depth<10):
                        print(f"{rm.depth}", end = '')
                    else:
                        print(f"{rm.depth-10}", end = '')
            print(" | ", end = '')
            #Draw Dungeon room types
            for x in range(0, self.size):
                #placehold: Print room types
                if self.dungeonarray[x][y] == 1:
                    print(" ", end = '')
                else:
                    pos = vec.Vector2(x, y)
                    rm = self.getroomfrompos(pos)
                    #print(f"{rm.rt.value}", end = '')
                    if rm.rt == RoomType.ENTRY:
                        print("S", end='')
                    elif rm.rt == RoomType.EMPTY:
                        print("□", end = '')
                    elif rm.rt == RoomType.ENEMY:
                        print("E", end = '')
                    elif rm.rt == RoomType.TREASURE:
                        print("T", end = '')
                    elif rm.rt == RoomType.HEALTH:
                        print("H", end = '')
                    elif rm.rt == RoomType.BOSS:
                        print("B", end='')
                    elif rm.rt == RoomType.UNDEFINED:
                        print("-", end = '')
                    else:
                        print("$", end = '')
            print("")
        print("")             