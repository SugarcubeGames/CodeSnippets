from storytext import *
from nodetypes import NodeTypes

class mapNode:
    
    direction = 0
    prevDirection = 0
    nType = NodeTypes.UNDECIDED
    pathBase = "resources/_.png"
    imagePath = pathBase.replace("_", "missing")
    lineColor = (255,0,255)
    story = ""
    
    def __init__(self,d):
        self.direction = d
        
        #Determine the node that will be grayed out
        #on the next map if this node is taken.
        if self.direction < 4:
            self.prevDirection = self.direction + 4
        else:
            self.prevDirection = self.direction - 4
            
    
    def setType(self, nt, storyHandler, rand):
        self.nType = nt
        
        #Set predefined variables based on the node type
        if self.nType == NodeTypes.EMPTY:
            self.lineColor = (0,0,0)
        elif self.nType == NodeTypes.PREV:
            self.updateImagePath("denseforest")
            self.lineColor = (50,47,56)
            self.story = storyText.getPreviousDirectionText(NodeTypes.PATH)
        elif self.nType == NodeTypes.FOREST:
            self.updateImagePath("forest")
            self.lineColor = (65,32,140)
        elif self.nType == NodeTypes.PATH:
            self.updateImagePath("path")
            self.lineColor = (145,93,36)
            self.story = storyHandler.getPathStory(8,rand)
        elif self.nType == NodeTypes.CLEARING:
            self.updateImagePath("clearing")
            self.lineColor = (116,147,161)
        elif self.nType == NodeTypes.ROSEGARDEN:
            self.lineColor = (232,9,9)
        elif self.nType == NodeTypes.GRAVEYARD:
            self.lineColor = (111,127,140)
        elif self.nType == NodeTypes.CHURCHYARD:
            self.lineColor = (166,226,237)
    
    def setStoryText(s):
        self.story = s
        
    def updateImagePath(self, imgName):
        self.imagePath = self.pathBase.replace("_", imgName)