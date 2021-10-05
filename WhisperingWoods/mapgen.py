import math
import pygame
import random
from mapnode import mapNode
from mapnode import NodeTypes

class mapGenerator:
    seed = 0
    #Counters that track how close the player is getting to
    #the unique areas.
    counterRoseGarden = 0
    counterGraveyard = 0
    counterChurchyard = 0
    #Track how long it's been since the last fire pit
    counterLastFirePit = 0
    #At the start of the game there will be fewer possible
    #directions for the player to move in.  As you get
    #later into the day more paths will open up.  % likelihood
    nodeProbability = 20

    def __init__(self, seed):
        self.seed = seed
        random.seed(self.seed)
        
    def genMap(self, time, previousDir):
        mapNodes = []
        
        for i in range(0,8):
            mapNodes.append(mapNode(i))
        
        for i in range (0,8):
            rand = random.randrange(0,100)
            #print(f'{i}: {rand}/{self.nodeProbability}  |  {rand<self.nodeProbability}')
            if rand>self.nodeProbability and i != previousDir:
                mapNodes[i].setType(NodeTypes.EMPTY)
        #Determine what sort of tile the the map is.
        for i in range(0,8):
            if(mapNodes[i].nType != NodeTypes.EMPTY):
                if i==previousDir:
                    mapNodes[i].setType(NodeTypes.PREV)
                else:
                    #For testing, determine if this is forest or path
                    rand = random.randrange(0,100)
                    if(rand<self.nodeProbability*2):
                        mapNodes[i].setType(NodeTypes.PATH)
                    else:
                        mapNodes[i].setType(NodeTypes.FOREST)
                    
        return mapNodes

    def getImagePath(self,val):
        if val == 1:
            return self.imageDenseForest
        elif val == 2:
            return self.imageForest
        elif val == 3:
            return self.imagePath
        elif val == 4:
            return self.imageClearing