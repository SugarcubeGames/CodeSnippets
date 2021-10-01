import pygame
import debug as dbg
import drawscreen as ds
from player import player
from vals import vals

#Initialize pygame
pygame.init()
val = vals(960, 540, 3, 64, 33, 120,(26,99,9),(65,32,140),(109,108,112),(146,144,150))
pl = player(36,10,8,10)

swidth=val.screenWidth
sheight=val.screenHeight
#Create the screen
screen = pygame.display.set_mode((swidth,sheight))

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

#Does the screen need to be updated?
screenNeedsUpdate = True
debugDraw = False

#Game Loop
running = True
while running:
    for event in pygame.event.get():
        if event.type == pygame.QUIT:
            running = False
    
    if screenNeedsUpdate:
        screen.fill((0,0,0))
        ds.drawscreenlayout(screen,swidth,sheight,myfont,val)
        
        #dbg.drawTestMovementIcon(screen,val)
        
        pl.updatePlayerStats(screen)
        ds.drawmap(screen,swidth,sheight,val,[1,1,1,1,1,1,1,1])
        ds.drawTimeCounter(screen,(255,255,255),pl,iconSun,iconMoon,iconSunrise,iconMoonrise,iconTimeArrow)
        
        #testString = "Whispering Woods"
        testString="I have been lost in these words for three days.  The bells ring out the hour, they are the only respite from the whispers.  The trees fall silent while they chime, all the forest falls silent.  I am getting closer, or they are, our movements don't seem to align.  If I can reach the bells I will be safe..."
        ds.drawNarrativeText(screen, testString, val.boardStoryTextRect, storyFont, val.boardStoryTextColor)
        
        pygame.display.update()
        screenNeedsUpdate = False;
        
    if debugDraw:
        dbg.drawboundary(screen, swidth, sheight)
        dbg.drawlayoutdivider(screen, swidth, sheight)
        dbg.drawgameareas(screen, swidth, sheight)
        dbg.drawTextArea(screen,val.boardStoryTextRect)
        pygame.display.update()
    
            
#On exiting the loop, close the screen
pygame.display.quit()

storyBlurb="I have been lost in these words for three days.  The bells ring out the hour, they are the only respite from the whispers.  The trees fall silent while they chime, all the forest falls silent.  I am getting closer, or they are, our movements don't seem to align.  If I can reach the bells I will be safe..."
