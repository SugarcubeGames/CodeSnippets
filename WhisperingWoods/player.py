import pygame

class player:
    
    #Each movement takes 15 minutes in game, so hours are multiples of four
    time = 36 #9am.  The game always starts at 9 am
    #What block of time is being shown:
    #  0: Night     : 23 - 7  : Moon, Sunrise
    #  1: Morning   : 5  - 13 : Sunrise, Sun  (Start in this block, at 9am)
    #  2: Afternoon : 11 - 19 : Sun, Moonrise
    #  3: Evening   : 17 - 1  : Moonrise, Moon
    timeblock = 1
    health = 10
    food = 10
    temp = 10
    
    #Track the last direction the player came from.  This is used during
    #map generation to ensure that the player can't go back that way
    #0:N, 1:NE, 2:W, 3:SE, 4:S, 5:SW, 6:W, 7:NW
    lastDir = 0
    
    #Icons
    hptic = pygame.image.load("resources/hptic.png")
    foodtic = pygame.image.load("resources/foodtic.png")
    tempticwarm = pygame.image.load("resources/tempticwarm.png")
    tempticcold = pygame.image.load("resources/tempticcold.png")
    tempticfrig = pygame.image.load("resources/tempticfrig.png")
    
    def __init__(self,t,h,f,tm):
        self.time = t
        self.health = h
        self.food = f
        self.temp = tm
        
        self.calcTimeblock()
        
    def updatePlayerStats(self, screen):
        self.drawStatTics(self.hptic, screen, self.health, (107,34))
        self.drawStatTics(self.foodtic, screen, self.food, (107,79))
        if self.temp < 4:
            self.drawStatTics(self.tempticfrig, screen, self.temp, (107,124))
        elif self.temp < 7:
            self.drawStatTics(self.tempticcold, screen, self.temp, (107,124))
        else:
            self.drawStatTics(self.tempticwarm, screen, self.temp, (107,124))
        
    def drawStatTics(self,img, screen, amnt, pos):
        for i in range(0,amnt):
            screen.blit(img, (pos[0]+(20*i),pos[1]))
            
    def calcTimeblock(self):
        if self.time < 7*4:
            self.timeblock = 0
        elif self.time < 13*4:
            self.timeblock = 1
        elif self.time < 19*4:
            self.timeblock = 2
        elif self.time < 24*4:
            self.timeblock = 3