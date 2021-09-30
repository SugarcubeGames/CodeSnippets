import debug as db
import math
import pygame

BCOL = (0,0,0)
t = 5

def drawscreenlayout(screen,w,h,font, val):
    x1 = math.floor(w/3)
    y1 = math.floor(h/3)
    y2 = y1*2
    
    drawgameareas(screen,w,h,x1,y1,y2,val.boardLineColor)
    drawtext(screen,font,val.boardTextColor)

def drawgameareas(screen,w,h,x1,y1,y2,COL):
    #Game Window
    pygame.draw.line(screen,COL,(x1+8,16),(w-16,16),t)
    pygame.draw.line(screen,COL,(x1+8,y2-8),(w-16,y2-8),t)
    pygame.draw.line(screen,COL,(x1+8,16),(x1+8,y2-8),t)
    pygame.draw.line(screen,COL,(w-16,16),(w-16,y2-8),t)
    #Text Area
    pygame.draw.line(screen,COL,(x1+8,y2+8),(w-16,y2+8),t)
    pygame.draw.line(screen,COL,(x1+8,h-16),(w-16,h-16),t)
    pygame.draw.line(screen,COL,(x1+8,y2+8),(x1+8,h-16),t)
    pygame.draw.line(screen,COL,(w-16,y2+8),(w-16,h-16),t)
    #Stat Window
    pygame.draw.line(screen,COL,(16,16),(x1-8,16),t)
    pygame.draw.line(screen,COL,(16,y1-8),(x1-8,y1-8),t)
    pygame.draw.line(screen,COL,(16,16),(16,y1-8),t)
    pygame.draw.line(screen,COL,(x1-8,16),(x1-8,y1-8),t)
    #ItemWindow
    pygame.draw.line(screen,COL,(16,y1+8),(x1-8,y1+8),t)
    pygame.draw.line(screen,COL,(16,y2+(y1/2)),(x1-8,y2+(y1/2)),t)
    pygame.draw.line(screen,COL,(16,y1+8),(16,y2+(y1/2)),t)
    pygame.draw.line(screen,COL,(x1-8,y1+8),(x1-8,y2+(y1/2)),t)
    #TimeWindow
    pygame.draw.line(screen,COL,(16,y2+(y1/2)+16),(x1-8,y2+(y1/2)+16),t)
    pygame.draw.line(screen,COL,(16,h-16),(x1-8,h-16),t)
    pygame.draw.line(screen,COL,(16,y2+(y1/2)+16),(16,h-16),t)
    pygame.draw.line(screen,COL,(x1-8,y2+(y1/2)+16),(x1-8,h-16),t)
    
def drawtext(screen, font, COL):
    hptxt =  font.render("HP:", True, COL)
    screen.blit(hptxt, (24,30))
    foodtxt =  font.render("FOOD:", True, COL)
    screen.blit(foodtxt, (24,75))
    heattxt =  font.render("TEMP:", True, COL)
    screen.blit(heattxt, (24,120))
        
#mapArray is an 8 long bit array that denotes whether or not a map tile should be drawn
def drawmap(screen, w, h, val, mapArray):
    x = (math.floor(w/3)*2)
    y = math.floor(h/3)
    
    tk = val.mapLineThickness #Line Thickness
    micr = val.mapInnerCircleRadius #mapInnerCircleRadius
    mocr = val.mapOuterCircleRadius #radius of the outer map circles
    mor = val.mapOrbitRadius #outer radius of the surrounding map points
    
    #Since there are only eight circles surrouding this, there are only four points that
    #need to be calculated which comprise combinations of positive and negative values
    #for two numbers.  Calculation moved to vals
    mox = val.mapOffsetX
    moy = val.mapOffsetY
    #Same logic for the lines between points
    mciox = val.mapConnectorInnerOffsetX #outer offset
    mcioy = val.mapConnectorInnerOffsetY #outer offset
    mcoox = val.mapConnectorOuterOffsetX
    mcooy = val.mapConnectorOuterOffsetY
    
    #Get colors
    micc = val.mapInnerCircleColor
    mocc = val.mapOuterCircleColor
    
    #Center
    pygame.draw.circle(screen,micc,(x,y),micr,t,True,True,True,True)
    
    for i in range(0,8):
        if mapArray[i]:
            drawMapPoint(screen,mocc,val.mapPointCenters[i],val.mapPointLinePos[i],mocr,t)
                         
def drawMapPoint(screen,color,pos,lineends,mocr,tk):
    pygame.draw.circle(screen,color,pos,mocr,tk,True,True,True,True)
    pygame.draw.line(screen,color,lineends[0],lineends[1],tk)
    
def drawTimeCounter(screen,color,player,sun,moon,sunrise,moonrise,timearrow):
    nt = 9 #Number of tics
    tl = 304 #Total length of the time bar
    ih = 507 #Icon Height

    sx = 28
    lts = tl/nt #Large (hour) tic separation (in pixels)
    lth = 15 #Large tic height
    ltt = 3 #Large tic thickness
    sts = lts/4 #Small (15 minute) tic separation
    sth = 7 #Small tic height
    stt = 2 #small tic thickness
    for i in range(0,nt):
        pygame.draw.line(screen, color, (sx+(i*lts),ih),(sx+(i*lts),ih-lth),ltt)
    for i in range(0,(nt*4)-4):
        pygame.draw.line(screen, color, (sx+(i*sts),ih),(sx+(i*sts),ih-sth),stt)
    
    #Draw icons based on the time block
    if player.timeblock == 0:
        #print("Draw Night")
        drawTimeIcons(screen,sx,lts,ih-lth-20,moon,sunrise)
        drawCurrentTime(screen,sx,sts,player.time+4,timearrow)
    elif player.timeblock == 1:
        #print("Draw Morning")
        drawTimeIcons(screen,sx,lts,ih-lth-20,sunrise,sun)
        drawCurrentTime(screen,sx,sts,player.time+4-24,timearrow)
        #+4 above is because the timebar starts one hour before the current time block
    elif player.timeblock == 2:
        #print("Draw Afternoon")
        drawTimeIcons(screen,sx,lts,ih-lth-20,sun,moonrise)
        drawCurrentTime(screen,sx,sts,player.time+4-48,timearrow)
    else:
        #print("Draw Night")
        drawTimeIcons(screen,sx,lts,ih-lth-20,moonrise,moon)
        drawCurrentTime(screen,sx,sts,player.time+4-64,timearrow)
        
        
#these icons are 16x16
#sx: Start X from above, lts: Large tile Separation, ih: Icon Height
def drawTimeIcons(screen, sx, lts, ih, icon1, icon2):
    screen.blit(icon1, ((sx-8)+lts,ih))
    screen.blit(icon2, ((sx-8)+(lts*7),ih))

#to: Time offset (number of steps into the current timeblock
def drawCurrentTime(screen, sx, sts, to, timearrow):
    screen.blit(timearrow, ((sx-4)+(sts*to),512))
    
#draw text to the narrative box
def drawNarrativeText(screen, string, rect, font,color):
    #Attempting to implement multiline text function found:
    #https://stackoverflow.com/questions/42014195/rendering-text-with-multiple-lines-in-pygame
    words=[word.split(' ') for word in string.splitlines()]
    space=font.size(' ')[0]
    maxWidth = rect.width-48
    x = rect.x+24
    lines = []
    curLine = -1
    lineHeight = 0
    for line in words:
        curLine+=1
        lines.append("")
        for word in line:
            word_surface = font.render(word,0,color)
            word_width, word_height = word_surface.get_size()
            if lineHeight == 0:
                lineHeight = word_height
            if (x+word_width)-(rect.x+24) > maxWidth:
                x = rect.x + 24
                curLine+=1
                lines.append("")
            x += word_width+space
            lines[curLine] += word + " "
        x=rect.x+24
    firstLineY = 0
    numLines=len(lines)
    #Determine if there's an even or odd number of lines
    if len(lines)%2 == 0:
        firstLineY = rect.center[1]-((numLines/2)*lineHeight-12)
    else:
        firstLineY = rect.center[1]-((((numLines-1)/2)*lineHeight)+(lineHeight/2)-12)
    
    for line in lines:
        tx = font.render(line,True,color)
        textRect = tx.get_rect(center=(rect.center[0],firstLineY))
        screen.blit(tx,textRect)
        
        firstLineY+=lineHeight 
