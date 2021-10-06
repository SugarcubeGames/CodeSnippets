from enum import Enum

class NodeTypes(Enum):
    UNDECIDED = -1
    EMPTY = 0
    PREV = 1 #Previous Direction
    FOREST = 2
    PATH = 3
    CLEARING = 4
    FIREPIT = 5
    ROSEGARDEN = 20
    GRAVEYARD = 40
    WELL = 60
    CHURCHYARD = 100