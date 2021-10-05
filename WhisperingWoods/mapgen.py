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

    #Node types:
    # 0: Empty
    # 1: Previous Direction
    # 2: Forest
    # 3: Path
    # 4: Clearing
    # 5: Firepit
    # 20: Rose Garden
    # 40: Graveyard
    # 60: Well
    colorBlack = (0,0,0)
    colorPreviousDirection = (50,47,56)
    colorForest = (65,32,140)
    colorPath = (145,93,36)
    colorRoseGarden = (232,9,9)
    colorGraveyard = (111,127,140)
    
    #Images for Map Node Types
    imageClearing = ""
    imageDenseForest = ""
    imageForest = ""
    imagePath = ""
    

    def __init__(self, seed):
        self.seed = seed
        random.seed(self.seed)
        
        self.imageClearing = "resources/clearing.png"
        self.imageDenseForest = "resources/denseforest.png"
        self.imageForest = "resources/forest.png"
        self.imagePath = "resources/path.png"
        
    def genMap(self, time, previousDir):
        mapNodes = [0,0,0,0,0,0,0,0]
        mapColors = [(0,0,0),(0,0,0),(0,0,0),(0,0,0),(0,0,0),(0,0,0),(0,0,0),(0,0,0)]
        mapTypes = [0,0,0,0,0,0,0,0]
        nodes = []
        
        for i in range(0,8):
            mapColors[i] = self.colorBlack
            nodes.append(mapNode(i))
        
        for i in range (0,8):
            rand = random.randrange(0,100)
            #print(f'{i}: {rand}/{self.nodeProbability}  |  {rand<self.nodeProbability}')
            if rand<self.nodeProbability or i==previousDir:
                mapNodes[i] = 1
            else:
                nodes[i].setType(NodeTypes.EMPTY)
        
        #Determine what sort of tile the the map is.
        for i in range(0,8):
            if(mapNodes[i] == 1):
                if i==previousDir:
                    mapColors[i] = self.colorPreviousDirection
                    mapTypes[i] = 1
                    nodes[i].setType(NodeTypes.PREV)
                else:
                    #For testing, determine if this is forest or path
                    rand = random.randrange(0,100)
                    if(rand<self.nodeProbability*2):
                        mapColors[i] = self.colorPath
                        mapTypes[i]  = 3
                        nodes[i].setType(NodeTypes.PATH)
                    else:
                        mapColors[i] = self.colorForest
                        mapTypes[i] = 2
                        nodes[i].setType(NodeTypes.FOREST)
                    
        return [mapNodes,mapColors,mapTypes,nodes]

    def getImagePath(self,val):
        if val == 1:
            return self.imageDenseForest
        elif val == 2:
            return self.imageForest
        elif val == 3:
            return self.imagePath
        elif val == 4:
            return self.imageClearing