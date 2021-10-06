import math
import pygame
import random
from mapnode import mapNode
from nodetypes import NodeTypes

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
        
    def genMap(self, time, previousDir, story):
        mapNodes = []
        
        for i in range(0,8):
            mapNodes.append(mapNode(i))
        
        for i in range (0,8):
            rand = random.randrange(0,100)
            #print(f'{i}: {rand}/{self.nodeProbability}  |  {rand<self.nodeProbability}')
            if rand>self.nodeProbability and i != previousDir:
                mapNodes[i].setType(NodeTypes.EMPTY,story,0)
        #Determine what sort of tile the the map is.
        for i in range(0,8):
            storyRand = random.randrange(0,10) #Used to select a 
            if(mapNodes[i].nType != NodeTypes.EMPTY):
                if i==previousDir:
                    mapNodes[i].setType(NodeTypes.PREV,story,storyRand)
                else:
                    #For testing, determine if this is forest or path
                    rand = random.randrange(0,100)
                    if(rand<self.nodeProbability*2):
                        mapNodes[i].setType(NodeTypes.PATH,story,storyRand)
                        #mapNodes[i].setStoryText(story.getPathStory(threat, rand.range(0,10)))
                    else:
                        mapNodes[i].setType(NodeTypes.FOREST,story,storyRand)
                    
        return mapNodes