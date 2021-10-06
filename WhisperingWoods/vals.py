#Constant holder for values
import math
import pygame

class vals:
    screenWidth = 960
    screenHeight = 540
    mapLineThickness = 2
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
    mapOuterCircleHighlightColor = (123,81,219)
    mapPointCenters=[(0.0,0.0),(0.0,0.0),(0.0,0.0),(0.0,0.0),(0.0,0.0),(0.0,0.0),(0.0,0.0),(0.0,0.0)]
    mapPointLinePos=[[(0.0,0.0),(0.0,0.0)],[(0.0,0.0),(0.0,0.0)],[(0.0,0.0),(0.0,0.0)],[(0.0,0.0),(0.0,0.0)],
                     [(0.0,0.0),(0.0,0.0)],[(0.0,0.0),(0.0,0.0)],[(0.0,0.0),(0.0,0.0)],[(0.0,0.0),(0.0,0.0)]]
    
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
        self.mapPointCenters[0] = (x2,y1-self.mapOrbitRadius)
        self.mapPointCenters[1] = (x2+self.mapOffsetX, y1-self.mapOffsetY)
        self.mapPointCenters[2] = (x2+self.mapOrbitRadius,y1)
        self.mapPointCenters[3] = (x2+self.mapOffsetX, y1+self.mapOffsetY)
        self.mapPointCenters[4] = (x2,y1+self.mapOrbitRadius)
        self.mapPointCenters[5] = (x2-self.mapOffsetX, y1+self.mapOffsetY)
        self.mapPointCenters[6] = (x2-self.mapOrbitRadius,y1)
        self.mapPointCenters[7] = (x2-self.mapOffsetX, y1-self.mapOffsetY)
        
        #Build a list of line endpoints for the connector lines
        self.mapPointLinePos[0] = [(x2,y1-self.mapInnerCircleRadius),
                                   (x2,y1-(self.mapOrbitRadius-self.mapOuterCircleRadius))]
        self.mapPointLinePos[1] = [(x2+self.mapConnectorInnerOffsetX,y1-self.mapConnectorInnerOffsetY),
                                   (x2+self.mapConnectorOuterOffsetX,y1-self.mapConnectorOuterOffsetY)]
        self.mapPointLinePos[2] = [(x2+self.mapInnerCircleRadius,y1),
                                   (x2+(self.mapOrbitRadius-self.mapOuterCircleRadius), y1)]
        self.mapPointLinePos[3] = [(x2+self.mapConnectorInnerOffsetX,y1+self.mapConnectorInnerOffsetY),
                                   (x2+self.mapConnectorOuterOffsetX,y1+self.mapConnectorOuterOffsetY)]
        self.mapPointLinePos[4] = [(x2,y1+self.mapInnerCircleRadius),
                                   (x2,y1+(self.mapOrbitRadius-self.mapOuterCircleRadius))]
        self.mapPointLinePos[5] = [(x2-self.mapConnectorInnerOffsetX,y1+self.mapConnectorInnerOffsetY),
                                   (x2-self.mapConnectorOuterOffsetX,y1+self.mapConnectorOuterOffsetY)]
        self.mapPointLinePos[6] = [(x2-self.mapInnerCircleRadius,y1),
                                   (x2-(self.mapOrbitRadius-self.mapOuterCircleRadius), y1)]
        self.mapPointLinePos[7] = [(x2-self.mapConnectorInnerOffsetX,y1-self.mapConnectorInnerOffsetY),
                                   (x2-self.mapConnectorOuterOffsetX,y1-self.mapConnectorOuterOffsetY)]
        
        self.boardStoryTextRect = pygame.Rect(x1+8, y2+8,(x1*2)-24,y1-24)

    def setMapCircleColor(self,col, col2):
        self.mapInnerCircleColor = col
        self.mapActiveNodeColor = col2
        
    def checkButtonClick(self,pos):
        #Iterate through each map button position and determine
        #if the click was within that button
        for i in range(0,8):
            distX = pos[0] - self.mapPointCenters[i][0]
            distY = pos[1] - self.mapPointCenters[i][1]
            if math.hypot(distX,distY)<self.mapOuterCircleRadius:
                return i
        return -1
