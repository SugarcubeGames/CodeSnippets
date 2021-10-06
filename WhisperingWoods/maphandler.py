from mapgen import mapGenerator
from mapnode import mapNode
from nodetypes import NodeTypes
from storytext import StoryText
from typing import List

class mapHandler:
    
    seed = 99999
    nodes = [] # type: List[mapNode]
    time = 36
    
    mapGen = mapGenerator(seed)
    
    genNewMap = True
    previousDirection = -1
    
    threatLevel = 10
    
    #Track if the player has clicked on a node.
    #This node will be highlighted, and it's
    #story text will be shown.  
    activeNode = -1
    
    storyHandler = StoryText()
    
    def __init__(self):
        self.time = 36
        self.previousDirection = -1
        
        self.calcThreatLevel()
        self.generateMap()
    
    def calcThreatLevel(self):
        #This will be figured out later
        self.threatLevel = 10
        
    def generateMap(self):
        mapReady = False
        
        while not mapReady:
            self.nodes = self.mapGen.genMap(self.time, self.previousDirection, self.storyHandler)
            nodeCount = 0
            for i in range(0,8):
                if self.nodes[i].nType != NodeTypes.EMPTY:
                    nodeCount += 1
                if nodeCount > 2:
                    mapReady = True
    
    def setActive(self, act):
        self.activeNode = act