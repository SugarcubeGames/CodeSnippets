import pygame
import debug as dbg
import drawscreen as ds
from player import player
from vals import vals

#Initialize pygame
pygame.init()
val = vals(960, 540, 3, 48, 28, 120,(26,99,9),(65,32,140),(109,108,112),(146,144,150))
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
        screen.fill((0,0,0))import pygame
import debug as dbg
import drawscreen as ds
from player import player
from vals import vals

#Initialize pygame
pygame.init()
val = vals(960, 540, 3, 48, 28, 120,(26,99,9),(65,32,140),(109,108,112),(146,144,150))
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
        #ds.drawhp(screen,hptic,10)
        #ds.drawfood(screen,foodtic,10)
        #ds.drawheat(screen,heattic,10)
        pl.updatePlayerStats(screen)
        ds.drawmap(screen,swidth,sheight,val,[1,0,1,0,1,0,0,1])
        ds.drawTimeCounter(screen,(255,255,255),pl,iconSun,iconMoon,iconSunrise,iconMoonrise,iconTimeArrow)
        
        #testText = storyFont.render("This is a lot of words that should exceed the capacity of the text box.  But it didn't, so I have to add way more.\nHopefully this will be on a new line.", True, val.boardStoryTextColor)
        #testString="This is a lot of words that should exceed the capacity of the text box.  But it didn't, so I have to add way more.  Now I'm adding a ton more text to get it up to three lines.  Now I need to add a whole lot more to try to get this to five lines to make sure that it still works properly."
        testString = "This is one line of text."
        testString = "This is one line of text..................................................................... This is two lines of text"
        testString = "This is one line of text..................................................................... This is two lines of text.................................................................... This is three lines of text"
        testString = "This is one line of text..................................................................... This is two lines of text.................................................................... This is three lines of text.................................................................... This is four lines of text"
        testString = "This is one line of text..................................................................... This is two lines of text.................................................................... This is three lines of text.................................................................... This is four lines of text.................................................................... This is five lines of text"
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
        ds.drawscreenlayout(screen,swidth,sheight,myfont,val)
        #ds.drawhp(screen,hptic,10)
        #ds.drawfood(screen,foodtic,10)
        #ds.drawheat(screen,heattic,10)
        pl.updatePlayerStats(screen)
        ds.drawmap(screen,swidth,sheight,val,[1,0,1,0,1,0,0,1])
        ds.drawTimeCounter(screen,(255,255,255),pl,iconSun,iconMoon,iconSunrise,iconMoonrise,iconTimeArrow)
        
        #testText = storyFont.render("This is a lot of words that should exceed the capacity of the text box.  But it didn't, so I have to add way more.\nHopefully this will be on a new line.", True, val.boardStoryTextColor)
        testString="This is a lot of words that should exceed the capacity of the text box.  But it didn't, so I have to add way more.  Now I'm adding a ton more text to get it up to three lines."
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
