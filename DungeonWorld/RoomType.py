from enum import Enum

class RoomType(Enum):
    ENTRY = 0
    EMPTY = 1
    ENEMY = 2
    TREASURE = 3
    HEALTH = 4
    BOSS = 5
    UNDEFINED = 6


#Room Types:
# 0: Entry room - No enemies or treasure, only first room
# 1: Empty room - No enemies or treasure, progressively less like
#                 to appear the deeper into the dungeon you go.
#                 Probably shouldn't appear past depth 4 or 5.
# 2: Enemy room - Room with an enemy to fight. Progressively more
#                 likely the deeper into the dungeon you go.  The
#                 deeper the room depth, the harder the enemy.
# 3: Treasure room - Room with treasure in it.  There should only
#                 be a few of these per dungeon (based on the
#                 total number of rooms in the dungeon.  I'm
#                 thinking 1/every 6 or 7 rooms.
# 4: Health Room - Room where you can replenish health and mana.
#                 There should only be one of these per dungeon.
#                 ideally at half-way mark.  (1/2 max depth)
# 5: Boss Room -  The room with the dungeon boss.  Always the
#                 deepest room in the dungeon.  If there are
#                 multiple deepest rooms then one is selected at
#                 random.