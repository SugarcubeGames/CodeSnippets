#Constant holder for values
import math
import debug as db
import pygame

class vals:
    screenWidth = 960
    screenHeight = 540
    mapLineThickness = 3
    mapInnerCircleRadius = 48
    mapOuterCircleRadius = 32
    mapOrbitRadius = 120
    mapOffsetX = 0.0 #X offset for the 45d orbiting map tiles
    mapOffsetY = 0.0
    mapConnectorInnerOffsetX = 0.0 #X offset for the inner endpoint of the connecting line.
    mapConnectorInnerOffsetY = 0.0
    mapConnectorOuterOffsetX = 0.0
    mapConnectorOuterOffsetY = 0.0
    mapInnerCircleColor = (255,255,255)
    mapOuterCircleColor = (65,32,140)
    mapConnectorCenterPointValues=[(0,0),(0,0),(0,0),(0,0),(0,0),(0,0),(0,0),(0,0)]
    
    boardLineColor = (109,108,112)
    boardTextColor = (146,144,150)
    boardStoryTextRect = pygame.Rect(0,0,10,10)
    boardStoryTextColor = (166,163,162)


    def __init__(self, sw, sh, mlt, micr, mocr, mor, micc, mocc, blc, btc):
        self.screenWidth = sw
        self.screenHeight = sh
        self.mapLineThickness = mlt
        self.mapInnerCircleRadius = micr
        self.mapOuterCircleRadius = mocr
        self.mapOrbitRadius = mor
        
        self.mapOffsetX = self.mapOrbitRadius*math.cos(math.radians(45))
        self.mapOffsetY = self.mapOrbitRadius*math.sin(math.radians(45))
        self.mapConnectorInnerOffsetX = self.mapInnerCircleRadius*math.cos(math.radians(45))
        self.mapConnectorInnerOffsetY = self.mapInnerCircleRadius*math.sin(math.radians(45))
        self.mapConnectorOuterOffsetX = (self.mapOrbitRadius-self.mapOuterCircleRadius)*math.cos(math.radians(45))
        self.mapConnectorOuterOffsetY = (self.mapOrbitRadius-self.mapOuterCircleRadius)*math.sin(math.radians(45))
        
        self.mapInnerCircleColor = micc
        self.mapOuterCircleColor = mocc
        self.boardLineColor = blc
        self.boardTextColor = btc
        
        x1=math.floor(self.screenWidth/3)
        x2=x1*2
        y1=math.floor(self.screenHeight/3)
        y2=y1*2
        
        #Build a list of center points for the map points.  Useful for image placement
        self.mapConnectorCenterPointValues[0] = (x2,y1-self.mapOrbitRadius)
        self.mapConnectorCenterPointValues[1] = (x2+self.mapOffsetX, y1-self.mapOffsetY)
        self.mapConnectorCenterPointValues[2] = (x2+self.mapOrbitRadius,y1)
        self.mapConnectorCenterPointValues[3] = (x2+self.mapOffsetX, y1+self.mapOffsetY)
        self.mapConnectorCenterPointValues[4] = (x2,y1+self.mapOrbitRadius)
        self.mapConnectorCenterPointValues[5] = (x2-self.mapOffsetX, y1+self.mapOffsetY)
        self.mapConnectorCenterPointValues[6] = (x2-self.mapOrbitRadius,y1)
        self.mapConnectorCenterPointValues[7] = (x2-self.mapOffsetX, y1-self.mapOffsetY)
        
        self.boardStoryTextRect = pygame.Rect(x1+8, y2+8,(x1*2)-24,y1-24)
