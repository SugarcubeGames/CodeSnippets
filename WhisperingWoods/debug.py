import math
import pygame

COL = (251,0,251)

def drawboundary(screen,w,h):
    #8 pixel boundary
    pygame.draw.line(screen,COL,(0,8),(w,8),1)
    pygame.draw.line(screen,COL,(0,h-8),(w,h-8),1)
    pygame.draw.line(screen,COL,(8,0),(8,h),1)
    pygame.draw.line(screen,COL,(w-8,0),(w-8,h),1)
    
def drawlayoutdivider(screen,w,h):
    x1 = math.floor(w/3)
    x2 = x1*2
    y1 = math.floor(h/3)
    y2 = y1*2
    
    #1/3 divider
    pygame.draw.line(screen,COL,(x1,0),(x1,h),1)
    pygame.draw.line(screen,COL,(x2,0),(x2,h),1)
    pygame.draw.line(screen,COL,(0,y1),(w,y1),1)
    pygame.draw.line(screen,COL,(0,y2),(w,y2),1)
    
def drawgameareas(screen,w,h):
    x1 = math.floor(w/3)
    y1 = math.floor(h/3)
    y2 = y1*2
    
    #Game Window
    pygame.draw.line(screen,COL,(x1+8,16),(w-16,16),1)
    pygame.draw.line(screen,COL,(x1+8,y2-8),(w-16,y2-8),1)
    pygame.draw.line(screen,COL,(x1+8,16),(x1+8,y2-8),1)
    pygame.draw.line(screen,COL,(w-16,16),(w-16,y2-8),1)
    #Text Area
    pygame.draw.line(screen,COL,(x1+8,y2+8),(w-16,y2+8),1)
    pygame.draw.line(screen,COL,(x1+8,h-16),(w-16,h-16),1)
    pygame.draw.line(screen,COL,(x1+8,y2+8),(x1+8,h-16),1)
    pygame.draw.line(screen,COL,(w-16,y2+8),(w-16,h-16),1)
    #Stat Window
    pygame.draw.line(screen,COL,(16,16),(x1-8,16),1)
    pygame.draw.line(screen,COL,(16,y1-8),(x1-8,y1-8),1)
    pygame.draw.line(screen,COL,(16,16),(16,y1-8),1)
    pygame.draw.line(screen,COL,(x1-8,16),(x1-8,y1-8),1)
    #ItemWindow
    pygame.draw.line(screen,COL,(16,y1+8),(x1-8,y1+8),1)
    pygame.draw.line(screen,COL,(16,y2+(y1/2)),(x1-8,y2+(y1/2)),1)
    pygame.draw.line(screen,COL,(16,y1+8),(16,y2+(y1/2)),1)
    pygame.draw.line(screen,COL,(x1-8,y1+8),(x1-8,y2+(y1/2)),1)
    #TimeWindow
    pygame.draw.line(screen,COL,(16,y2+(y1/2)+16),(x1-8,y2+(y1/2)+16),1)
    pygame.draw.line(screen,COL,(16,h-16),(x1-8,h-16),1)
    pygame.draw.line(screen,COL,(16,y2+(y1/2)+16),(16,h-16),1)
    pygame.draw.line(screen,COL,(x1-8,y2+(y1/2)+16),(x1-8,h-16),1)
    
def drawmaplines(screen,x,y, outr):
    #Since there are only eight circles surrouding this, there are only four points that
    #need to be calculated which comprise combinations of positive and negative values
    #for two numbers
    offx = outr*math.cos(math.radians(45))
    offy = outr*math.sin(math.radians(45))
    
    pygame.draw.line(screen, COL, (x,y), (x, y-outr), 1)
    pygame.draw.line(screen, COL, (x,y), (x+offx, y-offy),1)
    pygame.draw.line(screen, COL, (x,y), (x+outr, y), 1)
    pygame.draw.line(screen, COL, (x,y), (x+offx, y+offy),1)
    pygame.draw.line(screen, COL, (x,y), (x, y+outr), 1)
    pygame.draw.line(screen, COL, (x,y), (x-offx, y+offy),1)
    pygame.draw.line(screen, COL, (x,y), (x-outr, y), 1)
    pygame.draw.line(screen, COL, (x,y), (x-offx, y-offy),1)

def drawTextArea(screen,rect):
    pygame.draw.line(screen, COL, (rect.x,rect.y),(rect.x+rect.width,rect.y),1)
    pygame.draw.line(screen, COL, (rect.x,rect.y+rect.height),(rect.x+rect.width,rect.y+rect.height),1)
    pygame.draw.line(screen, COL, (rect.x,rect.y),(rect.x,rect.y+rect.height),1)
    pygame.draw.line(screen, COL, (rect.x+rect.width,rect.y),(rect.x+rect.width,rect.y+rect.height),1)
    
