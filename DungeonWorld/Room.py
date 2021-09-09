from RoomType import RoomType

class Room:
    #Depth defines difficulty
    depth, posx, posy = 0,0,0
    rt = RoomType.UNDEFINED
    
    def __init__(self, posx, posy, depth):
        self.posx = posx
        self.posy = posy
        self.depth = depth
        
    def settype(self, roomType):
        self.rt = roomType
        #print(self.rt.name)