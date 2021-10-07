import math
import pygame
from nodetypes import NodeTypes

class StoryText:
    
    pathThreat10 = ["The path is well-worn and quiet.",
                    "The well-lit path looks inviting."]
    pathThreat20 = ["The path looks clear.  I think I hear something in the leaves..."]
    pathThreat30 = ["The path is scattered with leaves.  I hear whispers in the branches..."]
    
    
    forestThreat10 = ["The forst is still, all I hear is wind in the leaves."]
    
    def __init__(self):
        print("storyText init.  Not sure this should ever display")
    
    #Since all selections will not have the same amount of story, it
    #is necessary to convert the received random value into an
    #appropriately-sized value for the given list.
    #val will be 1-10
    def calcChoice(self,val, length):
        #print(f"length: {length}\nval: {val}\nCalc: 10/length : {10/length}\n      val/adjLength : {val/(10/length)}\nFinal: {math.floor(val/(10/length))}")
        adjLength = 10/length #Determine how many val steps per story element
        return math.floor(val/adjLength)
        
    def getPreviousDirectionText(self,prevType):
         if prevType == NodeTypes.PATH:
             return "The path is gone, swallowed by trees and voices..."
    
    #rand is a value generated during node generation and determines which
    #of the options within a threat level is selected
    def getPathStory(self,threat, rand):
        if threat < 10:
            return self.pathThreat10[self.calcChoice(rand, len(self.pathThreat10))]
        else:
            return self.pathThreat20[self.calcChoice(rand, len(self.pathThreat20))]