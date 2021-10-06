import pygame
import debug as dbg
import drawscreen as ds
from player import player
from vals import vals
from maphandler import mapHandler
from nodetypes import NodeTypes

#Initialize pygame
pygame.init()
val = vals(960, 540, 3, 64, 33, 120,(26,99,9),(65,32,140),(109,108,112),(146,144,150))
val.setMapCircleColor((26,99,9),(36,140,13))
pl = player(36,10,8,10)

seed=99999 #Temp seed for map gen

#Create the screen
screen = pygame.display.set_mode((val.screenWidth,val.screenHeight))

#Title and Icon
pygame.display.set_caption("Whispering Woods")
icon = pygame.image.load("resources/icon.png")
pygame.display.set_icon(icon)

#Other Icons
iconSun = pygame.image.load("resources/sunsmall.png")
iconMoon = pygame.image.load("resources/moonsmall.png")
iconSunrise = pygame.image.load("resources/sunrisesmall.png")
iconMoonrise = pygame.image.load("resources/moonrisesmall.png")
iconTimeArrow = pygame.image.load("resources/timearrow.png")

#Font
myfont = pygame.font.SysFont('Arial',32)
storyFont = pygame.font.SysFont('Arial', 24)

curStoryText = testString="I have been lost in these woods for three days.  The bells ring out the hour, they are the only respite from the whispers.  The trees fall silent while they chime, all the forest falls silent.  I am getting closer, or they are, I can't tell anymore.  If I can reach the bells I will be safe..."
#Does the screen need to be updated?
screenNeedsUpdate = True
textNeedsUpdated = True
debugDraw = False

handler = mapHandler()
genNewMap = True;


#Game Loop
running = True
while running:
    
    for event in pygame.event.get():
        if event.type == pygame.QUIT:
            running = False
        if event.type == pygame.KEYDOWN:
            if event.key == pygame.K_KP_ENTER:
                genNewMap = True
                handler.setActive(-1)
        if event.type == pygame.MOUSEBUTTONDOWN:
            clickedBtn = val.checkButtonClick(event.pos)
            if clickedBtn > -1:
                if handler.nodes[clickedBtn].nType != NodeTypes.EMPTY:
                    if clickedBtn != handler.activeNode:
                        handler.setActive(clickedBtn)
                    if handler.nodes[clickedBtn].nType == NodeTypes.PREV:
                        handler.setActive(-1) #Deactivate the active node
                    
                    curStoryText = handler.nodes[clickedBtn].story
                    textNeedsUpdated = True
                    screenNeedsUpdate = True
            
    if genNewMap:
        handler.generateMap()
        screenNeedsUpdate = True
        genNewMap = False
        
    if screenNeedsUpdate:
        screen.fill((0,0,0))
        ds.drawscreenlayout(screen,val.screenWidth,val.screenHeight,myfont,val)
        
        pl.updatePlayerStats(screen)
        ds.drawMap(screen,val,handler)
        #Todo: Redo the time coutner to the sun/moon concept where it's a singlel image on
        #a sprite sheet and hours are not clearly marked.
        ds.drawTimeCounter(screen,(255,255,255),pl,iconSun,iconMoon,iconSunrise,iconMoonrise,iconTimeArrow)
        
        ds.drawNarrativeText(screen, curStoryText, val.boardStoryTextRect, storyFont, val.boardStoryTextColor)
        
        pygame.display.update()
        screenNeedsUpdate = False;
        
    if debugDraw:
        dbg.drawboundary(screen, val.screenWidth,val.screenHeight)
        dbg.drawlayoutdivider(screen, val.screenWidth,val.screenHeight)
        dbg.drawgameareas(screen, val.screenWidth,val.screenHeight)
        dbg.drawTextArea(screen,val.boardStoryTextRect)
        dbg.drawTestMovementIcon(screen,val)
        pygame.display.update()
            
#On exiting the loop, close the screen
pygame.display.quit()
        
storyBlurb="I have been lost in these woods for three days.  The bells ring out the hour, they are the only respite from the whispers.  The trees fall silent while they chime, all the forest falls silent.  I am getting closer, or they are, our movements don't seem to align.  If I can reach the bells I will be safe..."